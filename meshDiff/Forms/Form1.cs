// author: Josef Pelikan, modified by Jan Horesovsky

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MathSupport;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Scene3D;
using System.Threading;
using System.Diagnostics;

namespace meshDiff
{
	public partial class Form1 : Form
	{
		#region Fields

		// a text representation of fields which can be stored to a file
		const string fileSectionName = "Form Settings";

		const string clusteringTypeArrowsName = "clusteringTypeArrows";
		const string clusteringTypeColorsName = "clusteringTypeColors";
		const string colorVisName = "colorVisualization";
		const string arrowVisName = "arrowVisualization";

		/// <summary>
		/// Left scene read from file.
		/// </summary>
		SceneBrep scene1 = new SceneBrep();

		/// <summary>
		/// Right scene read from file.
		/// </summary>
		SceneBrep scene2 = new SceneBrep();

		/// <summary>
		/// The colored model of the left visualization.
		/// </summary>
		SceneBrep scene1Color = new SceneBrep();

		/// <summary>
		/// The colored model of the right visualization.
		/// </summary>
		SceneBrep scene2Color = new SceneBrep();

		/// <summary>
		/// The arrows of the left visualization.
		/// </summary>
		SceneBrep scene1Arrows = new SceneBrep();

		/// <summary>
		/// The arrows of the right visualization.
		/// </summary>
		SceneBrep scene2Arrows = new SceneBrep();

		/// <summary>
		/// Scene currently being shown in the left GLControl.
		/// </summary>
		SceneBrep currentScene1 = new SceneBrep();

		/// <summary>
		/// Scene currently being shown in the right GLControl.
		/// </summary>
		SceneBrep currentScene2 = new SceneBrep();

		/// <summary>
		/// Visualization currently being shown in the left GLControl.
		/// </summary>
		SceneBrep currentVisualization1 = null;

		/// <summary>
		/// Visualization currently being shown in the right GLControl.
		/// </summary>
		SceneBrep currentVisualization2 = null;

		// diff classes for current scenes for each clustering type
		DiffAlgo<ClusteringNone> diffNoClustering = new DiffAlgo<ClusteringNone>(null, null, ClusteringNone.Create);
		DiffAlgo<ClusteringSimple> diffSimpleClustering = new DiffAlgo<ClusteringSimple>(null, null, ClusteringSimple.Create);
		DiffAlgo<ClusteringSigned> diffSignedClustering = new DiffAlgo<ClusteringSigned>(null, null, ClusteringSigned.Create);

		/// <summary>
		/// The metric currently being used for obtaining arrows from both scenes.
		/// </summary>
		MetricType currentMetric = default(MetricType);

		/// <summary>
		/// The parameters currently being used for clustering in color visualization.
		/// </summary>
		ClusteringParameters clusteringParametersColors = new ClusteringParametersColors();

		/// <summary>
		/// The parameters currently being used for clustering in arrow visualization.
		/// </summary>
		ClusteringParameters clusteringParametersArrows = new ClusteringParametersArrows();

		/// <summary>
		/// The parameters currently being used for visualization.
		/// </summary>
		VisualizerParameters visualizerParameters = new VisualizerParameters();

		/// <summary>
		/// Scene center point.
		/// </summary>
		Vector3 center = Vector3.Zero;

		/// <summary>
		/// Scene diameter.
		/// </summary>
		float diameter = 4.0f;

		float near = 0.1f;
		float far = 5.0f;

		/// <summary>
		/// Global light source.
		/// </summary>
		Vector3 light = new Vector3(-2, 1, 1);

		Vector3? pointOrigin = null;
		Vector3 pointTarget;
		Vector3 eye;

		/// <summary>
		/// Frustum vertices, 0 or 8 vertices
		/// </summary>
		List<Vector3> frustumFrame = new List<Vector3>();

		/// <summary>
		/// Left GLControl guard flag.
		/// </summary>
		bool control1Loaded = false;

		/// <summary>
		/// Right GLControl guard flag
		/// </summary>
		bool control2Loaded = false;

		/// <summary>
		/// A trackball instance for the left GLControl.
		/// </summary>
		Trackball trackball1 = null;

		/// <summary>
		/// A trackball instance for the right GLControl.
		/// </summary>
		Trackball trackball2 = null;

		/// <summary>
		/// Global ToolTip instance (for reuse).
		/// </summary>
		ToolTip tt = new ToolTip();

		/// <summary>
		/// For debugging.
		/// </summary>
		SceneBrep.Decoration dec = new SceneBrep.Decoration();

		Cursor savedCursor;

		#endregion

		public Form1()
		{
			InitializeComponent();

			// Trackballs:
			trackball1 = new Trackball(TrackballType.Left, center, diameter);
			trackball2 = new Trackball(TrackballType.Right, center, diameter);

			InitShaderRepository();

			clusterCountArrowsTrackBar.Value = clusteringParametersArrows.ClusterCount;
			clusterCountColorsTrackBar.Value = clusteringParametersColors.ClusterCount;
			InitClusterComboBoxes();
			InitColorVisComboBox();
			InitArrowVisComboBox();
		}

		#region Helper Functions

		private SceneBrep LoadModel(string fileName)
		{
			if (fileName == null || fileName == "")
			{
				return null;
			}

			SceneBrep scene = new SceneBrep();
			
			string extension = Path.GetExtension(fileName);

			try
			{
				if (extension == ".ply")
				{
					StanfordPly plyReader;
					plyReader = new StanfordPly();
					plyReader.ReadBrep(fileName, scene);
				}
				else
				{
					WavefrontObj objReader;
					objReader = new WavefrontObj();
					objReader.MirrorConversion = false;
					objReader.ReadBrep(fileName, scene);
				}
			}
			// catch all exceptions related to reading files
			catch (Exception ex) when (
				(ex is FileNotFoundException) || (ex is DirectoryNotFoundException) ||
				(ex is PathTooLongException) || (ex is IOException) || (ex is System.Security.SecurityException) ||
				(ex is ArgumentException) || (ex is ArgumentOutOfRangeException) ||
				(ex is NotSupportedException)
			)
			{
				return null;
			}
			

			scene.BuildCornerTable();

			Debug.Assert(scene.CheckCornerTable(null, true) == 0);

			scene.SetGlobalColor(new Vector3(0.7f, 0.7f, 0.7f), false);
			scene.ComputeNormals();

			return scene;
		}

		private string ChoosePathToOpen(string dialogTitle = "Open Scene File")
		{
			OpenFileDialog ofd = new OpenFileDialog();

			ofd.Title = dialogTitle;
			ofd.Filter = "Wavefront OBJ Files|*.obj;*.obj.gz" + "|Polygon File Format|*.ply" +
						 "|All scene types|*.obj;*.ply";
			ofd.FilterIndex = 3;
			ofd.FileName = "";
			if (ofd.ShowDialog() != DialogResult.OK)
				return null;

			return ofd.FileName;
		}

		private void DrawScene(SceneBrep scene, int VBOIndex, Trackball trackball, GLControl glControl)
		{
			if (scene == null || scene.Vertices == 0)
			{
				return;
			}

			diameter = scene.GetDiameter(out center);
			trackball.Center = center;
			trackball.Diameter = diameter;
			SetLight(diameter, ref light);
			trackball.Reset();
			glControl.MakeCurrent();

			string fileLabelText = string.Format("File: {0} vertices, {1} edges ({2} shared), {3} faces",
												scene.Vertices, scene.statEdges, 
												scene.statShared, scene.Triangles);

			if (VBOIndex == 0)
			{
				currentScene1 = scene;
				currentVisualization1 = null;
				file1ValueLabel.Text = fileLabelText;
			}
			else
			{
				currentScene2 = scene;
				currentVisualization2 = null;
				file2ValueLabel.Text = fileLabelText;
			}

			PrepareDataBuffers(glControl, scene, VBOIndex);

			glControl.Invalidate();
		}

		// if successful, returns the path of the created file, otherwise returns null
		private string ExportModels(List<SceneBrep> scenes, string path = null)
		{
			SceneBrep sceneToExport = new SceneBrep();

			if (Helpers.CopyModelsInto(scenes, ref sceneToExport) == false)
			{
				return null;
			}

			if (sceneToExport.Vertices < 1)
			{
				return null;
			}

			if (path == null)
			{
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.Title = "Save file";
				sfd.Filter = "PLY Files|*.ply";
				sfd.AddExtension = true;
				sfd.FileName = "";
				if (sfd.ShowDialog() != DialogResult.OK)
				{
					return null;
				}

				path = sfd.FileName;
			}

			StanfordPly plyWriter = new StanfordPly();
			plyWriter.TextFormat = true;
			plyWriter.NativeNewLine = true;

			try
			{
				using (StreamWriter writer = new StreamWriter(path, false))
				{
					plyWriter.WriteBrep(writer, sceneToExport);
				}
			}
			// catch all exceptions related to writing files
			catch (Exception ex) when (
				(ex is UnauthorizedAccessException) || (ex is DirectoryNotFoundException) ||
				(ex is PathTooLongException) || (ex is IOException) || (ex is System.Security.SecurityException) ||
				(ex is ArgumentException)
			)
			{
				return null;
			}

			return path;
		}

		private bool ExportVisualization(SceneBrep colors, SceneBrep arrows, bool separately)
		{
			if (separately)
			{
				string path = null;

				if ((path = ExportModels(new List<SceneBrep> { colors })) != null)
				{
					string colorsFileName = Path.GetFileName(path);
					string[] colorsFileNameParts = colorsFileName.Split('.');
					string arrowsFileName = colorsFileNameParts[0] + ".arrows." + colorsFileNameParts[1];

					string arrowsPath = Path.Combine(Path.GetDirectoryName(path), arrowsFileName);

					if (ExportModels(new List<SceneBrep> { arrows }, arrowsPath) != null)
					{
						return true;
					}
				}

				return false;
			}

			if (ExportModels(new List<SceneBrep> { colors, arrows }) != null)
			{
				return true;
			}

			return false;
		}

		private void InitializeVisualizers()
		{
			if (scene1 != null && scene2 != null &&
				scene1.Vertices > 0 && scene2.Vertices > 0)
			{
				diffNoClustering = new DiffAlgo<ClusteringNone>(scene1, scene2, ClusteringNone.Create);
				diffSimpleClustering = new DiffAlgo<ClusteringSimple>(scene1, scene2, ClusteringSimple.Create);
				diffSignedClustering = new DiffAlgo<ClusteringSigned>(scene1, scene2, ClusteringSigned.Create);

				clusterCountArrowsTrackBar.Maximum = scene1.Vertices;
				clusterCountColorsTrackBar.Maximum = scene1.Vertices;
			}
		}

		private bool CursorOverGLControl1()
		{
			Point glControl1Location = glControl1.PointToScreen(glControl1.Location);

			if (Cursor.Position.X > glControl1Location.X && Cursor.Position.X < glControl1Location.X + glControl1.Width &&
				Cursor.Position.Y > glControl1Location.Y && Cursor.Position.Y < glControl1Location.Y + glControl1.Height)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool CursorOverGLControl2()
		{
			Point glControl2Location = glControl2.PointToScreen(glControl2.Location);

			if (Cursor.Position.X > glControl2Location.X && Cursor.Position.X < glControl2Location.X + glControl2.Width &&
				Cursor.Position.Y > glControl2Location.Y && Cursor.Position.Y < glControl2Location.Y + glControl2.Height)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void SetFromFile(string path)
		{
			using (ParameterReader paramReader = new ParameterReader(path, fileSectionName))
			{
				Tuple<string, string> paramPair;

				while ((paramPair = paramReader.ReadPair()) != null)
				{
					try
					{
						switch (paramPair.Item1)
						{
							case clusteringTypeArrowsName:
								clusteringArrowsComboBox.SelectedItem = Enum.Parse(typeof(ClusteringType), paramPair.Item2, true);
								break;
							case clusteringTypeColorsName:
								clusteringColorsComboBox.SelectedItem = Enum.Parse(typeof(ClusteringType), paramPair.Item2, true);
								break;
							case colorVisName:
								colorVisComboBox.SelectedItem = Enum.Parse(typeof(ColorVisType), paramPair.Item2, true);
								break;
							case arrowVisName:
								arrowVisComboBox.SelectedItem = Enum.Parse(typeof(YesNo), paramPair.Item2, true);
								break;
							default:
								break;
						}
					}
					catch (Exception ex) when (
						(ex is ArgumentOutOfRangeException) ||
						(ex is FormatException) ||
						(ex is ArgumentNullException) ||
						(ex is OverflowException) ||
						(ex is ArgumentException)
					)
					{
						clusteringArrowsComboBox.SelectedIndex = 0;
						clusteringColorsComboBox.SelectedIndex = 0;
						colorVisComboBox.SelectedIndex = 0;
						arrowVisComboBox.SelectedIndex = 1;

						return;
					}
				}
			}
		}
		
		private void SaveToFile(string path)
		{
			using (ParameterWriter paramWriter = new ParameterWriter(path, fileSectionName))
			{
				paramWriter.WritePair(clusteringTypeArrowsName, ((ClusteringType)clusteringArrowsComboBox.SelectedItem).ToString());
				paramWriter.WritePair(clusteringTypeColorsName, ((ClusteringType)clusteringColorsComboBox.SelectedItem).ToString());
				paramWriter.WritePair(colorVisName, ((ColorVisType)colorVisComboBox.SelectedItem).ToString());
				paramWriter.WritePair(arrowVisName, ((YesNo)arrowVisComboBox.SelectedItem).ToString());
				paramWriter.WriteEmptyLine();
			}
		}

		private void InitClusterComboBoxes()
		{
			clusteringArrowsComboBox.DataSource = Enum.GetValues(typeof(ClusteringType));
			clusteringArrowsComboBox.SelectedIndex = 0;

			clusteringColorsComboBox.DataSource = Enum.GetValues(typeof(ClusteringType));
			clusteringColorsComboBox.SelectedIndex = 0;
		}

		private void InitColorVisComboBox()
		{
			colorVisComboBox.DataSource = Enum.GetValues(typeof(ColorVisType));
			colorVisComboBox.SelectedIndex = 0;
		}

		private void InitArrowVisComboBox()
		{
			arrowVisComboBox.DataSource = Enum.GetValues(typeof(YesNo));
			arrowVisComboBox.SelectedIndex = 1;
		}

		#endregion

		#region UI Functions

		#region GLControl1 Functions

		private void glControl1_Load(object sender, EventArgs e)
		{
			glControl1.MakeCurrent();

			InitOpenGL();
			trackball1.GLsetupViewport(glControl1.Width, glControl1.Height, near, far);

			control1Loaded = true;
			Application.Idle += new EventHandler(Application_Idle);
		}

		private void glControl1_Resize(object sender, EventArgs e)
		{
			if (!control1Loaded) return;

			glControl1.MakeCurrent();
			trackball1.GLsetupViewport(glControl1.Width, glControl1.Height, near, far);
			glControl1.Invalidate();
		}

		private void glControl1_Paint(object sender, PaintEventArgs e)
		{
			glControl1.MakeCurrent();
			Render(glControl1, trackball1, currentScene1, currentVisualization1, 0, control1Loaded);
		}

		private void glControl1_MouseDown(object sender, MouseEventArgs e)
		{
			trackball1.MouseDown(e);

			if (pairedControlToolStripMenuItem.Checked)
			{
				trackball2.MouseDown(e);
			}
		}

		private void glControl1_MouseUp(object sender, MouseEventArgs e)
		{
			trackball1.MouseUp(e);

			if (pairedControlToolStripMenuItem.Checked)
			{
				trackball2.MouseUp(e);
			}
		}

		private void glControl1_MouseMove(object sender, MouseEventArgs e)
		{
			trackball1.MouseMove(e);

			if (pairedControlToolStripMenuItem.Checked)
			{
				trackball2.GetRotationFrom(trackball1);
			}
		}

		private void glControl1_MouseWheel(object sender, MouseEventArgs e)
		{
			trackball1.MouseWheel(e);
		}

		private void glControl1_KeyDown(object sender, KeyEventArgs e)
		{
			trackball1.KeyDown(e);
		}

		private void glControl1_KeyUp(object sender, KeyEventArgs e)
		{
			glControl1.MakeCurrent();
			trackball1.KeyUp(e);
		}

		#endregion

		#region GLControl2 Functions

		private void glControl2_Load(object sender, EventArgs e)
		{
			glControl2.MakeCurrent();
			InitOpenGL();
			trackball2.GLsetupViewport(glControl2.Width, glControl2.Height, near, far);

			control2Loaded = true;
		}

		private void glControl2_KeyDown(object sender, KeyEventArgs e)
		{
			trackball2.KeyDown(e);
		}

		private void glControl2_KeyUp(object sender, KeyEventArgs e)
		{
			glControl2.MakeCurrent();
			trackball2.KeyUp(e);
		}

		private void glControl2_MouseDown(object sender, MouseEventArgs e)
		{
			trackball2.MouseDown(e);

			if (pairedControlToolStripMenuItem.Checked)
			{
				trackball1.MouseDown(e);
			}
		}

		private void glControl2_MouseMove(object sender, MouseEventArgs e)
		{
			trackball2.MouseMove(e);

			if (pairedControlToolStripMenuItem.Checked)
			{
				trackball1.GetRotationFrom(trackball2);
			}
		}

		private void glControl2_MouseUp(object sender, MouseEventArgs e)
		{
			trackball2.MouseUp(e);

			if (pairedControlToolStripMenuItem.Checked)
			{
				trackball1.MouseUp(e);
			}
		}

		private void glControl2_MouseWheel(object sender, MouseEventArgs e)
		{
			trackball2.MouseWheel(e);
		}

		private void glControl2_Paint(object sender, PaintEventArgs e)
		{
			glControl2.MakeCurrent();
			Render(glControl2, trackball2, currentScene2, currentVisualization2, 2, control2Loaded);
		}

		private void glControl2_Resize(object sender, EventArgs e)
		{
			if (!control2Loaded) return;

			glControl2.MakeCurrent();
			trackball2.GLsetupViewport(glControl2.Width, glControl2.Height, near, far);
			glControl2.Invalidate();
		}

		#endregion

		private void Form1_MouseWheel(object sender, EventArgs e)
		{
			if ((CursorOverGLControl1() || CursorOverGLControl2()) &&
				pairedControlToolStripMenuItem.Checked)
			{
				glControl1_MouseWheel(sender, (MouseEventArgs)e);
				glControl2_MouseWheel(sender, (MouseEventArgs)e);
				return;
			}

			if (CursorOverGLControl1())
			{
				glControl1_MouseWheel(sender, (MouseEventArgs)e);
			}
			else if (CursorOverGLControl2())
			{
				glControl2_MouseWheel(sender, (MouseEventArgs)e);
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			if ((CursorOverGLControl1() || CursorOverGLControl2()) &&
				pairedControlToolStripMenuItem.Checked)
			{
				glControl1_KeyUp(sender, e);
				glControl2_KeyUp(sender, e);
				return;
			}

			if (CursorOverGLControl1())
			{
				glControl1_KeyUp(sender, e);
			}
			else if (CursorOverGLControl2())
			{
				glControl2_KeyUp(sender, e);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			DestroyTexture(ref texName);

			if (VBOid != null)
			{
				// this method silently ignores zeroes and non-existing buffer names
				GL.DeleteBuffers(8, VBOid);
				VBOid = null;
			}

			DestroyShaders();
		}

		private void buttonReset_Click(object sender, EventArgs e)
		{
			trackball1.Reset();
			trackball2.Reset();
		}

		private void loadModel1MenuItem_Click(object sender, EventArgs e)
		{
			string fileName = ChoosePathToOpen();
			scene1 = LoadModel(fileName);

			if (scene1 == null)
			{
				return;
			}

			DrawScene(scene1, 0, trackball1, glControl1);

			InitializeVisualizers();
			scene1Color = null;
			scene1Arrows = null;

			// if both scenes are loaded, exit visualization view mode
			if (scene1 != null && scene1.Vertices > 0 &&
				scene2 != null && scene2.Vertices > 0)
			{
				updateVisButton.Enabled = true;
			}
		}

		private void loadModel2MenuItem_Click(object sender, EventArgs e)
		{
			string fileName = ChoosePathToOpen();
			scene2 = LoadModel(fileName);

			if (scene2 == null)
			{
				return;
			}

			DrawScene(scene2, 2, trackball2, glControl2);

			InitializeVisualizers();
			scene2Color = null;
			scene2Arrows = null;

			// if both scenes are loaded, exit visualization view mode
			if (scene1 != null && scene1.Vertices > 0 &&
				scene2 != null && scene2.Vertices > 0)
			{
				updateVisButton.Enabled = true;
			}
		}

		private void exportVisualization1MenuItem_Click(object sender, EventArgs e)
		{
			bool separately;

			if (scene1Arrows != null && scene1Arrows.Vertices > 0 &&
				MessageBox.Show("Do you want to export arrows separately?", "Visualization Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
				== DialogResult.Yes)
			{
				separately = true;
			}
			else
			{
				separately = false;
			}

			if (ExportVisualization(scene1Color, scene1Arrows, separately))
			{
				MessageBox.Show("Visualization 1 saved!", "Visualization Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("Visualization export either failed or was cancelled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void exportVisualization2MenuItem_Click(object sender, EventArgs e)
		{
			bool separately;

			if (scene2Arrows != null && scene2Arrows.Vertices > 0 &&
				MessageBox.Show("Do you want to export arrows separately?", "Visualization Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
			{
				separately = true;
			}
			else
			{
				separately = false;
			}

			if (ExportVisualization(scene2Color, scene2Arrows, separately))
			{
				MessageBox.Show("Visualization 2 saved!", "Visualization Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("Visualization export either failed or was cancelled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void loadVisualization1MenuItem_Click(object sender, EventArgs e)
		{
			bool separately;
			if (MessageBox.Show("Do you want to load arrows separately?", "Visualization Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
			{
				separately = true;
			}
			else
			{
				separately = false;
			}

			string fileName = ChoosePathToOpen("Open Visualization File");
			scene1Color = LoadModel(fileName);
			DrawScene(scene1Color, 0, trackball1, glControl1);

			if (separately == true)
			{
				string arrowsFileName = ChoosePathToOpen("Open Arrows File");
				scene1Arrows = LoadModel(arrowsFileName);

				// draw arrows
				currentVisualization1 = scene1Arrows;
				PrepareDataBuffers(glControl1, scene1Arrows, 4);
			}
			else
			{
				scene1Arrows = null;
			}

			scene1 = new SceneBrep();

			// enter visualization view mode
			updateVisButton.Enabled = false;
			hideVisButton.Enabled = false;
			showVisButton.Enabled = false;
		}

		private void loadVisualization2MenuItem_Click(object sender, EventArgs e)
		{
			bool separately;
			if (MessageBox.Show("Do you want to load arrows separately?", "Visualization Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
			{
				separately = true;
			}
			else
			{
				separately = false;
			}

			string fileName = ChoosePathToOpen("Open Visualization File");
			scene2Color = LoadModel(fileName);
			DrawScene(scene2Color, 2, trackball2, glControl2);

			if (separately == true)
			{
				string arrowsFileName = ChoosePathToOpen("Open Arrows File");
				scene2Arrows = LoadModel(arrowsFileName);

				// draw arrows
				currentVisualization2 = scene2Arrows;
				PrepareDataBuffers(glControl2, scene2Arrows, 6);
			}
			else
			{
				scene2Arrows = null;
			}

			scene2 = new SceneBrep();

			// enter visualization view mode
			updateVisButton.Enabled = false;
			hideVisButton.Enabled = false;
			showVisButton.Enabled = false;
		}

		private void saveParametersMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Save Parameters";
			sfd.Filter = "Configuration settings (.ini)|*.ini";
			sfd.AddExtension = true;
			sfd.FileName = "";
			if (sfd.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			// replace a file if it already exists
			File.Delete(sfd.FileName);

			trackball1.SaveToFile(sfd.FileName);
			trackball2.SaveToFile(sfd.FileName);
			Metrics.SaveToFile(currentMetric, sfd.FileName);
			clusteringParametersArrows.SaveToFile(sfd.FileName);
			clusteringParametersColors.SaveToFile(sfd.FileName);
			visualizerParameters.SaveToFile(sfd.FileName);
			this.SaveToFile(sfd.FileName);
		}

		private void loadParametersMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Load Parameters";
			ofd.Filter = "Configuration settings (.ini)|*.ini";
			ofd.FileName = "";

			if (ofd.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			trackball1.SetFromFile(ofd.FileName);
			trackball2.SetFromFile(ofd.FileName);
			currentMetric = Metrics.LoadFromFile(ofd.FileName);
			clusteringParametersColors.LoadFromFile(ofd.FileName);
			clusteringParametersArrows.LoadFromFile(ofd.FileName);
			visualizerParameters = VisualizerParameters.LoadFromFile(ofd.FileName);
			this.SetFromFile(ofd.FileName);

			// update the cluster count trackbars
			clusterCountArrowsTrackBar.Value = clusteringParametersArrows.ClusterCount;
			clusterCountColorsTrackBar.Value = clusteringParametersColors.ClusterCount;
		}

		private void toolStrip1_MouseEnter(object sender, EventArgs e)
		{
			if (savedCursor == null)
			{
				savedCursor = this.Cursor;
				this.Cursor = Cursors.Default;
			}
		}

		private void toolStrip1_MouseLeave(object sender, EventArgs e)
		{
			this.Cursor = savedCursor;
			savedCursor = null;
		}

		private void updateVisButton_Click(object sender, EventArgs e)
		{
			if (scene1 == null || scene2 == null)
			{
				return;
			}

			scene1Color = scene1.Clone();
			scene2Color = scene2.Clone();
			scene1Arrows = new SceneBrep();
			scene2Arrows = new SceneBrep();

			IDiffAlgo diffForColor = null;
			IDiffAlgo diffForArrows = null;
			IVisualizer visualizerColor = null;
			IVisualizer visualizerArrow = null;

			// if arrow visualization was chosen, find the requested diff for it
			if ((YesNo)arrowVisComboBox.SelectedItem == YesNo.Yes)
			{
				visualizerArrow = new VisualizerArrow();

				// clustering type for arrows
				switch ((ClusteringType)clusteringArrowsComboBox.SelectedItem)
				{
					case ClusteringType.None:
						diffForArrows = diffNoClustering;
						break;
					case ClusteringType.Simple:
						diffForArrows = diffSimpleClustering;
						break;
					case ClusteringType.Signed:
						diffForArrows = diffSignedClustering;
						break;
				}
			}

			// set the requested color visualizer
			switch ((ColorVisType)colorVisComboBox.SelectedItem)
			{
				case ColorVisType.Relative:
					visualizerColor = new VisualizerColorClusterRelative(scene1.Vertices);
					break;
				case ColorVisType.Absolute:
					visualizerColor = new VisualizerColorClusterAbsolute(scene1.Vertices);
					break;
				case ColorVisType.Random:
					visualizerColor = new VisualizerColorClusterRandom(scene1.Vertices);
					break;
				case ColorVisType.None:
					break;
			}

			// if color visualizer was chosen, find the requested diff for it
			if (visualizerColor != null)
			{
				// clustering type for colors
				switch ((ClusteringType)clusteringColorsComboBox.SelectedItem)
				{
					case ClusteringType.None:
						diffForColor = diffNoClustering;
						break;
					case ClusteringType.Simple:
						diffForColor = diffSimpleClustering;
						break;
					case ClusteringType.Signed:
						diffForColor = diffSignedClustering;
						break;
				}
			}

			ProgressDialog progressDialog = new ProgressDialog()
			{
				StartPosition = FormStartPosition.CenterScreen
			};
			
			JobParameters jobParameters = new JobParameters(
				diffForArrows,
				diffForColor,
				currentMetric,
				clusteringParametersColors,
				clusteringParametersArrows,
				visualizerParameters,
				visualizerColor,
				visualizerArrow,
				scene1Color,
				scene2Color,
				scene1Arrows,
				scene2Arrows
			);

			Job job = new Job(jobParameters, progressDialog);
			Thread thread = new Thread(job.Run);

			// since the control is being accessed from the thread right at the beginning of the thread's execution,
			// we need to make sure its handle has already been created at that point
			progressDialog.HandleCreated += (object sender2, EventArgs e2) => thread.Start();

			// show dialog AND run job
			progressDialog.ShowDialog();

			if (!job.HasSucceeded)
			{
				scene1Color.Reset();
				scene2Color.Reset();
				scene1Arrows.Reset();
				scene2Arrows.Reset();

				hideVisButton.Enabled = false;
				showVisButton.Enabled = false;
				return;
			}

			if (scene1Arrows.Vertices == 0)
			{
				scene1Arrows = null;
				scene2Arrows = null;
			}

			currentScene1 = scene1Color;
			currentScene2 = scene2Color;
			currentVisualization1 = scene1Arrows;
			currentVisualization2 = scene2Arrows;

			PrepareDataBuffers(glControl1, scene1Color, 0);
			PrepareDataBuffers(glControl1, scene1Arrows, 4);

			PrepareDataBuffers(glControl2, scene2Color, 2);
			PrepareDataBuffers(glControl2, scene2Arrows, 6);

			hideVisButton.Enabled = true;
			showVisButton.Enabled = false;
		}

		private void hideVisButton_Click(object sender, EventArgs e)
		{
			hideVisButton.Enabled = false;
			showVisButton.Enabled = true;

			currentScene1 = scene1;
			currentScene2 = scene2;
			currentVisualization1 = null;
			currentVisualization2 = null;

			PrepareDataBuffers(glControl1, scene1, 0);
			PrepareDataBuffers(glControl1, null, 4);

			PrepareDataBuffers(glControl2, scene2, 2);
			PrepareDataBuffers(glControl2, null, 6);
		}

		private void showVisButton_Click(object sender, EventArgs e)
		{
			showVisButton.Enabled = false;
			hideVisButton.Enabled = true;

			currentScene1 = scene1Color;
			currentScene2 = scene2Color;
			currentVisualization1 = scene1Arrows;
			currentVisualization2 = scene2Arrows;

			PrepareDataBuffers(glControl1, scene1Color, 0);
			PrepareDataBuffers(glControl1, scene1Arrows, 4);

			PrepareDataBuffers(glControl2, scene2Color, 2);
			PrepareDataBuffers(glControl2, scene2Arrows, 6);
		}

		private void clusteringParametersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClusteringParameterEditForm parameterEditForm = new ClusteringParameterEditForm(clusteringParametersArrows, clusteringParametersColors)
			{
				StartPosition = FormStartPosition.CenterScreen
			};
			parameterEditForm.ShowDialog();
		}

		private void visualizerParametersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			VisualizerParameterEditForm parameterEditForm = new VisualizerParameterEditForm(visualizerParameters, scene1)
			{
				StartPosition = FormStartPosition.CenterScreen
			};
			parameterEditForm.ShowDialog();
		}

		private void metricsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MetricsEditForm metricsEditForm = new MetricsEditForm(currentMetric);
			metricsEditForm.StartPosition = FormStartPosition.CenterScreen;
			var dialogResult = metricsEditForm.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				currentMetric = metricsEditForm.ChosenMetric;
			}
		}

		private void clusterCountArrowsTrackBar_ValueChanged(object sender, EventArgs e)
		{
			clusterCountArrowsValueLabel.Text = clusterCountArrowsTrackBar.Value.ToString();
			clusteringParametersArrows.ClusterCount = clusterCountArrowsTrackBar.Value;
		}

		private void clusterCountColorsTrackBar_ValueChanged(object sender, EventArgs e)
		{
			clusterCountColorValueLabel.Text = clusterCountColorsTrackBar.Value.ToString();
			clusteringParametersColors.ClusterCount = clusterCountColorsTrackBar.Value;
		}

		private void clusteringArrowsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((ClusteringType)clusteringArrowsComboBox.SelectedItem == ClusteringType.None)
			{
				clusterCountArrowsTrackBar.Enabled = false;
			}
			else
			{
				clusterCountArrowsTrackBar.Enabled = true;
			}
		}

		private void clusteringColorsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((ClusteringType)clusteringColorsComboBox.SelectedItem == ClusteringType.None)
			{
				clusterCountColorsTrackBar.Enabled = false;
			}
			else
			{
				clusterCountColorsTrackBar.Enabled = true;
			}
		}

		#endregion
	}

}

namespace meshDiff
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.glControl1 = new OpenTK.GLControl();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.saveButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.loadModel1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadModel2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportVisualization1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportVisualization2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadVisualization1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadVisualization2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveParametersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadParametersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.button_View = new System.Windows.Forms.ToolStripDropDownButton();
			this.resetViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pairedControlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkAxes = new System.Windows.Forms.ToolStripMenuItem();
			this.checkSmooth = new System.Windows.Forms.ToolStripMenuItem();
			this.checkWireframe = new System.Windows.Forms.ToolStripMenuItem();
			this.checkVisWireframe = new System.Windows.Forms.ToolStripMenuItem();
			this.checkTwosided = new System.Windows.Forms.ToolStripMenuItem();
			this.checkShaders = new System.Windows.Forms.ToolStripMenuItem();
			this.checkAmbient = new System.Windows.Forms.ToolStripMenuItem();
			this.checkDiffuse = new System.Windows.Forms.ToolStripMenuItem();
			this.checkSpecular = new System.Windows.Forms.ToolStripMenuItem();
			this.checkPhong = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.metricsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clusteringParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.visualizerParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip3 = new System.Windows.Forms.ToolStrip();
			this.labelFps = new System.Windows.Forms.ToolStripLabel();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.glControl2 = new OpenTK.GLControl();
			this.updateVisButton = new System.Windows.Forms.Button();
			this.hideVisButton = new System.Windows.Forms.Button();
			this.showVisButton = new System.Windows.Forms.Button();
			this.clusterCountArrowsTrackBar = new System.Windows.Forms.TrackBar();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.file1Label = new System.Windows.Forms.ToolStripLabel();
			this.file1ValueLabel = new System.Windows.Forms.ToolStripLabel();
			this.file2ValueLabel = new System.Windows.Forms.ToolStripLabel();
			this.file2Label = new System.Windows.Forms.ToolStripLabel();
			this.clusteringArrowsComboBox = new System.Windows.Forms.ComboBox();
			this.clusteringArrowsLabel = new System.Windows.Forms.Label();
			this.colorVisLabel = new System.Windows.Forms.Label();
			this.colorVisComboBox = new System.Windows.Forms.ComboBox();
			this.clusterCountArrowsLabel = new System.Windows.Forms.Label();
			this.clusterCountArrowsValueLabel = new System.Windows.Forms.Label();
			this.arrowVisLabel = new System.Windows.Forms.Label();
			this.arrowVisComboBox = new System.Windows.Forms.ComboBox();
			this.clusteringColorsLabel = new System.Windows.Forms.Label();
			this.clusteringColorsComboBox = new System.Windows.Forms.ComboBox();
			this.clusterCountColorValueLabel = new System.Windows.Forms.Label();
			this.clusterCountColorLabel = new System.Windows.Forms.Label();
			this.clusterCountColorsTrackBar = new System.Windows.Forms.TrackBar();
			this.toolStrip1.SuspendLayout();
			this.toolStrip3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.clusterCountArrowsTrackBar)).BeginInit();
			this.toolStrip2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.clusterCountColorsTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// glControl1
			// 
			this.glControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.glControl1.BackColor = System.Drawing.Color.Black;
			this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl1.Location = new System.Drawing.Point(0, 0);
			this.glControl1.Margin = new System.Windows.Forms.Padding(6);
			this.glControl1.Name = "glControl1";
			this.glControl1.Size = new System.Drawing.Size(421, 391);
			this.glControl1.TabIndex = 17;
			this.glControl1.VSync = false;
			this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
			this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
			this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
			this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
			this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
			this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.Color.White;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveButton,
            this.button_View,
            this.settingsToolStripDropDownButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip1.Size = new System.Drawing.Size(902, 25);
			this.toolStrip1.TabIndex = 56;
			this.toolStrip1.Text = "toolStrip1";
			this.toolStrip1.MouseEnter += new System.EventHandler(this.toolStrip1_MouseEnter);
			this.toolStrip1.MouseLeave += new System.EventHandler(this.toolStrip1_MouseLeave);
			// 
			// saveButton
			// 
			this.saveButton.AutoToolTip = false;
			this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.saveButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadModel1MenuItem,
            this.loadModel2MenuItem,
            this.exportVisualization1MenuItem,
            this.exportVisualization2MenuItem,
            this.loadVisualization1MenuItem,
            this.loadVisualization2MenuItem,
            this.saveParametersMenuItem,
            this.loadParametersMenuItem});
			this.saveButton.ForeColor = System.Drawing.Color.Black;
			this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveButton.Name = "saveButton";
			this.saveButton.ShowDropDownArrow = false;
			this.saveButton.Size = new System.Drawing.Size(29, 22);
			this.saveButton.Text = "File";
			this.saveButton.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			this.saveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
			// 
			// loadModel1MenuItem
			// 
			this.loadModel1MenuItem.Name = "loadModel1MenuItem";
			this.loadModel1MenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadModel1MenuItem.Text = "Load Model 1";
			this.loadModel1MenuItem.Click += new System.EventHandler(this.loadModel1MenuItem_Click);
			// 
			// loadModel2MenuItem
			// 
			this.loadModel2MenuItem.Name = "loadModel2MenuItem";
			this.loadModel2MenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadModel2MenuItem.Text = "Load Model 2";
			this.loadModel2MenuItem.Click += new System.EventHandler(this.loadModel2MenuItem_Click);
			// 
			// exportVisualization1MenuItem
			// 
			this.exportVisualization1MenuItem.Image = global::meshDiff.Properties.Resources.save;
			this.exportVisualization1MenuItem.Name = "exportVisualization1MenuItem";
			this.exportVisualization1MenuItem.Size = new System.Drawing.Size(185, 22);
			this.exportVisualization1MenuItem.Text = "Export Visualization 1";
			this.exportVisualization1MenuItem.Click += new System.EventHandler(this.exportVisualization1MenuItem_Click);
			// 
			// exportVisualization2MenuItem
			// 
			this.exportVisualization2MenuItem.Image = global::meshDiff.Properties.Resources.save;
			this.exportVisualization2MenuItem.Name = "exportVisualization2MenuItem";
			this.exportVisualization2MenuItem.Size = new System.Drawing.Size(185, 22);
			this.exportVisualization2MenuItem.Text = "Export Visualization 2";
			this.exportVisualization2MenuItem.Click += new System.EventHandler(this.exportVisualization2MenuItem_Click);
			// 
			// loadVisualization1MenuItem
			// 
			this.loadVisualization1MenuItem.Name = "loadVisualization1MenuItem";
			this.loadVisualization1MenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadVisualization1MenuItem.Text = "Load Visualization 1";
			this.loadVisualization1MenuItem.Click += new System.EventHandler(this.loadVisualization1MenuItem_Click);
			// 
			// loadVisualization2MenuItem
			// 
			this.loadVisualization2MenuItem.Name = "loadVisualization2MenuItem";
			this.loadVisualization2MenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadVisualization2MenuItem.Text = "Load Visualization 2";
			this.loadVisualization2MenuItem.Click += new System.EventHandler(this.loadVisualization2MenuItem_Click);
			// 
			// saveParametersMenuItem
			// 
			this.saveParametersMenuItem.Image = global::meshDiff.Properties.Resources.save;
			this.saveParametersMenuItem.Name = "saveParametersMenuItem";
			this.saveParametersMenuItem.Size = new System.Drawing.Size(185, 22);
			this.saveParametersMenuItem.Text = "Save Parameters";
			this.saveParametersMenuItem.Click += new System.EventHandler(this.saveParametersMenuItem_Click);
			// 
			// loadParametersMenuItem
			// 
			this.loadParametersMenuItem.Name = "loadParametersMenuItem";
			this.loadParametersMenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadParametersMenuItem.Text = "Load Parameters";
			this.loadParametersMenuItem.Click += new System.EventHandler(this.loadParametersMenuItem_Click);
			// 
			// button_View
			// 
			this.button_View.AutoToolTip = false;
			this.button_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.button_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetViewMenuItem,
            this.pairedControlToolStripMenuItem,
            this.checkAxes,
            this.checkSmooth,
            this.checkWireframe,
            this.checkVisWireframe,
            this.checkTwosided,
            this.checkShaders,
            this.checkAmbient,
            this.checkDiffuse,
            this.checkSpecular,
            this.checkPhong});
			this.button_View.ForeColor = System.Drawing.Color.Black;
			this.button_View.Image = ((System.Drawing.Image)(resources.GetObject("button_View.Image")));
			this.button_View.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.button_View.Name = "button_View";
			this.button_View.ShowDropDownArrow = false;
			this.button_View.Size = new System.Drawing.Size(36, 22);
			this.button_View.Text = "View";
			// 
			// resetViewMenuItem
			// 
			this.resetViewMenuItem.Image = global::meshDiff.Properties.Resources.cam2;
			this.resetViewMenuItem.Name = "resetViewMenuItem";
			this.resetViewMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.resetViewMenuItem.Size = new System.Drawing.Size(167, 22);
			this.resetViewMenuItem.Text = "Reset View";
			this.resetViewMenuItem.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// pairedControlToolStripMenuItem
			// 
			this.pairedControlToolStripMenuItem.Checked = true;
			this.pairedControlToolStripMenuItem.CheckOnClick = true;
			this.pairedControlToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.pairedControlToolStripMenuItem.Name = "pairedControlToolStripMenuItem";
			this.pairedControlToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
			this.pairedControlToolStripMenuItem.Text = "Paired Control";
			// 
			// checkAxes
			// 
			this.checkAxes.CheckOnClick = true;
			this.checkAxes.Name = "checkAxes";
			this.checkAxes.Size = new System.Drawing.Size(167, 22);
			this.checkAxes.Text = "Axes";
			// 
			// checkSmooth
			// 
			this.checkSmooth.Checked = true;
			this.checkSmooth.CheckOnClick = true;
			this.checkSmooth.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkSmooth.Name = "checkSmooth";
			this.checkSmooth.Size = new System.Drawing.Size(167, 22);
			this.checkSmooth.Text = "Smooth";
			// 
			// checkWireframe
			// 
			this.checkWireframe.Checked = true;
			this.checkWireframe.CheckOnClick = true;
			this.checkWireframe.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkWireframe.Name = "checkWireframe";
			this.checkWireframe.Size = new System.Drawing.Size(167, 22);
			this.checkWireframe.Text = "Wire";
			// 
			// checkVisWireframe
			// 
			this.checkVisWireframe.CheckOnClick = true;
			this.checkVisWireframe.Name = "checkVisWireframe";
			this.checkVisWireframe.Size = new System.Drawing.Size(167, 22);
			this.checkVisWireframe.Text = "Visualization Wire";
			// 
			// checkTwosided
			// 
			this.checkTwosided.Checked = true;
			this.checkTwosided.CheckOnClick = true;
			this.checkTwosided.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkTwosided.Name = "checkTwosided";
			this.checkTwosided.Size = new System.Drawing.Size(167, 22);
			this.checkTwosided.Text = "2-Sided";
			// 
			// checkShaders
			// 
			this.checkShaders.CheckOnClick = true;
			this.checkShaders.Name = "checkShaders";
			this.checkShaders.Size = new System.Drawing.Size(167, 22);
			this.checkShaders.Text = "GLSL";
			// 
			// checkAmbient
			// 
			this.checkAmbient.Checked = true;
			this.checkAmbient.CheckOnClick = true;
			this.checkAmbient.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAmbient.Name = "checkAmbient";
			this.checkAmbient.Size = new System.Drawing.Size(167, 22);
			this.checkAmbient.Text = "Ambient";
			// 
			// checkDiffuse
			// 
			this.checkDiffuse.Checked = true;
			this.checkDiffuse.CheckOnClick = true;
			this.checkDiffuse.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkDiffuse.Name = "checkDiffuse";
			this.checkDiffuse.Size = new System.Drawing.Size(167, 22);
			this.checkDiffuse.Text = "Diffuse";
			// 
			// checkSpecular
			// 
			this.checkSpecular.Checked = true;
			this.checkSpecular.CheckOnClick = true;
			this.checkSpecular.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkSpecular.Name = "checkSpecular";
			this.checkSpecular.Size = new System.Drawing.Size(167, 22);
			this.checkSpecular.Text = "Specular";
			// 
			// checkPhong
			// 
			this.checkPhong.CheckOnClick = true;
			this.checkPhong.Name = "checkPhong";
			this.checkPhong.Size = new System.Drawing.Size(167, 22);
			this.checkPhong.Text = "Phong";
			// 
			// settingsToolStripDropDownButton
			// 
			this.settingsToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.settingsToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.metricsToolStripMenuItem,
            this.clusteringParametersToolStripMenuItem,
            this.visualizerParametersToolStripMenuItem});
			this.settingsToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripDropDownButton.Image")));
			this.settingsToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.settingsToolStripDropDownButton.Name = "settingsToolStripDropDownButton";
			this.settingsToolStripDropDownButton.ShowDropDownArrow = false;
			this.settingsToolStripDropDownButton.Size = new System.Drawing.Size(53, 22);
			this.settingsToolStripDropDownButton.Text = "Settings";
			// 
			// metricsToolStripMenuItem
			// 
			this.metricsToolStripMenuItem.Name = "metricsToolStripMenuItem";
			this.metricsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.metricsToolStripMenuItem.Text = "Metrics";
			this.metricsToolStripMenuItem.Click += new System.EventHandler(this.metricsToolStripMenuItem_Click);
			// 
			// clusteringParametersToolStripMenuItem
			// 
			this.clusteringParametersToolStripMenuItem.Name = "clusteringParametersToolStripMenuItem";
			this.clusteringParametersToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.clusteringParametersToolStripMenuItem.Text = "Clustering Parameters";
			this.clusteringParametersToolStripMenuItem.Click += new System.EventHandler(this.clusteringParametersToolStripMenuItem_Click);
			// 
			// visualizerParametersToolStripMenuItem
			// 
			this.visualizerParametersToolStripMenuItem.Name = "visualizerParametersToolStripMenuItem";
			this.visualizerParametersToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.visualizerParametersToolStripMenuItem.Text = "Visualizer Parameters";
			this.visualizerParametersToolStripMenuItem.Click += new System.EventHandler(this.visualizerParametersToolStripMenuItem_Click);
			// 
			// toolStrip3
			// 
			this.toolStrip3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelFps});
			this.toolStrip3.Location = new System.Drawing.Point(0, 604);
			this.toolStrip3.Name = "toolStrip3";
			this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip3.Size = new System.Drawing.Size(902, 25);
			this.toolStrip3.TabIndex = 58;
			this.toolStrip3.Text = "toolStrip3";
			this.toolStrip3.MouseEnter += new System.EventHandler(this.toolStrip1_MouseEnter);
			this.toolStrip3.MouseLeave += new System.EventHandler(this.toolStrip1_MouseLeave);
			// 
			// labelFps
			// 
			this.labelFps.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.labelFps.Name = "labelFps";
			this.labelFps.Size = new System.Drawing.Size(28, 22);
			this.labelFps.Text = "Fps:";
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.Location = new System.Drawing.Point(18, 45);
			this.splitContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.glControl1);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.glControl2);
			this.splitContainer.Size = new System.Drawing.Size(851, 391);
			this.splitContainer.SplitterDistance = 421;
			this.splitContainer.SplitterWidth = 6;
			this.splitContainer.TabIndex = 59;
			// 
			// glControl2
			// 
			this.glControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.glControl2.BackColor = System.Drawing.Color.Black;
			this.glControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl2.Location = new System.Drawing.Point(0, 0);
			this.glControl2.Margin = new System.Windows.Forms.Padding(6);
			this.glControl2.Name = "glControl2";
			this.glControl2.Size = new System.Drawing.Size(424, 391);
			this.glControl2.TabIndex = 18;
			this.glControl2.VSync = false;
			this.glControl2.Load += new System.EventHandler(this.glControl2_Load);
			this.glControl2.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl2_Paint);
			this.glControl2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl2_MouseDown);
			this.glControl2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl2_MouseMove);
			this.glControl2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl2_MouseUp);
			this.glControl2.Resize += new System.EventHandler(this.glControl2_Resize);
			// 
			// updateVisButton
			// 
			this.updateVisButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.updateVisButton.Location = new System.Drawing.Point(619, 452);
			this.updateVisButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.updateVisButton.Name = "updateVisButton";
			this.updateVisButton.Size = new System.Drawing.Size(121, 23);
			this.updateVisButton.TabIndex = 63;
			this.updateVisButton.Text = "Update Visualization";
			this.updateVisButton.UseVisualStyleBackColor = true;
			this.updateVisButton.Click += new System.EventHandler(this.updateVisButton_Click);
			// 
			// hideVisButton
			// 
			this.hideVisButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.hideVisButton.Enabled = false;
			this.hideVisButton.Location = new System.Drawing.Point(748, 452);
			this.hideVisButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.hideVisButton.Name = "hideVisButton";
			this.hideVisButton.Size = new System.Drawing.Size(121, 23);
			this.hideVisButton.TabIndex = 64;
			this.hideVisButton.Text = "Hide Visualization";
			this.hideVisButton.UseVisualStyleBackColor = true;
			this.hideVisButton.Click += new System.EventHandler(this.hideVisButton_Click);
			// 
			// showVisButton
			// 
			this.showVisButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.showVisButton.Enabled = false;
			this.showVisButton.Location = new System.Drawing.Point(748, 485);
			this.showVisButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.showVisButton.Name = "showVisButton";
			this.showVisButton.Size = new System.Drawing.Size(121, 23);
			this.showVisButton.TabIndex = 65;
			this.showVisButton.Text = "Show Visualization";
			this.showVisButton.UseVisualStyleBackColor = true;
			this.showVisButton.Click += new System.EventHandler(this.showVisButton_Click);
			// 
			// clusterCountArrowsTrackBar
			// 
			this.clusterCountArrowsTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.clusterCountArrowsTrackBar.Enabled = false;
			this.clusterCountArrowsTrackBar.LargeChange = 10;
			this.clusterCountArrowsTrackBar.Location = new System.Drawing.Point(309, 470);
			this.clusterCountArrowsTrackBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.clusterCountArrowsTrackBar.Maximum = 1000;
			this.clusterCountArrowsTrackBar.Minimum = 1;
			this.clusterCountArrowsTrackBar.Name = "clusterCountArrowsTrackBar";
			this.clusterCountArrowsTrackBar.Size = new System.Drawing.Size(245, 45);
			this.clusterCountArrowsTrackBar.TabIndex = 69;
			this.clusterCountArrowsTrackBar.Value = 1;
			this.clusterCountArrowsTrackBar.ValueChanged += new System.EventHandler(this.clusterCountArrowsTrackBar_ValueChanged);
			// 
			// toolStrip2
			// 
			this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file1Label,
            this.file1ValueLabel,
            this.file2ValueLabel,
            this.file2Label});
			this.toolStrip2.Location = new System.Drawing.Point(0, 579);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(902, 25);
			this.toolStrip2.TabIndex = 75;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// file1Label
			// 
			this.file1Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.file1Label.Name = "file1Label";
			this.file1Label.Size = new System.Drawing.Size(54, 22);
			this.file1Label.Text = "Scene 1:";
			// 
			// file1ValueLabel
			// 
			this.file1ValueLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.file1ValueLabel.ForeColor = System.Drawing.Color.Black;
			this.file1ValueLabel.Name = "file1ValueLabel";
			this.file1ValueLabel.Size = new System.Drawing.Size(81, 22);
			this.file1ValueLabel.Text = "No file loaded";
			// 
			// file2ValueLabel
			// 
			this.file2ValueLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.file2ValueLabel.Name = "file2ValueLabel";
			this.file2ValueLabel.Size = new System.Drawing.Size(81, 22);
			this.file2ValueLabel.Text = "No file loaded";
			// 
			// file2Label
			// 
			this.file2Label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.file2Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.file2Label.Name = "file2Label";
			this.file2Label.Size = new System.Drawing.Size(54, 22);
			this.file2Label.Text = "Scene 2:";
			// 
			// clusteringArrowsComboBox
			// 
			this.clusteringArrowsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusteringArrowsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.clusteringArrowsComboBox.FormattingEnabled = true;
			this.clusteringArrowsComboBox.Location = new System.Drawing.Point(153, 468);
			this.clusteringArrowsComboBox.Name = "clusteringArrowsComboBox";
			this.clusteringArrowsComboBox.Size = new System.Drawing.Size(98, 21);
			this.clusteringArrowsComboBox.TabIndex = 76;
			this.clusteringArrowsComboBox.SelectedIndexChanged += new System.EventHandler(this.clusteringArrowsComboBox_SelectedIndexChanged);
			// 
			// clusteringArrowsLabel
			// 
			this.clusteringArrowsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusteringArrowsLabel.AutoSize = true;
			this.clusteringArrowsLabel.Location = new System.Drawing.Point(150, 452);
			this.clusteringArrowsLabel.Name = "clusteringArrowsLabel";
			this.clusteringArrowsLabel.Size = new System.Drawing.Size(106, 13);
			this.clusteringArrowsLabel.TabIndex = 77;
			this.clusteringArrowsLabel.Text = "Clustering for Arrows:";
			// 
			// colorVisLabel
			// 
			this.colorVisLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.colorVisLabel.AutoSize = true;
			this.colorVisLabel.Location = new System.Drawing.Point(15, 512);
			this.colorVisLabel.Name = "colorVisLabel";
			this.colorVisLabel.Size = new System.Drawing.Size(95, 13);
			this.colorVisLabel.TabIndex = 79;
			this.colorVisLabel.Text = "Color Visualization:";
			// 
			// colorVisComboBox
			// 
			this.colorVisComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.colorVisComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.colorVisComboBox.FormattingEnabled = true;
			this.colorVisComboBox.Location = new System.Drawing.Point(18, 528);
			this.colorVisComboBox.Name = "colorVisComboBox";
			this.colorVisComboBox.Size = new System.Drawing.Size(98, 21);
			this.colorVisComboBox.TabIndex = 78;
			// 
			// clusterCountArrowsLabel
			// 
			this.clusterCountArrowsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusterCountArrowsLabel.AutoSize = true;
			this.clusterCountArrowsLabel.Location = new System.Drawing.Point(306, 452);
			this.clusterCountArrowsLabel.Name = "clusterCountArrowsLabel";
			this.clusterCountArrowsLabel.Size = new System.Drawing.Size(123, 13);
			this.clusterCountArrowsLabel.TabIndex = 82;
			this.clusterCountArrowsLabel.Text = "Cluster Count for Arrows:";
			// 
			// clusterCountArrowsValueLabel
			// 
			this.clusterCountArrowsValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusterCountArrowsValueLabel.AutoSize = true;
			this.clusterCountArrowsValueLabel.Location = new System.Drawing.Point(435, 452);
			this.clusterCountArrowsValueLabel.Name = "clusterCountArrowsValueLabel";
			this.clusterCountArrowsValueLabel.Size = new System.Drawing.Size(151, 13);
			this.clusterCountArrowsValueLabel.TabIndex = 83;
			this.clusterCountArrowsValueLabel.Text = "clusterCountArrowsValueLabel";
			// 
			// arrowVisLabel
			// 
			this.arrowVisLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.arrowVisLabel.AutoSize = true;
			this.arrowVisLabel.Location = new System.Drawing.Point(15, 452);
			this.arrowVisLabel.Name = "arrowVisLabel";
			this.arrowVisLabel.Size = new System.Drawing.Size(98, 13);
			this.arrowVisLabel.TabIndex = 84;
			this.arrowVisLabel.Text = "Arrow Visualization:";
			// 
			// arrowVisComboBox
			// 
			this.arrowVisComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.arrowVisComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.arrowVisComboBox.FormattingEnabled = true;
			this.arrowVisComboBox.Location = new System.Drawing.Point(18, 468);
			this.arrowVisComboBox.Name = "arrowVisComboBox";
			this.arrowVisComboBox.Size = new System.Drawing.Size(98, 21);
			this.arrowVisComboBox.TabIndex = 85;
			// 
			// clusteringColorsLabel
			// 
			this.clusteringColorsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusteringColorsLabel.AutoSize = true;
			this.clusteringColorsLabel.Location = new System.Drawing.Point(150, 512);
			this.clusteringColorsLabel.Name = "clusteringColorsLabel";
			this.clusteringColorsLabel.Size = new System.Drawing.Size(103, 13);
			this.clusteringColorsLabel.TabIndex = 87;
			this.clusteringColorsLabel.Text = "Clustering for Colors:";
			// 
			// clusteringColorsComboBox
			// 
			this.clusteringColorsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusteringColorsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.clusteringColorsComboBox.FormattingEnabled = true;
			this.clusteringColorsComboBox.Location = new System.Drawing.Point(153, 528);
			this.clusteringColorsComboBox.Name = "clusteringColorsComboBox";
			this.clusteringColorsComboBox.Size = new System.Drawing.Size(98, 21);
			this.clusteringColorsComboBox.TabIndex = 86;
			this.clusteringColorsComboBox.SelectedIndexChanged += new System.EventHandler(this.clusteringColorsComboBox_SelectedIndexChanged);
			// 
			// clusterCountColorValueLabel
			// 
			this.clusterCountColorValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusterCountColorValueLabel.AutoSize = true;
			this.clusterCountColorValueLabel.Location = new System.Drawing.Point(435, 512);
			this.clusterCountColorValueLabel.Name = "clusterCountColorValueLabel";
			this.clusterCountColorValueLabel.Size = new System.Drawing.Size(143, 13);
			this.clusterCountColorValueLabel.TabIndex = 90;
			this.clusterCountColorValueLabel.Text = "clusterCountColorValueLabel";
			// 
			// clusterCountColorLabel
			// 
			this.clusterCountColorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clusterCountColorLabel.AutoSize = true;
			this.clusterCountColorLabel.Location = new System.Drawing.Point(306, 512);
			this.clusterCountColorLabel.Name = "clusterCountColorLabel";
			this.clusterCountColorLabel.Size = new System.Drawing.Size(120, 13);
			this.clusterCountColorLabel.TabIndex = 89;
			this.clusterCountColorLabel.Text = "Cluster Count for Colors:";
			// 
			// clusterCountColorsTrackBar
			// 
			this.clusterCountColorsTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.clusterCountColorsTrackBar.Enabled = false;
			this.clusterCountColorsTrackBar.LargeChange = 10;
			this.clusterCountColorsTrackBar.Location = new System.Drawing.Point(309, 530);
			this.clusterCountColorsTrackBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.clusterCountColorsTrackBar.Maximum = 1000;
			this.clusterCountColorsTrackBar.Minimum = 1;
			this.clusterCountColorsTrackBar.Name = "clusterCountColorsTrackBar";
			this.clusterCountColorsTrackBar.Size = new System.Drawing.Size(245, 45);
			this.clusterCountColorsTrackBar.TabIndex = 88;
			this.clusterCountColorsTrackBar.Value = 1;
			this.clusterCountColorsTrackBar.ValueChanged += new System.EventHandler(this.clusterCountColorsTrackBar_ValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(902, 629);
			this.Controls.Add(this.clusterCountColorValueLabel);
			this.Controls.Add(this.clusterCountColorLabel);
			this.Controls.Add(this.clusterCountColorsTrackBar);
			this.Controls.Add(this.clusteringColorsLabel);
			this.Controls.Add(this.clusteringColorsComboBox);
			this.Controls.Add(this.arrowVisComboBox);
			this.Controls.Add(this.arrowVisLabel);
			this.Controls.Add(this.clusterCountArrowsValueLabel);
			this.Controls.Add(this.clusterCountArrowsLabel);
			this.Controls.Add(this.colorVisLabel);
			this.Controls.Add(this.colorVisComboBox);
			this.Controls.Add(this.clusteringArrowsLabel);
			this.Controls.Add(this.clusteringArrowsComboBox);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.clusterCountArrowsTrackBar);
			this.Controls.Add(this.showVisButton);
			this.Controls.Add(this.hideVisButton);
			this.Controls.Add(this.updateVisButton);
			this.Controls.Add(this.toolStrip3);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.splitContainer);
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new System.Drawing.Size(918, 636);
			this.Name = "Form1";
			this.Text = "Mesh Diff";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseWheel);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip3.ResumeLayout(false);
			this.toolStrip3.PerformLayout();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.clusterCountArrowsTrackBar)).EndInit();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.clusterCountColorsTrackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenTK.GLControl glControl1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripDropDownButton saveButton;
		private System.Windows.Forms.ToolStripMenuItem exportVisualization1MenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadModel1MenuItem;
		private System.Windows.Forms.ToolStripDropDownButton button_View;
		private System.Windows.Forms.ToolStripMenuItem resetViewMenuItem;
		private System.Windows.Forms.ToolStripMenuItem checkSmooth;
		private System.Windows.Forms.ToolStripMenuItem checkWireframe;
		private System.Windows.Forms.ToolStripMenuItem checkTwosided;
		private System.Windows.Forms.ToolStripMenuItem checkAxes;
		private System.Windows.Forms.ToolStripMenuItem checkShaders;
		private System.Windows.Forms.ToolStripMenuItem checkAmbient;
		private System.Windows.Forms.ToolStripMenuItem checkDiffuse;
		private System.Windows.Forms.ToolStripMenuItem checkSpecular;
		private System.Windows.Forms.ToolStripMenuItem checkPhong;
		private System.Windows.Forms.ToolStrip toolStrip3;
		private System.Windows.Forms.ToolStripLabel labelFps;
		private System.Windows.Forms.SplitContainer splitContainer;
		private OpenTK.GLControl glControl2;
		private System.Windows.Forms.ToolStripMenuItem loadModel2MenuItem;
		private System.Windows.Forms.Button updateVisButton;
		private System.Windows.Forms.Button hideVisButton;
		private System.Windows.Forms.Button showVisButton;
		private System.Windows.Forms.TrackBar clusterCountArrowsTrackBar;
		private System.Windows.Forms.ToolStripDropDownButton settingsToolStripDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem clusteringParametersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pairedControlToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem metricsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem visualizerParametersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportVisualization2MenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadVisualization1MenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadVisualization2MenuItem;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripLabel file1Label;
		private System.Windows.Forms.ToolStripLabel file1ValueLabel;
		private System.Windows.Forms.ToolStripLabel file2ValueLabel;
		private System.Windows.Forms.ToolStripLabel file2Label;
		private System.Windows.Forms.ToolStripMenuItem saveParametersMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadParametersMenuItem;
		private System.Windows.Forms.ToolStripMenuItem checkVisWireframe;
		private System.Windows.Forms.ComboBox clusteringArrowsComboBox;
		private System.Windows.Forms.Label clusteringArrowsLabel;
		private System.Windows.Forms.Label colorVisLabel;
		private System.Windows.Forms.ComboBox colorVisComboBox;
		private System.Windows.Forms.Label clusterCountArrowsLabel;
		private System.Windows.Forms.Label clusterCountArrowsValueLabel;
		private System.Windows.Forms.Label arrowVisLabel;
		private System.Windows.Forms.ComboBox arrowVisComboBox;
		private System.Windows.Forms.Label clusteringColorsLabel;
		private System.Windows.Forms.ComboBox clusteringColorsComboBox;
		private System.Windows.Forms.Label clusterCountColorValueLabel;
		private System.Windows.Forms.Label clusterCountColorLabel;
		private System.Windows.Forms.TrackBar clusterCountColorsTrackBar;
	}
}


// author: Jan Horesovsky

using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using Scene3D;

namespace meshDiff
{
	public partial class VisualizerParameterEditForm : Form
	{
		VisualizerParameters parametersToEdit;

		public VisualizerParameterEditForm(VisualizerParameters parametersToEdit, SceneBrep scene)
		{
			InitializeComponent();

			this.parametersToEdit = parametersToEdit;

			float sceneDiameter = scene.GetDiameter();

			SetColorDiffTrackbar(sceneDiameter);
			SetDisabledLengthTrackbar(sceneDiameter);
			SetDisabledSizeTrackbar(sceneDiameter * sceneDiameter);

			minWidthScaleTrackBar.Value = GetTrackBarValue(parametersToEdit.ArrowWidthMinScale);
			maxWidthScaleTrackBar.Value = GetTrackBarValue(parametersToEdit.ArrowWidthMaxScale);
			minHeightScaleTrackBar.Value = GetTrackBarValue(parametersToEdit.ArrowHeightMinScale);
			maxHeightScaleTrackBar.Value = GetTrackBarValue(parametersToEdit.ArrowHeightMaxScale);
			arrowOutwardsColorDialog.Color = GetColor(parametersToEdit.ArrowOutwardsColor);
			arrowInwardsColorDialog.Color = GetColor(parametersToEdit.ArrowInwardsColor);
			colorMetricOutwardsDialog.Color = GetColor(parametersToEdit.ColorMetricOutwards);
			colorMetricInwardsDialog.Color = GetColor(parametersToEdit.ColorMetricInwards);
			disabledColorDialog.Color = GetColor(parametersToEdit.DisabledColor);

			UpdateTrackBarLabels();
			UpdateColorView(arrowOutwardsColorPictureBox, GetColor(parametersToEdit.ArrowOutwardsColor));
			UpdateColorView(arrowInwardsColorPictureBox, GetColor(parametersToEdit.ArrowInwardsColor));
			UpdateColorView(colorMetricOutwardsPictureBox, GetColor(parametersToEdit.ColorMetricOutwards));
			UpdateColorView(colorMetricInwardsPictureBox, GetColor(parametersToEdit.ColorMetricInwards));
			UpdateColorView(disabledColorPictureBox, GetColor(parametersToEdit.DisabledColor));
		}

		// sets the minimum, the maximum and the current value of the color diff trackbar
		private void SetColorDiffTrackbar(float sceneDiameter)
		{
			int minValue = 1;
			int maxValue = (sceneDiameter > minValue) ? (int)sceneDiameter : minValue;

			colorDiffThresholdTrackBar.Minimum = minValue;
			colorDiffThresholdTrackBar.Maximum = maxValue;
			colorDiffThresholdTrackBar.SmallChange = minValue;
			colorDiffThresholdTrackBar.LargeChange = (sceneDiameter > 10) ? 5 : minValue;

			int trackBarValue = GetTrackBarValue(parametersToEdit.ColorDiffThreshold);

			if (trackBarValue < minValue)
			{
				colorDiffThresholdTrackBar.Value = minValue;
			}
			else if (trackBarValue > maxValue)
			{
				colorDiffThresholdTrackBar.Value = maxValue;
			}
			else
			{
				colorDiffThresholdTrackBar.Value = trackBarValue;
			}
		}

		// sets the minimum, the maximum and the current value of the disabled length trackbar
		private void SetDisabledLengthTrackbar(float sceneDiameter)
		{
			int maxValue = (int)sceneDiameter;

			disabledThresholdLengthTrackBar.Minimum = 0;
			disabledThresholdLengthTrackBar.Maximum = maxValue;

			int trackBarValue = GetTrackBarValue(parametersToEdit.DisabledThresholdLength);

			if (trackBarValue > maxValue)
			{
				disabledThresholdLengthTrackBar.Value = maxValue;
			}
			else
			{
				disabledThresholdLengthTrackBar.Value = trackBarValue;
			}
		}

		// sets the minimum, the maximum and the current value of the disabled size trackbar
		private void SetDisabledSizeTrackbar(float sceneArea)
		{
			int maxValue = (int)sceneArea;

			disabledThresholdSizeTrackBar.Minimum = 0;
			disabledThresholdSizeTrackBar.Maximum = maxValue;

			int trackBarValue = GetTrackBarValue(parametersToEdit.DisabledThresholdSize);

			if (trackBarValue > maxValue)
			{
				disabledThresholdSizeTrackBar.Value = maxValue;
			}
			else
			{
				disabledThresholdSizeTrackBar.Value = trackBarValue;
			}
		}

		private void UpdateTrackBarLabels()
		{
			minWidthScaleValueLabel.Text = GetParamValueFromTrackBar(minWidthScaleTrackBar.Value).ToString();
			maxWidthScaleValueLabel.Text = GetParamValueFromTrackBar(maxWidthScaleTrackBar.Value).ToString();
			minHeightScaleValueLabel.Text = GetParamValueFromTrackBar(minHeightScaleTrackBar.Value).ToString();
			maxHeightScaleValueLabel.Text = GetParamValueFromTrackBar(maxHeightScaleTrackBar.Value).ToString();
			colorDiffThresholdValueLabel.Text = GetParamValueFromTrackBar(colorDiffThresholdTrackBar.Value).ToString();
			disabledThresholdLengthValueLabel.Text = GetParamValueFromTrackBar(disabledThresholdLengthTrackBar.Value).ToString();
			disabledThresholdSizeValueLabel.Text = GetParamValueFromTrackBar(disabledThresholdSizeTrackBar.Value).ToString();
		}

		private void UpdateColorView(PictureBox pictureBox, Color color)
		{
			Image colorImage = new Bitmap(pictureBox.Width, pictureBox.Height);

			using (var graphics = Graphics.FromImage(colorImage))
			using (var brush = new SolidBrush(color))
			{
				Rectangle colorImageRect = new Rectangle(0, 0, colorImage.Width, colorImage.Height);
				graphics.FillRectangle(brush, colorImageRect);
			}

			var oldImage = pictureBox.Image;
			pictureBox.Image = colorImage;
			pictureBox.Invalidate();

			if (oldImage != null)
			{
				oldImage.Dispose();
			}
		}

		private int GetTrackBarValue(float paramValue)
		{
			return (int)(paramValue * 10);
		}

		private float GetParamValueFromTrackBar(int trackBarValue)
		{
			return (float)trackBarValue / 10;
		}
		
		private Color GetColor(Vector3 vector)
		{
			int red = GetColorValue(vector.X);
			int green = GetColorValue(vector.Y);
			int blue = GetColorValue(vector.Z);

			return Color.FromArgb(red, green, blue);
		}

		private int GetColorValue(float vectorValue)
		{
			if (vectorValue < 0)
			{
				return 0;
			}
			else if (vectorValue > 1)
			{
				return 255;
			}
			else
			{
				return (int)(vectorValue * 255);
			}
		}

		private Vector3 GetColorVector(Color color)
		{
			return new Vector3 (
				(float)color.R / 255,
				(float)color.G / 255,
				(float)color.B / 255
			);
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			VisualizerParameters newVisualizerParameters = new VisualizerParameters();

			try
			{
				newVisualizerParameters.ArrowWidthMinScale = GetParamValueFromTrackBar(minWidthScaleTrackBar.Value);
				newVisualizerParameters.ArrowWidthMaxScale = GetParamValueFromTrackBar(maxWidthScaleTrackBar.Value);
				newVisualizerParameters.ArrowHeightMinScale = GetParamValueFromTrackBar(minHeightScaleTrackBar.Value);
				newVisualizerParameters.ArrowHeightMaxScale = GetParamValueFromTrackBar(maxHeightScaleTrackBar.Value);
				newVisualizerParameters.ArrowOutwardsColor = GetColorVector(arrowOutwardsColorDialog.Color);
				newVisualizerParameters.ArrowInwardsColor = GetColorVector(arrowInwardsColorDialog.Color);
				newVisualizerParameters.ColorMetricOutwards = GetColorVector(colorMetricOutwardsDialog.Color);
				newVisualizerParameters.ColorMetricInwards = GetColorVector(colorMetricInwardsDialog.Color);
				newVisualizerParameters.DisabledColor = GetColorVector(disabledColorDialog.Color);
				newVisualizerParameters.ColorDiffThreshold = GetParamValueFromTrackBar(colorDiffThresholdTrackBar.Value);
				newVisualizerParameters.DisabledThresholdLength = GetParamValueFromTrackBar(disabledThresholdLengthTrackBar.Value);
				newVisualizerParameters.DisabledThresholdSize = GetParamValueFromTrackBar(disabledThresholdSizeTrackBar.Value);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			try
			{
				newVisualizerParameters.ValidateValues();
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			newVisualizerParameters.CopyInto(parametersToEdit);
			this.Close();
		}

		private void trackBar_ValueChanged(object sender, EventArgs e)
		{
			UpdateTrackBarLabels();
		}

		private void arrowOutwardsColorButton_Click(object sender, EventArgs e)
		{
			if (arrowOutwardsColorDialog.ShowDialog() == DialogResult.OK)
			{
				UpdateColorView(arrowOutwardsColorPictureBox, arrowOutwardsColorDialog.Color);
			}
		}

		private void arrowInwardsColorButton_Click(object sender, EventArgs e)
		{
			if (arrowInwardsColorDialog.ShowDialog() == DialogResult.OK)
			{
				UpdateColorView(arrowInwardsColorPictureBox, arrowInwardsColorDialog.Color);
			}
		}

		private void colorMetricOutwardsButton_Click(object sender, EventArgs e)
		{
			if (colorMetricOutwardsDialog.ShowDialog() == DialogResult.OK)
			{
				UpdateColorView(colorMetricOutwardsPictureBox, colorMetricOutwardsDialog.Color);
			}
		}

		private void colorMetricInwardsButton_Click(object sender, EventArgs e)
		{
			if (colorMetricInwardsDialog.ShowDialog() == DialogResult.OK)
			{
				UpdateColorView(colorMetricInwardsPictureBox, colorMetricInwardsDialog.Color);
			}
		}

		private void disabledColorButton_Click(object sender, EventArgs e)
		{
			if (disabledColorDialog.ShowDialog() == DialogResult.OK)
			{
				UpdateColorView(disabledColorPictureBox, disabledColorDialog.Color);
			}
		}
	}
}

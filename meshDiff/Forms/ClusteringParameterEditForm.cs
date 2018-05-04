// author: Jan Horesovsky

using System;
using System.Windows.Forms;

namespace meshDiff
{
	public partial class ClusteringParameterEditForm : Form
	{
		ClusteringParameters arrowParametersToEdit;
		ClusteringParameters colorParametersToEdit;

		ClusteringParameters tempArrowParameters;
		ClusteringParameters tempColorParameters;

		public ClusteringParameterEditForm(ClusteringParameters arrowParametersToEdit, ClusteringParameters colorParametersToEdit)
		{
			InitializeComponent();

			this.arrowParametersToEdit = arrowParametersToEdit;
			this.colorParametersToEdit = colorParametersToEdit;

			tempArrowParameters = new ClusteringParameters("temp");
			tempColorParameters = new ClusteringParameters("temp");

			// this has to come before the temp values are initialized 
			// because it copies current trackbar values into the newly checked parameters set!
			arrowsRadioButton.Checked = true;

			arrowParametersToEdit.CopyInto(tempArrowParameters);
			colorParametersToEdit.CopyInto(tempColorParameters);

			InitTrackBarValues(arrowParametersToEdit);
			UpdateTrackBarLabels();
		}

		private void InitTrackBarValues(ClusteringParameters parameters)
		{
			directionSignificanceTrackBar.Value = parameters.DirectionSignificance;
			magnitudeSignificanceTrackBar.Value = parameters.MagnitudeSignificance;
			positionSignificanceTrackBar.Value = parameters.PositionSignificance;
			resolutionSignificanceTrackBar.Value = parameters.ResolutionSignificance;
		}

		private void UpdateTrackBarLabels()
		{
			directionSignificanceValueLabel.Text = directionSignificanceTrackBar.Value.ToString();
			magnitudeSignificanceValueLabel.Text = magnitudeSignificanceTrackBar.Value.ToString();
			positionSignificanceValueLabel.Text = positionSignificanceTrackBar.Value.ToString();
			resolutionSignificanceValueLabel.Text = resolutionSignificanceTrackBar.Value.ToString();
		}

		private void SaveTempValues(ClusteringParameters tempParameters)
		{
			try
			{
				tempParameters.DirectionSignificance = directionSignificanceTrackBar.Value;
				tempParameters.MagnitudeSignificance = magnitudeSignificanceTrackBar.Value;
				tempParameters.PositionSignificance = positionSignificanceTrackBar.Value;
				tempParameters.ResolutionSignificance = resolutionSignificanceTrackBar.Value;
			}
			catch (ArgumentOutOfRangeException ex)
			{
				MessageBox.Show(ex.Message, "Invalid values chosen!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				tempParameters = new ClusteringParameters("temp");
			}
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (arrowsRadioButton.Checked)
			{
				SaveTempValues(tempArrowParameters);
			}
			else
			{
				SaveTempValues(tempColorParameters);
			}

			try
			{
				tempArrowParameters.ValidateValues();
				tempColorParameters.ValidateValues();
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			tempArrowParameters.CopyInto(arrowParametersToEdit);
			tempColorParameters.CopyInto(colorParametersToEdit);

			this.Close();
		}

		private void trackBar_ValueChanged(object sender, EventArgs e)
		{
			UpdateTrackBarLabels();
		}

		private void arrowsRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (arrowsRadioButton.Checked)
			{
				SaveTempValues(tempColorParameters);
				InitTrackBarValues(tempArrowParameters);
				UpdateTrackBarLabels();
			}
		}

		private void colorsRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (colorsRadioButton.Checked)
			{
				SaveTempValues(tempArrowParameters);
				InitTrackBarValues(tempColorParameters);
				UpdateTrackBarLabels();
			}
		}
	}
}

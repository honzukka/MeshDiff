// author: Jan Horesovsky

using System;
using System.Windows.Forms;

namespace meshDiff
{
	public partial class MetricsEditForm : Form
	{
		public MetricType ChosenMetric { get; set; }

		public MetricsEditForm(MetricType currentMetric)
		{
			InitializeComponent();

			switch (currentMetric)
			{
				case MetricType.Distance:
					distanceRadioButton.Checked = true;
					projectedDistanceRadioButton.Checked = false;
					break;
				case MetricType.NormalProjectedDistance:
					distanceRadioButton.Checked = false;
					projectedDistanceRadioButton.Checked = true;
					break;
			}
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (distanceRadioButton.Checked)
			{
				ChosenMetric = MetricType.Distance;
			}
			else if (projectedDistanceRadioButton.Checked)
			{
				ChosenMetric = MetricType.NormalProjectedDistance;
			}

			DialogResult = DialogResult.OK;

			this.Close();
		}
	}
}

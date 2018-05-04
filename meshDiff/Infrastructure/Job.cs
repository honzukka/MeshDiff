// author: Jan Horesovsky

namespace meshDiff
{
	class Job
	{
		JobParameters parameters;
		ProgressDialog progressDialog;

		public bool HasSucceeded { get; private set; }

		public Job(JobParameters parameters, ProgressDialog progressDialog)
		{
			this.parameters = parameters;
			this.progressDialog = progressDialog;
			HasSucceeded = true;
		}

		public void Run()
		{
			if ((parameters.DiffForColors == null && parameters.DiffForArrows == null) ||
				parameters == null || 
				progressDialog == null)
			{
				progressDialog.ShowErrorMessage("Invalid input.");
				progressDialog.CanBeClosed = true;
				HasSucceeded = false;
				return;
			}

			var scene1Color = parameters.Scene1Color;
			var scene2Color = parameters.Scene2Color;
			var scene1Arrows = parameters.Scene1Arrows;
			var scene2Arrows = parameters.Scene2Arrows;

			// set total tasks
			int totalTasks = 0;
			totalTasks += (parameters.DiffForColors != null) ? parameters.DiffForColors.GetTotalTasks() : 0;
			totalTasks += (parameters.DiffForArrows != null) ? parameters.DiffForArrows.GetTotalTasks() : 0;
			progressDialog.SetTotalTasks(totalTasks);

			// color visualization
			if (parameters.DiffForColors != null)
			{
				HasSucceeded = false;
				progressDialog.AddInfoMessage("Creating color visualization...", false);

				if (parameters.DiffForColors.CreateVisualization(parameters.Metric, parameters.ClusteringParametersColors, parameters.VisualizerParameters,
					parameters.VisualizerColor, ref scene1Color, ref scene2Color, progressDialog))
				{
					progressDialog.AddInfoMessage("Color visualization ready!", false);
					HasSucceeded = true;
				}
			}

			if (HasSucceeded == false)
			{
				progressDialog.CanBeClosed = true;
				return;
			}

			// arrow visualization
			if (parameters.DiffForArrows != null)
			{
				HasSucceeded = false;

				progressDialog.AddInfoMessage("Creating arrow visualization...", false);

				if (parameters.DiffForArrows.CreateVisualization(parameters.Metric, parameters.ClusteringParametersArrows, parameters.VisualizerParameters, 
					parameters.VisualizerArrow, ref scene1Arrows, ref scene2Arrows, progressDialog))
				{
					progressDialog.AddInfoMessage("Arrow visualization ready!", false);
					HasSucceeded = true;
				}
			}

			if (HasSucceeded == false)
			{
				progressDialog.CanBeClosed = true;
				return;
			}

			// react to cancellations which were requested right before the end of the process
			if (progressDialog?.IsCancellationRequested() == true)
			{
				progressDialog.ShowErrorMessage("Process cancelled.");
				progressDialog.CanBeClosed = true;
				HasSucceeded = false;
				return;
			}

			// all good
			progressDialog.CloseSafe();
		}
	}
}

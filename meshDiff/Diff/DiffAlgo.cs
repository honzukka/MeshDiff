// author: Jan Horesovsky

using System;
using System.Collections.Generic;
using Scene3D;

namespace meshDiff
{
	class DiffAlgo<T> : IDiffAlgo
		where T : Clustering
	{
		SceneBrep scene1, scene2;
		bool sceneDataValid;

		List<Arrow> arrows;
		List<Cluster> clusters;

		T clustering;
		readonly Func<List<Arrow>, SceneBrep, T> clusteringFactory;

		MetricType currentMetric;

		public DiffAlgo(SceneBrep scene1, SceneBrep scene2, Func<List<Arrow>, SceneBrep, T> clusteringFactory)
		{
			// check if the scenes provided are valid
			if (scene1 == null || scene2 == null ||
				scene1.Vertices == 0 || scene2.Vertices == 0 ||
				scene1.Vertices != scene2.Vertices)
			{
				this.scene1 = null;
				this.scene2 = null;

				sceneDataValid = false;
			}
			else
			{
				this.scene1 = scene1;
				this.scene2 = scene2;

				sceneDataValid = true;
			}

			arrows = null;
			clusters = null;

			clustering = null;
			this.clusteringFactory = clusteringFactory;
		}

		/// <summary>
		/// Creates a visualization based on the provided data, parameters, and uses the visualizer object to bake it into the output scenes.
		/// </summary>
		/// <param name="progressDialog">If this methon runs in a separate thread, it can report progress to this progress dialog.</param>
		/// <returns>TRUE if it succeeds, FALSE otherwise</returns>
		public bool CreateVisualization(MetricType metric, ClusteringParameters clusteringParameters, VisualizerParameters visParameters,
			IVisualizer visualizer, ref SceneBrep outputScene1, ref SceneBrep outputScene2, ProgressDialog progressDialog = null)
		{
			if (!sceneDataValid || visualizer == null)
			{
				progressDialog?.ShowErrorMessage("Invalid scene data.");
				return false;
			}

			// get the arrows for a given metric if not yet available
			if (arrows == null || currentMetric != metric)
			{
				arrows = Metrics.GetVectorMetric(scene1, scene2, metric);
				clustering = clusteringFactory(arrows, scene1);

				currentMetric = metric;
			}

			progressDialog?.IncrementProgressBarValue();
			progressDialog?.AddInfoMessage("Metric computed.", false);

			// cluster the arrows
			clusters = clustering.GetClusters(clusteringParameters, progressDialog);

			// react to requested cancellations
			if (progressDialog?.IsCancellationRequested() == true)
			{
				arrows = null;
				clustering = null;
				progressDialog?.ShowErrorMessage("Process cancelled.");
				return false;
			}
			progressDialog?.IncrementProgressBarValue();
			progressDialog?.AddInfoMessage("Metric clustered.", false);

			// bake the visualization
			bool hasSucceeded = true;
			hasSucceeded = visualizer.BakeVisualization(clusters, visParameters, ref outputScene1) && hasSucceeded;
			hasSucceeded = visualizer.BakeVisualizationInverted(clusters, visParameters, ref outputScene2) && hasSucceeded;

			// react to failures
			if (!hasSucceeded)
			{
				progressDialog?.ShowErrorMessage("Visualization baking failed.");
				return false;
			}

			progressDialog?.IncrementProgressBarValue();
			return true;
		}
		
		/// <summary>
		/// Returns the total number of tasks which will be reported to a progress dialog by this class.
		/// </summary>
		public int GetTotalTasks()
		{
			return 3;
		}
	}
}

// author: Jan Horesovsky

using Scene3D;

namespace meshDiff
{
	interface IDiffAlgo
	{
		bool CreateVisualization(MetricType metric, ClusteringParameters clusteringParameters, VisualizerParameters visParameters,
			IVisualizer visualizer, ref SceneBrep scene1, ref SceneBrep scene2, ProgressDialog progressDialog = null);
		int GetTotalTasks();
	}
}

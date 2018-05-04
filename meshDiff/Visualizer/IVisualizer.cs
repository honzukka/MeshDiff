// author: Jan Horesovsky

using System.Collections.Generic;
using Scene3D;

namespace meshDiff
{
	interface IVisualizer
	{
		bool BakeVisualization(List<Cluster> clusters, VisualizerParameters parameters, ref SceneBrep scene);
		bool BakeVisualizationInverted(List<Cluster> clusters, VisualizerParameters parameters, ref SceneBrep scene);
	}
}

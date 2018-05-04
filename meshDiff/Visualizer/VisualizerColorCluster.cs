// author: Jan Horesovsky

using System.Collections.Generic;
using OpenTK;
using Scene3D;

namespace meshDiff
{
	/// <summary>
	/// Color visualization where all vertices in the same cluster have the same color.
	/// </summary>
	abstract class VisualizerColorCluster : IVisualizer
	{
		protected int vertexCount;

		public VisualizerColorCluster(int vertexCount)
		{
			this.vertexCount = vertexCount;
		}

		/// <summary>
		/// Generates a color visualization based on the clusters and parameters supplied and applies it onto the scene.
		/// </summary>
		/// <returns>TRUE is it succeeds, FALSE otherwise</returns>
		public bool BakeVisualization(List<Cluster> clusters, VisualizerParameters parameters, ref SceneBrep scene)
		{
			return BakeVisualization(clusters, parameters, ref scene, false);
		}

		/// <summary>
		/// Generates an inverted color visualization based on the clusters and parameters supplied and applies it onto the scene.
		/// </summary>
		/// <returns>TRUE is it succeeds, FALSE otherwise</returns>
		public bool BakeVisualizationInverted(List<Cluster> clusters, VisualizerParameters parameters, ref SceneBrep scene)
		{
			return BakeVisualization(clusters, parameters, ref scene, true);
		}

		private bool BakeVisualization(List<Cluster> clusters, VisualizerParameters parameters, ref SceneBrep scene, bool isInverted)
		{
			if (scene == null || scene.Vertices == 0 ||
				clusters == null || clusters.Count == 0)
			{
				return false;
			}

			List<Vector3> visualization = GenerateColors(clusters, parameters, isInverted);

			if (visualization == null || visualization.Count == 0 ||
				scene.Vertices != visualization.Count)
			{
				return false;
			}

			for (int i = 0; i < scene.Vertices; i++)
			{
				scene.SetColor(i, visualization[i]);
			}

			return true;
		}
		protected abstract List<Vector3> GenerateColors(List<Cluster> clusters, VisualizerParameters parameters, bool isInverted);
	}
}

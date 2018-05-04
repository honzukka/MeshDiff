// author: Jan Horesovsky

using System;
using System.Collections.Generic;
using OpenTK;

namespace meshDiff
{
	/// <summary>
	/// Color visualization where the color of a cluster is random.
	/// </summary>
	class VisualizerColorClusterRandom : VisualizerColorCluster
	{
		public VisualizerColorClusterRandom(int vertexCount) : base(vertexCount)
		{
		
		}

		protected override List<Vector3> GenerateColors(List<Cluster> clusters, VisualizerParameters parameters, bool isInverted)
		{
			Vector3[] colors = new Vector3[vertexCount];
			Random rnd = new Random();

			foreach (var cluster in clusters)
			{
				Vector3 clusterColor = new Vector3(
					(float)rnd.NextDouble(),
					(float)rnd.NextDouble(),
					(float)rnd.NextDouble()
				);

				foreach (var primaryArrow in cluster.PrimaryArrows)
				{
					colors[primaryArrow.VertexHandle] = clusterColor;
				}
			}

			return new List<Vector3>(colors);
		}
	}
}

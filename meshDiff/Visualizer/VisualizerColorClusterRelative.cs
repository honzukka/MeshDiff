// author: Jan Horesovsky

using System.Collections.Generic;
using OpenTK;
using System.Diagnostics;

namespace meshDiff
{
	/// <summary>
	/// Color visualization where the color of a cluster is relative to the maximum difference found in the data provided.
	/// </summary>
	class VisualizerColorClusterRelative : VisualizerColorCluster
	{
		public VisualizerColorClusterRelative(int vertexCount) : base(vertexCount)
		{

		}

		protected override List<Vector3> GenerateColors(List<Cluster> clusters, VisualizerParameters parameters, bool isInverted)
		{
			Vector3[] colors = new Vector3[vertexCount];

			foreach (var cluster in clusters)
			{
				Debug.Assert(cluster.PrimaryArrows.Count > 0);

				int clusterId = cluster.PrimaryArrows[0].ClusterId;

				// create a color for this cluster based on the representative arrow
				Vector3 clusterColor;

				if (cluster.RepresentativeArrow.Direction.Length < parameters.DisabledThresholdLength ||
					cluster.Size < parameters.DisabledThresholdSize)
				{
					clusterColor = parameters.DisabledColor;
				}
				else
				{
					if ((cluster.RepresentativeArrow.Orientation == ArrowOrientation.Outwards && !isInverted) ||
						(cluster.RepresentativeArrow.Orientation == ArrowOrientation.Inwards && isInverted))
					{
						clusterColor = cluster.RepresentativeArrow.HeightRatio * parameters.ColorMetricOutwards;
					}
					else
					{
						clusterColor = cluster.RepresentativeArrow.HeightRatio * parameters.ColorMetricInwards;
					}
				}

				// assign the color to all primary arrows belonging to this cluster
				foreach (var primaryArrow in cluster.PrimaryArrows)
				{
					colors[primaryArrow.VertexHandle] = clusterColor;
				}
			}

			return new List<Vector3>(colors);
		}
	}
}

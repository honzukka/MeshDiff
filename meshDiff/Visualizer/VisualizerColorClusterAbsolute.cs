using System.Collections.Generic;
using OpenTK;
using System.Diagnostics;

namespace meshDiff
{
	/// <summary>
	/// Color visualization where the color of a cluster is relative to a given maximum difference.
	/// </summary>
	class VisualizerColorClusterAbsolute : VisualizerColorCluster
	{
		public VisualizerColorClusterAbsolute(int vertexCount) : base(vertexCount)
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
					float lengthRatio = cluster.RepresentativeArrow.Direction.Length / parameters.ColorDiffThreshold;

					// clip the values
					if (lengthRatio > 1)
					{
						lengthRatio = 1;
					}

					if ((cluster.RepresentativeArrow.Orientation == ArrowOrientation.Outwards && !isInverted) ||
						(cluster.RepresentativeArrow.Orientation == ArrowOrientation.Inwards && isInverted))
					{
						clusterColor = lengthRatio * parameters.ColorMetricOutwards;
					}
					else
					{
						clusterColor = lengthRatio * parameters.ColorMetricInwards;
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

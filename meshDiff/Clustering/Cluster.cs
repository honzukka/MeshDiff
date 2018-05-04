// author: Jan Horesovsky

using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;

namespace meshDiff
{
	public class Cluster : IComparable<Cluster>
	{
		/// <summary>
		/// A list of neighbouring clusters (potential merge candidates).
		/// Invariant: All neighbours of a non-clustered cluster are also not clustered.
		/// </summary>
		public HashSet<Cluster> Neighbours { get; set; }

		/// <summary>
		/// Marks the number of the algorithm step this cluster was created in.
		/// </summary>
		public int Level { get; }

		/// <summary>
		/// Representative arrow of this cluster.
		/// </summary>
		public Arrow RepresentativeArrow { get; private set; }

		/// <summary>
		/// Geometrical size of the cluster.
		/// </summary>
		public float Size { get; }

		/// <summary>
		/// The approximate density of the mesh in the area of the cluster.
		/// </summary>
		public float MeshResolution { get; }

		/// <summary>
		/// Number of arrows (clusters of level 0) in the cluster.
		/// </summary>
		public int Count { get; }

		/// <summary>
		/// The clustering error of the immediate children of this cluster.
		/// </summary>
		public float Error { get; }

		/// <summary>
		/// Is this cluster a part of a bigger cluster?
		/// If it is, its data no longer changes (e.g. the neighbour set doesn't get updated anymore).
		/// </summary>
		public bool IsClustered { get; set; }

		/// <summary>
		/// Left child in the tree that represents the merging process.
		/// </summary>
		public Cluster LeftChild { get; }

		/// <summary>
		/// Right child in the tree that represents the merging process.
		/// </summary>
		public Cluster RightChild { get; }

		/// <summary>
		/// A reference to all the scene vertices that belong to this cluster.
		/// </summary>
		public List<Arrow> PrimaryArrows { get; }

		/// <summary>
		/// The total number of vertices in the scene where this cluster lies.
		/// </summary>
		public int SpaceSize { get; }

		/// <summary>
		/// A method which is able to retrieve handles to all pairs of vertices which form a triangle with a given vertex from the scene.
		/// </summary>
		private Func<int, IEnumerable<Tuple<int, int>>> GetPairsForTriangle;

		/// <summary>
		/// A method which is able to retrieve handles to all vertices which form a triangle with a given pair of vertices from the scene.
		/// </summary>
		private Func<int, int, IEnumerable<int>> GetVerticesForTriangle;

		// many fields of the cluster class are assigned during the clustering process which is handled by the class, 
		// so the constructor is private and only a static method which can create clusters is exposed
		private Cluster(int level, Arrow representativeArrow, float size, float meshResolution, int count, float error, Cluster leftChild, Cluster rightChild, 
			List<Arrow> primaryArrows, int spaceSize, Func<int, IEnumerable<Tuple<int, int>>> getPairsForTriangle, Func<int, int, IEnumerable<int>> getVerticesForTriangle)
		{
			Neighbours = new HashSet<Cluster>();
			Level = level;
			RepresentativeArrow = representativeArrow;
			Size = size;
			MeshResolution = meshResolution;
			Count = count;
			Error = error;
			IsClustered = false;
			LeftChild = leftChild;
			RightChild = rightChild;
			PrimaryArrows = primaryArrows;
			SpaceSize = spaceSize;
			GetPairsForTriangle = getPairsForTriangle;
			GetVerticesForTriangle = getVerticesForTriangle;
		}

		/// <summary>
		/// Makes a cluster out of a single arrow.
		/// No neighbours are added since the structure of the arrow field isn't known here!
		/// </summary>
		/// <param name="representativeArrow">Representative arrow of this cluster.</param>
		/// <param name="meshResolution">The approximate density of the mesh in the area of the cluster.</param>
		/// <param name="spaceSize">The total number of vertices in the scene where this cluster lies.</param>
		/// <param name="getPairsForTriangle">A method which is able to retrieve handles to all pairs of vertices which form a triangle with a given vertex from the scene.</param>
		/// <param name="getVerticesForTriangle">A method which is able to retrieve handles to all vertices which form a triangle with a given pair of vertices from the scene.</param>
		public static Cluster MakeCluster(Arrow representativeArrow, float meshResolution, int spaceSize, Func<int, 
			IEnumerable<Tuple<int, int>>> getPairsForTriangle, Func<int, int, IEnumerable<int>> getVerticesForTriangle)
		{
			return new Cluster(0, representativeArrow, 0, meshResolution, 1, 0, null, null, new List<Arrow> { representativeArrow }, spaceSize, getPairsForTriangle, getVerticesForTriangle);
		}

		/// <summary>
		/// Merges two clusters into a new one.
		/// Updates neighbour lists of the affected clusters.
		/// </summary>
		/// <param name="cluster1">Merging candidate</param>
		/// <param name="cluster2">Merging candidate</param>
		/// <param name="level">The number of the clustering algorithm step.</param>
		/// <param name="clusteringParameters">Parameters.</param>
		/// <param name="error">The clustering error of the two chosen merging candidates.</param>
		public static Cluster MergeClusters(Cluster cluster1, Cluster cluster2, int level, ClusteringParameters clusteringParameters, float error, bool orientationFixNeeded = true)
		{
			// cluster count
			int newClusterCount = cluster1.Count + cluster2.Count;

			// approximation of cluster size
			float newSize = cluster1.ComputeNewClusterSize(cluster2);

			// representative arrow
			Vector3 newArrowOrigin = (cluster1.Size != 0 && cluster2.Size != 0) ?
				(cluster1.Size * cluster1.RepresentativeArrow.Origin + cluster2.Size * cluster2.RepresentativeArrow.Origin) / (cluster1.Size + cluster2.Size) :
				(cluster1.Count * cluster1.RepresentativeArrow.Origin + cluster2.Count * cluster2.RepresentativeArrow.Origin) / newClusterCount;

			Vector3 newArrowDirection = (cluster1.Size != 0 && cluster2.Size != 0) ?
				(cluster1.Size * cluster1.RepresentativeArrow.Direction + cluster2.Size * cluster2.RepresentativeArrow.Direction) / (cluster1.Size + cluster2.Size) :
				(cluster1.Count * cluster1.RepresentativeArrow.Direction + cluster2.Count * cluster2.RepresentativeArrow.Direction) / newClusterCount;

			// primary arrows
			List<Arrow> newPrimaryArrows = new List<Arrow>();
			newPrimaryArrows.AddRange(cluster1.PrimaryArrows);
			newPrimaryArrows.AddRange(cluster2.PrimaryArrows);

			// representative arrow orientation
			ArrowOrientation newRepresentativeArrowOrientation = orientationFixNeeded ?
				GetClusterOrientation(newPrimaryArrows) :
				cluster1.RepresentativeArrow.Orientation;	// assume that clusters of the same orientation are being merged

			Arrow newRepresentativeArrow = new Arrow(newArrowOrigin, newArrowDirection, -1, newRepresentativeArrowOrientation);

			// mesh resolution
			float newMeshResolution = (cluster1.MeshResolution + cluster2.MeshResolution) / 2;

			// form a new cluster
			Cluster newCluster = new Cluster(level, newRepresentativeArrow, newSize, newMeshResolution, newClusterCount, error, cluster1, cluster2, 
				newPrimaryArrows, cluster1.SpaceSize, cluster1.GetPairsForTriangle, cluster1.GetVerticesForTriangle);

			// set neighbours of the new cluster
			newCluster.Neighbours.UnionWith(cluster1.Neighbours);
			newCluster.Neighbours.UnionWith(cluster2.Neighbours);
			newCluster.Neighbours.Remove(cluster1);
			newCluster.Neighbours.Remove(cluster2);

			// update neighbours of surrounding clusters
			foreach (var neighbour in cluster1.Neighbours)
			{
				if (neighbour == cluster2)
					continue;

				neighbour.Neighbours.Remove(cluster1);
				neighbour.Neighbours.Add(newCluster);
			}
			foreach (var neighbour in cluster2.Neighbours)
			{
				if (neighbour == cluster1)
					continue;

				neighbour.Neighbours.Remove(cluster2);
				neighbour.Neighbours.Add(newCluster);
			}

			cluster1.IsClustered = true;
			cluster2.IsClustered = true;

			return newCluster;
		}

		/// <param name="otherCluster">The other cluster to measure the error with.</param>
		/// <param name="clusteringParameters">Parameters.</param>
		/// <param name="maxMeshResolution">The maximum mesh resolution from all areas in the whole scene.</param>
		/// <param name="maxDistance">The maximum distance between two clusters in the whole scene.</param>
		/// <param name="maxMagnitude">The maximum length of an arrow in the whole scene.</param>
		public float GetClusteringError(Cluster otherCluster, ClusteringParameters clusteringParameters, float maxMeshResolution, float maxDistance, float maxMagnitude)
		{
			Vector3 thisPosition = this.RepresentativeArrow.Origin;
			Vector3 otherPosition = otherCluster.RepresentativeArrow.Origin;

			Vector3 thisVector = this.RepresentativeArrow.Direction;
			Vector3 otherVector = otherCluster.RepresentativeArrow.Direction;

			// get the difference from the other cluster
			float positionDifference = (otherPosition - thisPosition).Length;
			float directionDifference = Math.Abs(Helpers.GetAngle(thisVector, otherVector));
			float magnitudeDifference = Math.Abs(thisVector.Length - otherVector.Length);
			float totalMeshResolution = this.MeshResolution + otherCluster.MeshResolution;

			float totalSignificance =
				clusteringParameters.DirectionSignificance +
				clusteringParameters.PositionSignificance +
				clusteringParameters.MagnitudeSignificance +
				clusteringParameters.ResolutionSignificance;

			if (totalSignificance == 0)
			{
				throw new ArgumentException("Total significance has to be larger than zero.");
			}

			float error = 0f;

			// apply weigths to the difference to get the error
			if (maxDistance > 0)
			{
				error += (clusteringParameters.PositionSignificance / totalSignificance) * (positionDifference / maxDistance);
			}
			if (maxMagnitude > 0)
			{
				error += (clusteringParameters.MagnitudeSignificance / totalSignificance) * (magnitudeDifference / maxMagnitude);
			}
			if (maxMeshResolution > 0)
			{
				error += (clusteringParameters.ResolutionSignificance / totalSignificance) * (totalMeshResolution / maxMeshResolution);
			}
			if (!float.IsNaN(directionDifference))
			{
				error += (float)((clusteringParameters.DirectionSignificance / totalSignificance) * (directionDifference / Math.PI));
			}

			Debug.Assert(error >= 0);

			return error;
		}

		private static ArrowOrientation GetClusterOrientation(List<Arrow> primaryArrows)
		{
			int outwardsCount = 0;
			int inwardsCount = 0;

			// decide according to the orientation of the majority of primary arrows
			foreach (var arrow in primaryArrows)
			{
				if (arrow.Orientation == ArrowOrientation.Outwards)
				{
					outwardsCount++;
				}
				else if (arrow.Orientation == ArrowOrientation.Inwards)
				{
					inwardsCount++;
				}
			}

			if (outwardsCount > inwardsCount)
			{
				return ArrowOrientation.Outwards;
			}
			else if (inwardsCount > outwardsCount)
			{
				return ArrowOrientation.Inwards;
			}
			else
			{
				return ArrowOrientation.Outwards;
			}
		}

		// an approximation of the Euclidean area of the new cluster
		private float ComputeNewClusterSize(Cluster otherCluster)
		{
			// 1-1: no area
			if (this.Count == 1 && otherCluster.Count == 1)
			{
				return 0;
			}

			// 1-2: triangle area
			if (this.Count + otherCluster.Count == 3)
			{
				Vector3 vertex1, vertex2, vertex3;

				if (this.Count == 2)
				{
					vertex1 = this.LeftChild.RepresentativeArrow.Origin;
					vertex2 = this.RightChild.RepresentativeArrow.Origin;
					vertex3 = otherCluster.RepresentativeArrow.Origin;
				}
				else
				{
					vertex1 = this.RepresentativeArrow.Origin;
					vertex2 = otherCluster.LeftChild.RepresentativeArrow.Origin;
					vertex3 = otherCluster.RightChild.RepresentativeArrow.Origin;
				}

				return Vector3.Cross(vertex2 - vertex1, vertex2 - vertex3).Length / 2;
			}

			// 1-many: new triangles + big cluster area
			if (this.Count == 1 || otherCluster.Count == 1)
			{
				if (this.Count == 1)
				{
					return GetNewArea(otherCluster, this.RepresentativeArrow) + otherCluster.Size;
				}
				else
				{
					return GetNewArea(this, otherCluster.RepresentativeArrow) + this.Size;
				}
			}

			// 2-any: all new triangles + big cluster area
			if (this.Count == 2 || otherCluster.Count == 2)
			{
				if (this.Count == 2)
				{
					return GetNewArea(otherCluster, this.LeftChild.RepresentativeArrow) +
						GetNewArea(otherCluster, this.RightChild.RepresentativeArrow) +
						GetNewArea(otherCluster, this.LeftChild.RepresentativeArrow, this.RightChild.RepresentativeArrow) +
						otherCluster.Size;
				}
				else
				{
					return GetNewArea(this, otherCluster.LeftChild.RepresentativeArrow) +
						GetNewArea(this, otherCluster.RightChild.RepresentativeArrow) +
						GetNewArea(this, otherCluster.LeftChild.RepresentativeArrow, otherCluster.RightChild.RepresentativeArrow) +
						this.Size;
				}
			}

			// many-many: just a sum of areas (slightly inaccurate)
			if (this.Count >= 3 && otherCluster.Count >= 3)
			{
				return this.Size + otherCluster.Size;
			}

			// all cases should be covered above
			Debug.Assert(false);

			return 0f;
		}

		private Vector3? GetArrowOrigin(int vertexHandle)
		{
			foreach (var arrow in this.PrimaryArrows)
			{
				if (arrow.VertexHandle == vertexHandle)
				{
					return arrow.Origin;
				}
			}

			return null;
		}

		// get the area between the cluster and the arrow which will be added to the cluster by adding the arrow
		private float GetNewArea(Cluster cluster, Arrow arrow)
		{
			Debug.Assert(arrow.VertexHandle != -1);

			var trianglePairs = GetPairsForTriangle(arrow.VertexHandle);
			List<Tuple<Vector3, Vector3, Vector3>> newTriangles = new List<Tuple<Vector3, Vector3, Vector3>>();

			foreach (var trianglePair in trianglePairs)
			{
				Vector3? vertex2 = cluster.GetArrowOrigin(trianglePair.Item1);
				Vector3? vertex3 = cluster.GetArrowOrigin(trianglePair.Item2);

				if (vertex2 != null && vertex3 != null)
				{
					newTriangles.Add(new Tuple<Vector3, Vector3, Vector3>(
						arrow.Origin,
						vertex2.Value,
						vertex3.Value
					));
				}
			}

			float newArea = 0;
			foreach (var triangle in newTriangles)
			{
				newArea += Vector3.Cross(triangle.Item2 - triangle.Item1, triangle.Item2 - triangle.Item3).Length / 2;
			}

			return newArea;
		}

		// get the area between the cluster and the two arrows which will be added to the cluster by adding the arrows
		private float GetNewArea(Cluster cluster, Arrow arrow1, Arrow arrow2)
		{
			Debug.Assert(arrow1.VertexHandle != -1 && arrow2.VertexHandle != -1);

			var triangleVertices = GetVerticesForTriangle(arrow1.VertexHandle, arrow2.VertexHandle);
			List<Tuple<Vector3, Vector3, Vector3>> newTriangles = new List<Tuple<Vector3, Vector3, Vector3>>();

			foreach (var triangleVertex in triangleVertices)
			{
				Vector3? vertex3 = cluster.GetArrowOrigin(triangleVertex);

				if (vertex3 != null)
				{
					newTriangles.Add(new Tuple<Vector3, Vector3, Vector3>(
						arrow1.Origin, 
						arrow2.Origin,
						vertex3.Value
					));
				}
			}

			float newArea = 0;
			foreach (var triangle in newTriangles)
			{
				newArea += Vector3.Cross(triangle.Item2 - triangle.Item1, triangle.Item2 - triangle.Item3).Length / 2;
			}

			return newArea;
		}

		/// <summary>
		/// The comparison is based on the level of the cluster which marks the number of the algorithm step the cluster was created in.
		/// </summary>
		public int CompareTo(Cluster otherCluster)
		{
			if (Level < otherCluster.Level)
			{
				return -1;
			}
			else if (Level > otherCluster.Level)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}
	}
}

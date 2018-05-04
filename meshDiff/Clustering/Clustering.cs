// author: Jan Horesovsky

using System;
using System.Collections.Generic;
using Scene3D;
using System.Diagnostics;

namespace meshDiff
{
	/// <summary>
	/// This family of classes transforms arrows into clusters.
	/// </summary>
	abstract class Clustering
	{
		protected List<Arrow> arrows;
		protected SceneBrep scene;

		protected ClusteringParameters currentParameters;
		protected int currentClusterCount;
		protected List<Arrow> currentChosenArrows;

		protected float maxMeshResolution;
		protected float maxDistance;
		protected float maxMagnitude;

		/// <summary>
		/// Initializes the Clustering object with a list of arrows and a scene the arrows are tied to.
		/// Only references to both are saved!
		/// </summary>
		public Clustering(List<Arrow> arrows, SceneBrep scene)
		{
			this.arrows = arrows;
			this.scene = scene;

			currentParameters = new ClusteringParameters("temp");
			currentClusterCount = 0;
			currentChosenArrows = null;

			maxMeshResolution = 0;
			maxDistance = scene.GetDiameter();
			maxMagnitude = 0;
		}

		/// <summary>
		/// Returns clusters based on the parameters of the clustering.
		/// </summary>
		/// <param name="parameters">Clustering parameters.</param>
		/// <param name="progressDialog">If this method runs in a separate thread, it can report progress to the progress dialog.</param>
		/// <returns>Null if it fails.</returns>
		public virtual List<Cluster> GetClusters(ClusteringParameters parameters, ProgressDialog progressDialog = null)
		{
			if (parameters == null)
			{
				parameters = new ClusteringParameters("temp");
			}

			if (!ClusteringExists() || !parameters.HasSameValuesAsExceptForClusterCount(currentParameters))
			{
				// required clustering doesn't exist yet, so let's make it
				parameters.CopyInto(currentParameters);
				if (PerformClustering(progressDialog) == false)
				{
					return null;
				}
			}

			List<Cluster> clusters = GetClusters(parameters.ClusterCount);

			// set cluster fields which depend on the clustering of the scene
			SetClusterId(clusters);
			SetWidthRatio(clusters);
			SetHeightRatio(clusters);

			return clusters;
		}

		protected abstract bool ClusteringExists();

		protected abstract bool PerformClustering(ProgressDialog progressDialog = null);

		/// <summary>
		/// Returns a clustering of the while space containing the given number of clusters.
		/// </summary>
		/// <returns>Null if it fails (e.g. if there is no clustering available).</returns>
		protected abstract List<Cluster> GetClusters(int clusterCount);

		/// <summary>
		/// Returns the geometrical size of all the clusters in the current clustering.
		/// </summary>
		protected abstract float GetTotalSize();

		/// <summary>
		/// Creates clusters out of arrows and initializes all of their neighbour lists.
		/// </summary>
		/// <param name="arrows">Arrows.</param>
		/// <returns>Null if it fails</returns>
		protected List<Cluster> InitializeClusters(List<Arrow> arrows)
		{
			if (arrows == null)
			{
				return null;
			}

			List<Cluster> clusters = new List<Cluster>();

			foreach (var arrow in arrows)
			{
				float meshResolution = scene.GetMeshResolution(arrow.VertexHandle);

				Cluster cluster = Cluster.MakeCluster(
					arrow,
					meshResolution,
					arrows.Count, 
					scene.GetPairsForTriangle,
					scene.GetVerticesForTriangle
				);
				clusters.Add(cluster);

				if (meshResolution > maxMeshResolution)
				{
					maxMeshResolution = meshResolution;
				}

				if (arrow.Direction.Length > maxMagnitude)
				{
					maxMagnitude = arrow.Direction.Length;
				}
			}

			// populate the neighbour lists of all the clusters
			foreach (var cluster in clusters)
			{
				var clusterNeighboursIndices = scene.GetVertexNeighbours(cluster.RepresentativeArrow.VertexHandle);
				foreach (var neighbourIndex in clusterNeighboursIndices)
				{
					// !!! uses the fact that clusters are saved in the same order as vertices in the scene !!!
					Debug.Assert(neighbourIndex == clusters[neighbourIndex].RepresentativeArrow.VertexHandle);

					cluster.Neighbours.Add(clusters[neighbourIndex]);
				}
			}

			return clusters;
		}

		/// <summary>
		/// Assigns the same id to all primary arrows which belong to the same cluster.
		/// </summary>
		protected void SetClusterId(List<Cluster> clusters)
		{
			if (clusters == null)
			{
				return;
			}

			int clusterId = 0;

			foreach (var cluster in clusters)
			{
				foreach (var primaryArrow in cluster.PrimaryArrows)
				{
					primaryArrow.ClusterId = clusterId;
				}

				clusterId++;
			}
		}

		protected void SetWidthRatio(List<Cluster> clusters)
		{
			if (clusters == null)
			{
				return;
			}

			float totalClusterSize = GetTotalSize();

			foreach (var cluster in clusters)
			{
				// set the width ratio based on the complete clustering
				cluster.RepresentativeArrow.WidthRatio = cluster.Size / totalClusterSize;
			}
		}

		protected void SetHeightRatio(List<Cluster> clusters)
		{
			if (clusters == null)
			{
				return;
			}

			float minLength = float.MaxValue;
			float maxLength = float.MinValue;

			foreach (var cluster in clusters)
			{
				float arrowLength = cluster.RepresentativeArrow.Direction.Length;

				if (arrowLength > maxLength)
				{
					maxLength = arrowLength;
				}

				if (arrowLength < minLength)
				{
					minLength = arrowLength;
				}
			}

			float lengthRange = maxLength - minLength;

			foreach (var cluster in clusters)
			{
				float lengthRatio = (lengthRange != 0) ?
					(cluster.RepresentativeArrow.Direction.Length - minLength) / lengthRange :
					0;

				Debug.Assert(lengthRatio >= 0 && lengthRatio <= 1);

				cluster.RepresentativeArrow.HeightRatio = lengthRatio;
			}
		}

		/// <summary>
		/// This class represents candidate clusters for merging.
		/// </summary>
		protected class ClusterPair : IComparable<ClusterPair>
		{
			public Cluster Cluster1 { get; }
			public Cluster Cluster2 { get; }
			public float ClusteringError { get; }

			public ClusterPair(Cluster cluster1, Cluster cluster2, float clusteringError)
			{
				Cluster1 = cluster1;
				Cluster2 = cluster2;
				ClusteringError = clusteringError;
			}

			public int CompareTo(ClusterPair other)
			{
				if (ClusteringError < other.ClusteringError)
				{
					return -1;
				}
				else if (ClusteringError > other.ClusteringError)
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
}

// author: Jan Horesovsky

using System.Collections.Generic;
using Scene3D;
using System.Diagnostics;

namespace meshDiff
{
	/// <summary>
	/// This class performs a clustering by considering only neighboring arrows with the same orientation as potential merge candidates.
	/// </summary>
	class ClusteringSigned : Clustering
	{
		List<Cluster> rootClusters;

		public ClusteringSigned(List<Arrow> arrows, SceneBrep scene) : base(arrows, scene)
		{
			rootClusters = null;
		}

		public static ClusteringSigned Create(List<Arrow> arrows, SceneBrep scene)
		{
			return new ClusteringSigned(arrows, scene);
		}

		protected override bool ClusteringExists()
		{
			return rootClusters != null;
		}

		/// <summary>
		/// Builds a forest of cluster hierarchies (trees). 
		/// Each root of a hierarchy is surrounded by clusters that it cannot be clustered with.
		/// </summary>
		/// <returns>False if it fails, true otherwise.</returns>
		protected override bool PerformClustering(ProgressDialog progressDialog = null)
		{
			if (arrows == null || arrows.Count == 0)
			{
				return false;
			}

			progressDialog?.AddInfoMessage("Initializing clustering...", false);

			List<Cluster> clusters = InitializeClusters(arrows);

			rootClusters = new List<Cluster>();
			MinHeap<ClusterPair> clusteringCandidates = new MinHeap<ClusterPair>();

			// form initial clustering pairs
			foreach (var cluster in clusters)
			{
				if (AddClusteringCandidates(cluster, clusteringCandidates) == false)
				{
					rootClusters.Add(cluster);
				}
			}

			int level = 0;

			if (progressDialog?.IsCancellationRequested() == true)
			{
				rootClusters = null;
				return false;
			}
			progressDialog?.AddInfoMessage("Clustering...", false);

			// in each step merge the most similar clusters and create new clustering candidates based on the changes
			while (clusteringCandidates.Count > 0)
			{
				ClusterPair bestCandidates = clusteringCandidates.ExtractMin();

				if (bestCandidates.Cluster1.IsClustered == false && bestCandidates.Cluster2.IsClustered == false)
				{
					level++;
					Cluster newCluster = Cluster.MergeClusters(bestCandidates.Cluster1, bestCandidates.Cluster2, level, currentParameters, bestCandidates.ClusteringError, false);

					if (AddClusteringCandidates(newCluster, clusteringCandidates) == false)
					{
						rootClusters.Add(newCluster);
					}
				}

				if (progressDialog?.IsCancellationRequested() == true)
				{
					rootClusters = null;
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// It forms all pairs (cluster, its neighbour) that have the same orientation and adds them into clusteringCandidates.
		/// </summary>
		/// <returns>False if no pairs exist, true otherwise.</returns>
		private bool AddClusteringCandidates(Cluster cluster, MinHeap<ClusterPair> clusteringCandidates)
		{
			bool hasGoodNeighbours = false;

			foreach (var neighbour in cluster.Neighbours)
			{
				if (cluster.RepresentativeArrow.Orientation == neighbour.RepresentativeArrow.Orientation)
				{
					hasGoodNeighbours = true;
					float error = cluster.GetClusteringError(neighbour, currentParameters, maxMeshResolution, maxDistance, maxMagnitude);

					Debug.Assert(!float.IsNaN(error));

					ClusterPair clusterPair = new ClusterPair(cluster, neighbour, error);
					clusteringCandidates.Insert(clusterPair);
				}
			}

			return hasGoodNeighbours;
		}

		/// <summary>
		/// Returns a clustering of the while space containing the given number of clusters.
		/// If more clusters are requested than there are clusters available, returns all available clusters.
		/// </summary>
		/// <returns>Null if it fails (e.g. if there is no clustering available).</returns>
		protected override List<Cluster> GetClusters(int clusterCount)
		{
			if (rootClusters == null || rootClusters.Count == 0)
			{
				return null;
			}

			int totalClusterCount = 0;
			foreach (var rootCluster in rootClusters)
			{
				totalClusterCount += rootCluster.Count;
			}

			if (clusterCount < 1)
			{
				return null;
			}

			if (clusterCount > totalClusterCount)
			{
				clusterCount = totalClusterCount;
			}

			// for a small number of clusters, we need to get the biggest clusters overall, 
			// not just the biggest root clusters
			MaxHeap<Cluster> chosenClusters = new MaxHeap<Cluster>();
			foreach (var rootCluster in rootClusters)
			{
				chosenClusters.Insert(rootCluster);
			}

			// choose clusters from the forest
			for (int i = 0; i < clusterCount; i++)
			{
				Cluster highestCluster = chosenClusters.ExtractMax();

				if (highestCluster.Level > 0)
				{
					chosenClusters.Insert(highestCluster.LeftChild);
					chosenClusters.Insert(highestCluster.RightChild);
				}
				else
				{
					// highest cluster is a root of level 0 => all clusters are in chosenClusters and we can proceed to the next step
					chosenClusters.Insert(highestCluster);
					break;
				}
				
			}

			// some of the clusters might have been chosen only because they are root, not because they are big
			// we need to remove these from the list now
			List<Cluster> chosenClusterList = new List<Cluster>();
			if (clusterCount < chosenClusters.Count)
			{
				for (int i = 0; i < clusterCount; i++)
				{
					chosenClusterList.Add(chosenClusters.ExtractMax());
				}
			}
			else
			{
				foreach (var cluster in chosenClusters)
				{
					chosenClusterList.Add(cluster);
				}
			}

			return chosenClusterList;
		}

		/// <summary>
		/// Returns the geometrical size of all the clusters in the current clustering.
		/// </summary>
		protected override float GetTotalSize()
		{
			if (rootClusters == null)
			{
				return 0f;
			}

			float totalSize = 0;
			foreach (var rootCluster in rootClusters)
			{
				totalSize += rootCluster.Size;
			}

			return totalSize;
		}
	}
}

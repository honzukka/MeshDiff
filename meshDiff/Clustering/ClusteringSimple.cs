// author: Jan Horesovsky

using System.Collections.Generic;
using Scene3D;
using System.Diagnostics;

namespace meshDiff
{
	/// <summary>
	/// This class performs a clustering by considering all neighbouring clusters to be potential merge candidates.
	/// </summary>
	class ClusteringSimple : Clustering
	{
		Cluster rootCluster;

		public ClusteringSimple(List<Arrow> arrows, SceneBrep scene) : base(arrows, scene)
		{
			rootCluster = null;
		}

		public static ClusteringSimple Create(List<Arrow> arrows, SceneBrep scene)
		{
			return new ClusteringSimple(arrows, scene);
		}

		protected override bool ClusteringExists()
		{
			return rootCluster != null;
		}

		/// <summary>
		/// Builds a cluster hierarchy (tree).
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

			MinHeap<ClusterPair> clusteringCandidates = new MinHeap<ClusterPair>();

			// form initial clustering candidate pairs
			foreach (var cluster in clusters)
			{
				AddClusteringCandidates(cluster, clusteringCandidates);
			}

			int level = 0;
			Cluster newCluster = clusters[0];   // the following loop will be skipped if there is just one arrow (basic cluster)

			if (progressDialog?.IsCancellationRequested() == true)
			{
				rootCluster = null;
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
					newCluster = Cluster.MergeClusters(bestCandidates.Cluster1, bestCandidates.Cluster2, level, currentParameters, bestCandidates.ClusteringError, true);

					AddClusteringCandidates(newCluster, clusteringCandidates);
				}

				if (progressDialog?.IsCancellationRequested() == true)
				{
					rootCluster = null;
					return false;
				}
			}

			rootCluster = newCluster;
			return true;
		}

		/// <summary>
		/// It forms pairs (cluster, its neighbour) and adds them into clusteringCandidates.
		/// </summary>
		private void AddClusteringCandidates(Cluster cluster, MinHeap<ClusterPair> clusteringCandidates)
		{
			foreach (var neighbour in cluster.Neighbours)
			{
				float error = cluster.GetClusteringError(neighbour, currentParameters, maxMeshResolution, maxDistance, maxMagnitude);

				Debug.Assert(!float.IsNaN(error));

				ClusterPair clusterPair = new ClusterPair(cluster, neighbour, error);
				clusteringCandidates.Insert(clusterPair);
			}
		}

		/// <summary>
		/// Returns a clustering of the while space containing the given number of clusters.
		/// If more clusters are requested than there are clusters available, returns all available clusters.
		/// </summary>
		/// <returns>Null if it fails (e.g. if there is no clustering available).</returns>
		protected override List<Cluster> GetClusters(int clusterCount)
		{
			if (rootCluster == null)
			{
				return null;
			}

			if (clusterCount < 1)
			{
				return null;
			}

			if (clusterCount > rootCluster.Count)
			{
				clusterCount = rootCluster.Count;
			}

			MaxHeap<Cluster> chosenClusters = new MaxHeap<Cluster>();
			chosenClusters.Insert(rootCluster);

			// choose clusters from the tree
			for (int i = 1; i < clusterCount; i++)
			{
				Cluster highestCluster = chosenClusters.ExtractMax();

				chosenClusters.Insert(highestCluster.LeftChild);
				chosenClusters.Insert(highestCluster.RightChild);
			}

			List<Cluster> chosenClusterList = new List<Cluster>();

			foreach (var cluster in chosenClusters)
			{
				chosenClusterList.Add(cluster);
			}

			return chosenClusterList;
		}

		/// <summary>
		/// Returns the geometrical size of all the clusters in the current clustering.
		/// </summary>
		protected override float GetTotalSize()
		{
			if (rootCluster == null)
			{
				return 0f;
			}

			return rootCluster.Size;
		}
	}
}

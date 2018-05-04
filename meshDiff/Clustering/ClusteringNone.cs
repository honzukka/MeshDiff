// author: Jan Horesovsky

using System;
using System.Collections.Generic;
using Scene3D;

namespace meshDiff
{
	/// <summary>
	/// This class simply creates clusters out of initial arrows and returns them without any further processing.
	/// </summary>
	class ClusteringNone : Clustering
	{
		public ClusteringNone(List<Arrow> arrows, SceneBrep scene) : base (arrows, scene)
		{

		}

		public static ClusteringNone Create(List<Arrow> arrows, SceneBrep scene)
		{
			return new ClusteringNone(arrows, scene);
		}


		/// <summary>
		/// Simply creates clusters out of initial arrows and returns them without any further processing.
		/// </summary>
		public override List<Cluster> GetClusters(ClusteringParameters parameters, ProgressDialog progressDialog = null)
		{
			List<Cluster> primaryClusters = InitializeClusters(arrows);

			SetHeightRatio(primaryClusters);

			return primaryClusters;
		}

		protected override bool ClusteringExists()
		{
			throw new NotImplementedException();
		}

		protected override List<Cluster> GetClusters(int clusterCount)
		{
			throw new NotImplementedException();
		}

		protected override bool PerformClustering(ProgressDialog progressDialog = null)
		{
			throw new NotImplementedException();
		}

		protected override float GetTotalSize()
		{
			throw new NotImplementedException();
		}
	}
}

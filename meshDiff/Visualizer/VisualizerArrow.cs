// author: Jan Horesovsky

using System;
using System.Collections.Generic;
using OpenTK;
using Scene3D;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace meshDiff
{
	/// <summary>
	/// Arrow visualization.
	/// </summary>
	class VisualizerArrow : IVisualizer
	{
		SceneBrep arrowModel;

		public VisualizerArrow(string arrowResourceName = "meshDiff.Resources.arrow8Sided.obj")
		{
			var assembly = Assembly.GetExecutingAssembly();

			using (Stream resourceStream = assembly.GetManifestResourceStream(arrowResourceName))
			using (StreamReader arrowFileReader = new StreamReader(resourceStream))
			{
				LoadArrowModel(arrowFileReader);
			}	
		}

		/// <summary>
		/// Generates an arrow visualization based on the clusters and parameters supplied and copies it into the scene.
		/// </summary>
		/// <returns>TRUE if it succeeds, FALSE otherwise</returns>
		public bool BakeVisualization(List<Cluster> clusters, VisualizerParameters parameters, ref SceneBrep scene)
		{
			if (scene == null || clusters == null || clusters.Count == 0)
			{
				return false;
			}

			List<SceneBrep> arrowModels = CreateArrowModels(clusters, parameters, false);
			return Helpers.CopyModelsInto(arrowModels, ref scene, true);
		}

		/// <summary>
		/// Generates an inverted arrow visualization based on the clusters and parameters supplied and copies it into the scene.
		/// </summary>
		/// <returns>TRUE if it succeeds, FALSE otherwise</returns>
		public bool BakeVisualizationInverted(List<Cluster> clusters, VisualizerParameters parameters, ref SceneBrep scene)
		{
			if (scene == null)
			{
				return false;
			}

			List<SceneBrep> arrowModels = CreateArrowModels(clusters, parameters, true);
			return Helpers.CopyModelsInto(arrowModels, ref scene, true);
		}

		private void LoadArrowModel(StreamReader arrowFileReader)
		{
			arrowModel = new SceneBrep();

			WavefrontObj objReader = new WavefrontObj();
			objReader.MirrorConversion = false;
			objReader.TextureUpsideDown = false;

			objReader.ReadBrep(arrowFileReader, arrowModel);

			arrowModel.BuildCornerTable();
			Debug.Assert(arrowModel.CheckCornerTable(null, true) == 0);

			arrowModel.GenerateColors(12);
			arrowModel.ComputeNormals();
		}

		private List<SceneBrep> CreateArrowModels(List<Cluster> clusters, VisualizerParameters parameters, bool isInverted)
		{
			if (clusters == null)
			{
				return null;
			}

			List<SceneBrep> arrowModels = new List<SceneBrep>(clusters.Count);

			foreach (var cluster in clusters)
			{
				Arrow arrow = cluster.RepresentativeArrow;

				// do not draw arrows which have zero length,
				// arrows which are below the threshold and
				// arrows which represent clusters which are too small
				if (arrow.Direction == Vector3.Zero ||
					arrow.Direction.Length < parameters.DisabledThresholdLength ||
					cluster.Size < parameters.DisabledThresholdSize)
				{
					continue;
				}

				SceneBrep newArrowModel = arrowModel.Clone();

				// arrow color
				Vector3 arrowColor;

				if ((arrow.Orientation == ArrowOrientation.Outwards && !isInverted) || 
					arrow.Orientation == ArrowOrientation.Inwards && isInverted)
				{
					arrowColor = parameters.ArrowOutwardsColor;
				}
				else
				{
					arrowColor = parameters.ArrowInwardsColor;
				}

				// assuming that the arrow model's origin is at (0, 0, 0)
				Matrix4 translationMatrix = isInverted ? 
					Matrix4.CreateTranslation(arrow.Origin + arrow.Direction) : 
					Matrix4.CreateTranslation(arrow.Origin);

				// width scaling
				float widthScalingRange = parameters.ArrowWidthMaxScale - parameters.ArrowWidthMinScale;
				float arrowWidthScale = (arrow.WidthRatio * widthScalingRange) + parameters.ArrowWidthMinScale;

				// height scaling
				float heightScalingRange = parameters.ArrowHeightMaxScale - parameters.ArrowHeightMinScale;
				float arrowHeightScale = (arrow.HeightRatio * heightScalingRange) + parameters.ArrowHeightMinScale;

				Matrix4 scalingMatrix = Matrix4.CreateScale(arrowWidthScale, arrowHeightScale, arrowWidthScale);

				// (0, 1, 0) is the orientation of arrowModel
				// arrow.Direction is definitely not Vector3.Zero, so the angle will be a valid number 
				double angle = isInverted ? 
					-Math.Acos(Vector3.Dot(new Vector3(0, 1, 0), -arrow.Direction.Normalized())) : 
					Math.Acos(Vector3.Dot(new Vector3(0, 1, 0), arrow.Direction.Normalized()));

				Vector3 axis = Vector3.Cross(new Vector3(0, 1, 0), arrow.Direction);
				Matrix4 rotationMatrix = Matrix4.CreateFromAxisAngle(axis, (float)angle);

				for (int i = 0; i < newArrowModel.Vertices; i++)
				{
					Vector3 originalVertex = newArrowModel.GetVertex(i);
					Vector4 tempVertex = new Vector4(originalVertex.X, originalVertex.Y, originalVertex.Z, 1);
					Vector4 newTempVertex = tempVertex * scalingMatrix * rotationMatrix * translationMatrix;
					Vector3 newVertex = new Vector3(newTempVertex.X, newTempVertex.Y, newTempVertex.Z);

					newArrowModel.SetVertex(i, newVertex);
					newArrowModel.SetColor(i, arrowColor);
				}

				newArrowModel.ComputeNormals();

				arrowModels.Add(newArrowModel);
			}

			return arrowModels;
		}
	}
}

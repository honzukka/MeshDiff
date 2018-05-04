// author: Jan Horesovsky

using System;
using System.Collections.Generic;
using OpenTK;
using Scene3D;
using System.Diagnostics;

namespace meshDiff
{
	static class Metrics
	{
		// a text representation of metric data which can be saved to a file
		const string fileSectionName = "Metric";

		const string metricName = "metric";
		const string distanceName = "distance";
		const string normalProjectedDistanceName = "normalProjectedDistance";

		/// <summary>
		/// Creates arrow instances from two scenes based on the given metric type.
		/// </summary>
		public static List<Arrow> GetVectorMetric(SceneBrep scene1, SceneBrep scene2, MetricType metricType)
		{
			switch (metricType)
			{
				case MetricType.Distance:
					return GetVertexDistances(scene1, scene2);
				case MetricType.NormalProjectedDistance:
					return GetVertexNormalProjectedDistances(scene1, scene2);
				default:
					throw new InvalidOperationException("Supplied metric type doesn't generate a vector metric.");
			}
		}

		public static void SaveToFile(MetricType metric, string path)
		{
			using (ParameterWriter paramWriter = new ParameterWriter(path, fileSectionName))
			{
				switch (metric)
				{
					case MetricType.Distance:
						paramWriter.WritePair(metricName, distanceName);
						break;
					case MetricType.NormalProjectedDistance:
						paramWriter.WritePair(metricName, normalProjectedDistanceName);
						break;
				}

				paramWriter.WriteEmptyLine();
			}
		}

		public static MetricType LoadFromFile(string path)
		{
			using (ParameterReader paramReader = new ParameterReader(path, fileSectionName))
			{
				Tuple<string, string> parameterPair = paramReader.ReadPair();
				
				if (parameterPair == null)
				{
					return default(MetricType);
				}
				
				switch (parameterPair.Item2)
				{
					case distanceName:
						return MetricType.Distance;
					case normalProjectedDistanceName:
						return MetricType.NormalProjectedDistance;
					default:
						return default(MetricType);
				}
			}
		}

		/// <summary>
		/// Returns a list of arrows representing oriented distances 
		/// between all pairs of corresponding vertices in provided scenes.
		/// </summary>
		private static List<Arrow> GetVertexDistances(SceneBrep scene1, SceneBrep scene2)
		{
			if (scene1 == null || scene2 == null ||
				scene1.Vertices != scene2.Vertices)
			{
				return null;
			}

			List<Arrow> arrows = new List<Arrow>();

			for (int i = 0; i < scene1.Vertices; i++)
			{
				Vector3 vertex1 = scene1.GetVertex(i);
				Vector3 vertex2 = scene2.GetVertex(i);
				Vector3 distance = vertex2 - vertex1;

				// find out the orientation
				Vector3 normal = scene1.GetNormal(i);
				double normalDistanceAngle = Helpers.GetAngle(normal, distance);
				ArrowOrientation orientation = (normalDistanceAngle < Math.PI / 2 && normalDistanceAngle > -Math.PI / 2) 
					? ArrowOrientation.Outwards 
					: ArrowOrientation.Inwards;

				arrows.Add(new Arrow(vertex1, distance, i, orientation));
			}

			return arrows;
		}

		/// <summary>
		/// Returns a list of arrows representing oriented distances 
		/// between all pairs of corresponding vertices in provided scenes
		/// projected into normals.
		/// </summary>
		private static List<Arrow> GetVertexNormalProjectedDistances(SceneBrep scene1, SceneBrep scene2)
		{
			if (scene1 == null || scene2 == null ||
				scene1.Vertices != scene2.Vertices)
			{
				return null;
			}

			List<Arrow> arrows = new List<Arrow>();

			for (int i = 0; i < scene1.Vertices; i++)
			{
				Vector3 vertex1 = scene1.GetVertex(i);
				Vector3 vertex2 = scene2.GetVertex(i);

				Vector3 distance = vertex2 - vertex1;
				Vector3 normal = scene1.GetNormal(i);

				// degenerated triangles in the scene do not have normals
				if (normal == Vector3.Zero)
				{
					arrows.Add(new Arrow(vertex1, Vector3.Zero, i, ArrowOrientation.Outwards));
					continue;
				}

				float normalDistanceAngle = Helpers.GetAngle(distance, normal);
				double projectedDistanceFactor = distance.Length * Math.Cos(normalDistanceAngle);

				Debug.Assert(!double.IsNaN(projectedDistanceFactor));

				Vector3 normalProjectedDistance = (float)projectedDistanceFactor * normal;

				// find out the orientation
				ArrowOrientation orientation = (normalDistanceAngle < Math.PI / 2 && normalDistanceAngle > -Math.PI / 2)
					? ArrowOrientation.Outwards
					: ArrowOrientation.Inwards;

				arrows.Add(new Arrow(vertex1, normalProjectedDistance, i, orientation));
			}

			return arrows;
		}
	}
}

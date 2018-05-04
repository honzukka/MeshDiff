// author: Jan Horesovsky

using System;
using OpenTK;
using Scene3D;
using System.Collections.Generic;
using System.Globalization;

namespace meshDiff
{
	class Helpers
	{
		/// <summary>
		/// Returns the angle between the two vectors in radians.
		/// </summary>
		public static float GetAngle(Vector3 vector1, Vector3 vector2)
		{
			double dotProduct = Vector3.Dot(vector1.Normalized(), vector2.Normalized());

			// account for a possible rounding error or insufficient precision
			if (dotProduct > 1)
			{
				dotProduct = 1;
			}
			else if (dotProduct < -1)
			{
				dotProduct = -1;
			}

			double angle = Math.Acos(dotProduct);

			return (float)angle;
		}

		/// <summary>
		/// Copies models into the scene.
		/// </summary>
		/// <returns>FALSE if models were null or the list was empty, TRUE otherwise</returns>
		public static bool CopyModelsInto(List<SceneBrep> models, ref SceneBrep scene, bool append = true)
		{
			if (models == null)
			{
				return false;
			}

			int j = 0;
			while (j < models.Count)
			{
				if (models[j] == null)
				{
					models.Remove(models[j]);
					continue;
				}

				j++;
			}

			if (models.Count == 0)
			{
				return false;
			}

			if (scene == null)
			{
				scene = new SceneBrep();
			}

			if (!append)
			{
				scene.Reset();
			}

			scene.Reserve(models.Count * models[0].Vertices);

			foreach (var model in models)
			{
				int sceneVertices = scene.Vertices;

				for (int i = 0; i < model.Vertices; i++)
				{
					scene.AddVertex(model.GetVertex(i));
					scene.SetColor(sceneVertices + i, model.GetColor(i));
				}

				for (int i = 0; i < model.Triangles; i++)
				{
					int v1, v2, v3;
					model.GetTriangleVertices(i, out v1, out v2, out v3);
					scene.AddTriangle(
						v1 + sceneVertices,
						v2 + sceneVertices,
						v3 + sceneVertices
					);
				}
			}

			scene.ComputeNormals();

			return true;
		}

		/// <exception cref="FormatException">Throw by number parser.</exception>
		/// <exception cref="ArgumentNullException">Thrown by number parser.</exception>
		/// <exception cref="OverflowException">Thrown by number parser.</exception>
		public static Matrix4 ParseMatrix4(string matrixString, char rowSeparator = ';', char numberSeparator = ',')
		{
			string[] vectorStrings = matrixString.Split(rowSeparator);
			Vector4[] matrixRows = new Vector4[4];

			for (int i = 0; i < vectorStrings.Length; i++)
			{
				string[] vectorValueStrings = vectorStrings[i].Split(numberSeparator);
				matrixRows[i] = new Vector4(
					float.Parse(vectorValueStrings[0], CultureInfo.InvariantCulture),
					float.Parse(vectorValueStrings[1], CultureInfo.InvariantCulture),
					float.Parse(vectorValueStrings[2], CultureInfo.InvariantCulture),
					float.Parse(vectorValueStrings[3], CultureInfo.InvariantCulture)
				);
			}

			return new Matrix4(matrixRows[0], matrixRows[1], matrixRows[2], matrixRows[3]);
		}

		public static string WriteMatrix4(Matrix4 matrix, char rowSeparator = ';', char numberSeparator = ',')
		{
			string row0 =
				matrix.Row0.X.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row0.Y.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row0.Z.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row0.W.ToString(CultureInfo.InvariantCulture);

			string row1 =
				matrix.Row1.X.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row1.Y.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row1.Z.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row1.W.ToString(CultureInfo.InvariantCulture);

			string row2 =
				matrix.Row2.X.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row2.Y.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row2.Z.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row2.W.ToString(CultureInfo.InvariantCulture);

			string row3 =
				matrix.Row3.X.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row3.Y.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row3.Z.ToString(CultureInfo.InvariantCulture) + numberSeparator +
				matrix.Row3.W.ToString(CultureInfo.InvariantCulture);

			return row0 + rowSeparator + row1 + rowSeparator + row2 + rowSeparator + row3;
		}

		/// <exception cref="FormatException">Thrown by number parser.</exception>
		/// <exception cref="ArgumentNullException">Thrown by number parser.</exception>
		/// <exception cref="OverflowException">Thrown by number parser.</exception>
		public static Vector3 ParseColor(string colorString, char valueSeparator = ',')
		{
			string[] colorValues = colorString.Split(valueSeparator);

			float red = float.Parse(colorValues[0], CultureInfo.InvariantCulture);
			float green = float.Parse(colorValues[1], CultureInfo.InvariantCulture);
			float blue = float.Parse(colorValues[2], CultureInfo.InvariantCulture);

			return new Vector3(red, green, blue);
		}

		public static string WriteColor(Vector3 color, char valueSeparator = ',')
		{
			return color.X.ToString(CultureInfo.InvariantCulture) + valueSeparator +
				color.Y.ToString(CultureInfo.InvariantCulture) + valueSeparator +
				color.Z.ToString(CultureInfo.InvariantCulture);
		}
	}
}

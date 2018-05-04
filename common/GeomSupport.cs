﻿// author: Josef Pelikan

using System;
using OpenTK;
using System.Collections.Generic;
using System.Globalization;

// Support math code.
namespace MathSupport
{
	/// <summary>
	/// Geometric support.
	/// </summary>
	public class Geometry
	{
		public static bool IsZero(double a)
		{
			return (a <= double.Epsilon && a >= -double.Epsilon);
		}

		public static void SpecularReflection(ref Vector3d normal, ref Vector3d input, out Vector3d output)
		{
			double k;
			Vector3d.Dot(ref normal, ref input, out k);
			output = (k + k) * normal - input;
		}

		public static Vector3d SpecularRefraction(Vector3d normal, double n, Vector3d input)
		{
			normal.Normalize();
			input.Normalize();
			double d = Vector3d.Dot(normal, input);

			if (d < 0.0)                        // (N*L) should be > 0.0 (N and L in the same half-space)
			{
				d = -d;
				normal = -normal;
			}
			else
				n = 1.0 / n;

			double cos2 = 1.0 - n * n * (1.0 - d * d);
			if (cos2 <= 0.0) return Vector3d.Zero; // total reflection

			d = n * d - Math.Sqrt(cos2);
			return (normal * d - input * n);
		}

		/// <summary>
		/// Finds two other axes to the given vector, their vector product will
		/// give the original vector, all three vectors will be perpendicular to each other.
		/// </summary>
		/// <param name="p">Original non-zero vector.</param>
		/// <param name="p1">First axis perpendicular to p.</param>
		/// <param name="p2">Second axis perpendicular to p.</param>
		public static void GetAxes(ref Vector3d p, out Vector3d p1, out Vector3d p2)
		{
			double ax = Math.Abs(p.X);
			double ay = Math.Abs(p.Y);
			double az = Math.Abs(p.Z);

			if (ax >= az &&
				 ay >= az)                       // ax, ay are dominant
			{
				p1.X = -p.Y;
				p1.Y = p.X;
				p1.Z = 0.0;
			}
			else
			if (ax >= ay &&
				 az >= ay)                       // ax, az are dominant
			{
				p1.X = -p.Z;
				p1.Y = 0.0;
				p1.Z = p.X;
			}
			else                                  // ay, az are dominant
			{
				p1.X = 0.0;
				p1.Y = -p.Z;
				p1.Z = p.Y;
			}

			Vector3d.Cross(ref p, ref p1, out p2);
		}

		/// <summary>
		/// Random direction around the [0,0,1] vector.
		/// Normal distribution is used, using required variance.
		/// </summary>
		/// <param name="rnd">Random generator to be used.</param>
		/// <param name="variance">Variance of the deviation angle in radians.</param>
		/// <returns></returns>
		public static Vector3d RandomDirectionNormal(RandomJames rnd, double variance)
		{
			// result: [0,0,1] * rotX( deviation ) * rotZ( orientation )

			double deviation = rnd.Normal(0.0, variance);
			double orientation = rnd.RandomDouble(0.0, Math.PI);
			Matrix4d mat = Matrix4d.CreateRotationX(deviation) * Matrix4d.CreateRotationZ(orientation);

			return new Vector3d(mat.Row2); // [0,0,1] * mat
		}

		/// <summary>
		/// Random direction around the givent center vector.
		/// Normal distribution is used, using required variance.
		/// </summary>
		/// <param name="rnd">Random generator to be used.</param>
		/// <param name="dir">Central direction.</param>
		/// <param name="variance">Variance of the deviation angle in radians.</param>
		/// <returns></returns>
		public static Vector3d RandomDirectionNormal(RandomJames rnd, Vector3d dir, double variance)
		{
			// Matrix3d fromz: [0,0,1] -> dir
			// Vector4d delta: [0,0,1] * rotX( deviation ) * rotZ( orientation )
			// result: delta * fromz

			dir.Normalize();
			Vector3d axis1, axis2;
			GetAxes(ref dir, out axis1, out axis2);
			Matrix4d fromz = new Matrix4d(new Vector4d(axis1), new Vector4d(axis2), new Vector4d(dir), Vector4d.UnitW);
			//fromz.Transpose();
			double deviation = rnd.Normal(0.0, variance);
			double orientation = rnd.RandomDouble(0.0, Math.PI);
			Matrix4d mat = Matrix4d.CreateRotationX(deviation) * Matrix4d.CreateRotationZ(orientation) * fromz;

			return new Vector3d(mat.Row2); // [0,0,1] * mat
		}

		/// <summary>
		/// Parse vector of float numbers delimited by semicolons/colons/commas optinally enclosed in [] or () parenthesis.
		/// Empty token is assumed to be zero (0.0f).
		/// </summary>
		/// <returns>Float array or null if failed</returns>
		public static float[] TryParseFloatVector(string str)
		{
			int len;
			if (str == null ||
				 (len = (str = str.Trim()).Length) == 0)
				return null;

			if ((str[0] == '(' &&
				  str[len - 1] == ')') ||
				 (str[0] == '[' &&
				  str[len - 1] == ']'))
				str = str.Substring(1, len - 2);

			string[] tokens;
			if (str.IndexOf(';') >= 0)
				tokens = str.Split(';');
			else
			  if (str.IndexOf(':') >= 0)
				tokens = str.Split(':');
			else
				tokens = str.Split(',');

			List<float> floats = new List<float>();
			float val;
			foreach (string token in tokens)
				if (token.Trim().Length == 0)
					floats.Add(0.0f);
				else
				  if (float.TryParse(token, NumberStyles.Number, CultureInfo.InvariantCulture, out val))
					floats.Add(val);

			return floats.ToArray();
		}

		/// <summary>
		/// Parse vector of double numbers delimited by semicolons/colons/commas optinally enclosed in [] or () parenthesis.
		/// Empty token is assumed to be zero (0.0).
		/// </summary>
		/// <returns>Float array or null if failed</returns>
		public static double[] TryParseDoubleVector(string str)
		{
			int len;
			if (str == null ||
				 (len = (str = str.Trim()).Length) == 0)
				return null;

			if ((str[0] == '(' &&
				  str[len - 1] == ')') ||
				 (str[0] == '[' &&
				  str[len - 1] == ']'))
				str = str.Substring(1, len - 2);

			string[] tokens;
			if (str.IndexOf(';') >= 0)
				tokens = str.Split(';');
			else
			  if (str.IndexOf(':') >= 0)
				tokens = str.Split(':');
			else
				tokens = str.Split(',');

			List<double> floats = new List<double>();
			double val;
			foreach (string token in tokens)
				if (token.Trim().Length == 0)
					floats.Add(0.0);
				else
				  if (double.TryParse(token, NumberStyles.Number, CultureInfo.InvariantCulture, out val))
					floats.Add(val);

			return floats.ToArray();
		}

		/// <summary>
		/// Parses Vector3 value from the dictionary.
		/// </summary>
		/// <returns>True if everything went well.</returns>
		public static bool TryParse(Dictionary<string, string> rec, string key, ref Vector3 val)
		{
			string sval;
			if (!rec.TryGetValue(key, out sval))
				return false;

			float[] vec = TryParseFloatVector(sval);
			if (vec == null ||
				 vec.Length < 3)
				return false;

			val.X = vec[0];
			val.Y = vec[1];
			val.Z = vec[2];
			return true;
		}

		/// <summary>
		/// Parses Vector2 value from the dictionary.
		/// </summary>
		/// <returns>True if everything went well.</returns>
		public static bool TryParse(Dictionary<string, string> rec, string key, ref Vector2 val)
		{
			string sval;
			if (!rec.TryGetValue(key, out sval))
				return false;

			float[] vec = TryParseFloatVector(sval);
			if (vec == null ||
				 vec.Length < 2)
				return false;

			val.X = vec[0];
			val.Y = vec[1];
			return true;
		}

		/// <summary>
		/// Parses Vector2d value from the dictionary.
		/// </summary>
		/// <returns>True if everything went well.</returns>
		public static bool TryParse(Dictionary<string, string> rec, string key, ref Vector2d val)
		{
			string sval;
			if (!rec.TryGetValue(key, out sval))
				return false;

			double[] vec = TryParseDoubleVector(sval);
			if (vec == null ||
				 vec.Length < 2)
				return false;

			val.X = vec[0];
			val.Y = vec[1];
			return true;
		}


		/// <summary>
		/// gluUnproject substitution.
		/// </summary>
		/// <param name="projection">Projection matrix.</param>
		/// <param name="view">View matrix.</param>
		/// <param name="width">Viewport width in pixels.</param>
		/// <param name="height">Viewport height in pixels.</param>
		/// <param name="x">Pointing 2D screen coordinate - x.</param>
		/// <param name="y">Pointing 2D screen coordinate - y.</param>
		/// <param name="z">Supplemental z-coord (0.0 for a near point, 1.0 for a far point).</param>
		/// <returns>Coordinate in the world-space.</returns>
		public static Vector3 UnProject(ref Matrix4 projection, ref Matrix4 view, int width, int height, float x, float y, float z = 0.0f)
		{
			Vector4 vec;
			vec.X = 2.0f * x / (float)width - 1;
			vec.Y = 2.0f * y / (float)height - 1;
			vec.Z = z;
			vec.W = 1.0f;

			Matrix4 viewInv = Matrix4.Invert(view);
			Matrix4 projInv = Matrix4.Invert(projection);

			Vector4.Transform(ref vec, ref projInv, out vec);
			Vector4.Transform(ref vec, ref viewInv, out vec);

			if (vec.W > float.Epsilon ||
				 vec.W < float.Epsilon)
			{
				vec.X /= vec.W;
				vec.Y /= vec.W;
				vec.Z /= vec.W;
				vec.W = 1.0f;
			}

			return new Vector3(vec);
		}

		/// <summary>
		/// Ray vs. AABB intersection, direction vector in regular form,
		/// box defined by lower-left corner and size.
		/// </summary>
		/// <param name="result">Parameter (t) bounds: [min, max].</param>
		/// <returns>True if intersections exist.</returns>
		public static bool RayBoxIntersection(ref Vector3d p0, ref Vector3d p1, ref Vector3d ul, ref Vector3d size, out Vector2d result)
		{
			result.X =
			result.Y = -1.0;
			double tMin = double.NegativeInfinity;
			double tMax = double.PositiveInfinity;
			double t1, t2, mul;

			// X axis:
			if (IsZero(p1.X))
			{
				if (p0.X <= ul.X ||
					 p0.X >= ul.X + size.X)
					return false;
			}
			else
			{
				mul = 1.0 / p1.X;
				t1 = (ul.X - p0.X) * mul;
				t2 = t1 + size.X * mul;

				if (mul > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax) return false;
			}

			// Y axis:
			if (IsZero(p1.Y))
			{
				if (p0.Y <= ul.Y ||
					 p0.Y >= ul.Y + size.Y)
					return false;
			}
			else
			{
				mul = 1.0 / p1.Y;
				t1 = (ul.Y - p0.Y) * mul;
				t2 = t1 + size.Y * mul;

				if (mul > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax) return false;
			}

			// Z axis:
			if (IsZero(p1.Z))
			{
				if (p0.Z <= ul.Z ||
					 p0.Z >= ul.Z + size.Z)
					return false;
			}
			else
			{
				mul = 1.0 / p1.Z;
				t1 = (ul.Z - p0.Z) * mul;
				t2 = t1 + size.Z * mul;

				if (mul > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax) return false;
			}

			result.X = tMin;
			result.Y = tMax;
			return true;
		}

		/// <summary>
		/// Ray vs. AABB intersection, direction vector in inverted form,
		/// box defined by lower-left corner and size.
		/// </summary>
		/// <param name="result">Parameter (t) bounds: [min, max].</param>
		/// <returns>True if intersections exist.</returns>
		public static bool RayBoxIntersectionInv(ref Vector3d p0, ref Vector3d p1inv, ref Vector3d ul, ref Vector3d size, out Vector2d result)
		{
			result.X =
			result.Y = -1.0;
			double tMin = double.NegativeInfinity;
			double tMax = double.PositiveInfinity;
			double t1, t2;

			// X axis:
			if (double.IsInfinity(p1inv.X))
			{
				if (p0.X <= ul.X ||
					 p0.X >= ul.X + size.X)
					return false;
			}
			else
			{
				t1 = (ul.X - p0.X) * p1inv.X;
				t2 = t1 + size.X * p1inv.X;

				if (p1inv.X > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax)
					return false;
			}

			// Y axis:
			if (double.IsInfinity(p1inv.Y))
			{
				if (p0.Y <= ul.Y ||
					 p0.Y >= ul.Y + size.Y)
					return false;
			}
			else
			{
				t1 = (ul.Y - p0.Y) * p1inv.Y;
				t2 = t1 + size.Y * p1inv.Y;

				if (p1inv.Y > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax)
					return false;
			}

			// Z axis:
			if (double.IsInfinity(p1inv.Z))
			{
				if (p0.Z <= ul.Z ||
					 p0.Z >= ul.Z + size.Z)
					return false;
			}
			else
			{
				t1 = (ul.Z - p0.Z) * p1inv.Z;
				t2 = t1 + size.Z * p1inv.Z;

				if (p1inv.Z > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax)
					return false;
			}

			result.X = tMin;
			result.Y = tMax;
			return true;
		}

		/// <summary>
		/// Ray vs. AABB intersection, direction vector in inverted form,
		/// box defined by lower-left corner and size.
		/// </summary>
		/// <param name="result">Parameter (t) bounds: [min, max].</param>
		/// <returns>True if intersections exist.</returns>
		public static bool RayBoxIntersectionInv(ref Vector3d p0, ref Vector3d p1inv, ref Vector3 ul, ref Vector3 size, out Vector2d result)
		{
			result.X =
			result.Y = -1.0;
			double tMin = double.NegativeInfinity;
			double tMax = double.PositiveInfinity;
			double t1, t2;

			// X axis:
			if (double.IsInfinity(p1inv.X))
			{
				if (p0.X <= ul.X ||
					 p0.X >= ul.X + size.X)
					return false;
			}
			else
			{
				t1 = (ul.X - p0.X) * p1inv.X;
				t2 = t1 + size.X * p1inv.X;

				if (p1inv.X > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax)
					return false;
			}

			// Y axis:
			if (double.IsInfinity(p1inv.Y))
			{
				if (p0.Y <= ul.Y ||
					 p0.Y >= ul.Y + size.Y)
					return false;
			}
			else
			{
				t1 = (ul.Y - p0.Y) * p1inv.Y;
				t2 = t1 + size.Y * p1inv.Y;

				if (p1inv.Y > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax)
					return false;
			}

			// Z axis:
			if (double.IsInfinity(p1inv.Z))
			{
				if (p0.Z <= ul.Z ||
					 p0.Z >= ul.Z + size.Z)
					return false;
			}
			else
			{
				t1 = (ul.Z - p0.Z) * p1inv.Z;
				t2 = t1 + size.Z * p1inv.Z;

				if (p1inv.Z > 0.0)
				{
					if (t1 > tMin) tMin = t1;
					if (t2 < tMax) tMax = t2;
				}
				else
				{
					if (t2 > tMin) tMin = t2;
					if (t1 < tMax) tMax = t1;
				}

				if (tMin > tMax)
					return false;
			}

			result.X = tMin;
			result.Y = tMax;
			return true;
		}

		/// <summary>
		/// Ray-triangle intersection test in 3D.
		/// According to Tomas Moller, <a href="http://www.acm.org/jgt/">JGT</a>.
		/// (origin + t * direction = (1 - u - v) * a + u * b + v * c)
		/// </summary>
		/// <param name="p0">Ray origin.</param>
		/// <param name="p1">Ray direction vector.</param>
		/// <param name="a">Vertex A of the triangle.</param>
		/// <param name="b">Vertex B of the triangle.</param>
		/// <param name="c">Vertex C of the triangle.</param>
		/// <param name="uv">Barycentric coordinates of the intersection.</param>
		/// <returns>Parametric coordinate on the ray if succeeded, double.NegativeInfinity otherwise.</returns>
		public static double RayTriangleIntersection(ref Vector3d p0, ref Vector3d p1,
													   ref Vector3d a, ref Vector3d b, ref Vector3d c,
													   out Vector2d uv)
		{
			Vector3d e1 = b - a;
			Vector3d e2 = c - a;
			Vector3d pvec;
			Vector3d.Cross(ref p1, ref e2, out pvec);
			double det;
			Vector3d.Dot(ref e1, ref pvec, out det);
			uv.X = uv.Y = 0.0;
			if (IsZero(det)) return double.NegativeInfinity;

			double detInv = 1.0 / det;
			Vector3d tvec = p0 - a;
			Vector3d.Dot(ref tvec, ref pvec, out uv.X);
			uv.X *= detInv;
			if (uv.X < 0.0 || uv.X > 1.0) return double.NegativeInfinity;

			Vector3d qvec;
			Vector3d.Cross(ref tvec, ref e1, out qvec);
			Vector3d.Dot(ref p1, ref qvec, out uv.Y);
			uv.Y *= detInv;
			if (uv.Y < 0.0 || uv.X + uv.Y > 1.0) return double.NegativeInfinity;

			Vector3d.Dot(ref e2, ref qvec, out det);
			return (detInv * det);
		}

		/// <summary>
		/// Ray-triangle intersection test in 3D.
		/// According to Tomas Moller, <a href="http://www.acm.org/jgt/">JGT</a>.
		/// (origin + t * direction = (1 - u - v) * a + u * b + v * c)
		/// </summary>
		/// <param name="p0">Ray origin.</param>
		/// <param name="p1">Ray direction vector.</param>
		/// <param name="a">Vertex A of the triangle.</param>
		/// <param name="b">Vertex B of the triangle.</param>
		/// <param name="c">Vertex C of the triangle.</param>
		/// <param name="uv">Barycentric coordinates of the intersection.</param>
		/// <returns>Parametric coordinate on the ray if succeeded, double.NegativeInfinity otherwise.</returns>
		public static double RayTriangleIntersection(ref Vector3d p0, ref Vector3d p1,
													   ref Vector3 a, ref Vector3 b, ref Vector3 c,
													   out Vector2d uv)
		{
			Vector3d e1 = (Vector3d)b - (Vector3d)a;
			Vector3d e2 = (Vector3d)c - (Vector3d)a;
			Vector3d pvec;
			Vector3d.Cross(ref p1, ref e2, out pvec);
			double det;
			Vector3d.Dot(ref e1, ref pvec, out det);
			uv.X = uv.Y = 0.0;
			if (IsZero(det)) return double.NegativeInfinity;

			double detInv = 1.0 / det;
			Vector3d tvec = p0 - (Vector3d)a;
			Vector3d.Dot(ref tvec, ref pvec, out uv.X);
			uv.X *= detInv;
			if (uv.X < 0.0 || uv.X > 1.0) return double.NegativeInfinity;

			Vector3d qvec;
			Vector3d.Cross(ref tvec, ref e1, out qvec);
			Vector3d.Dot(ref p1, ref qvec, out uv.Y);
			uv.Y *= detInv;
			if (uv.Y < 0.0 || uv.X + uv.Y > 1.0) return double.NegativeInfinity;

			Vector3d.Dot(ref e2, ref qvec, out det);
			return (detInv * det);
		}

		public static float DistanceSquared(Vector3 A, Vector3 B)
		{
			return (A - B).LengthSquared;
		}
	}
}

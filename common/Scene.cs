// author: Josef Pelikan, modified by Jan Horesovsky

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenTK;

namespace Scene3D
{
	using Edge = KeyValuePair<int, int>;

	/// <summary>
	/// B-rep 3D scene with associated corner-table (Jarek Rossignac).
	/// </summary>
	public partial class SceneBrep : ICloneable
	{
		#region Constants

		/// <summary>
		/// Invalid handle (for vertices, trinagles, corners..).
		/// </summary>
		public const int NULL = -1;

		#endregion

		#region Scene data

		/// <summary>
		/// Array of vertex coordinates (float[3]).
		/// </summary>
		public List<Vector3> geometry = null;

		/// <summary>
		/// Array of normal vectors (non mandatory).
		/// </summary>
		protected List<Vector3> normals = null;

		/// <summary>
		/// Array of vertex colors (non mandatory).
		/// </summary>
		protected List<Vector3> colors = null;

		/// <summary>
		/// Array of 2D texture coordinates (non mandatory).
		/// </summary>
		protected List<Vector2> txtCoords = null;

		/// <summary>
		/// Vertex pointer (handle) for each corner.
		/// </summary>
		public List<int> vertexPtr = null;

		/// <summary>
		/// Opposite corner pointer (handle) for each corner.
		/// Valid only for topological scene (triangles are connected).
		/// </summary>
		public List<int> oppositePtr = null;

		public int statEdges = 0;
		public int statShared = 0;

		public SortedSet<int> deleted = null;

		/// <summary>
		/// A list of corner handles for each vertex handle.
		/// </summary>
		private List<List<int>> vertexCorners = null;

		#endregion

		#region Construction

		public SceneBrep()
		{
			Reset();
		}

		#endregion

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public SceneBrep Clone()
		{
			SceneBrep tmp = new SceneBrep();
			tmp.geometry = new List<Vector3>(geometry);
			if (normals != null) tmp.normals = new List<Vector3>(normals);
			if (colors != null) tmp.colors = new List<Vector3>(colors);
			if (txtCoords != null) tmp.txtCoords = new List<Vector2>(txtCoords);
			if (vertexPtr != null) tmp.vertexPtr = new List<int>(vertexPtr);
			if (oppositePtr != null) tmp.oppositePtr = new List<int>(oppositePtr);
			if (vertexCorners != null) tmp.vertexCorners = new List<List<int>>(vertexCorners);
			tmp.BuildCornerTable();
			return tmp;
		}

		#region B-rep API

		/// <summary>
		/// [Re]initializes the scene data.
		/// </summary>
		public void Reset()
		{
			geometry = new List<Vector3>(256);
			normals = null;
			colors = null;
			txtCoords = null;
			vertexPtr = new List<int>(256);
			oppositePtr = null;
			deleted = null;
			vertexCorners = new List<List<int>>(256);
		}

		/// <summary>
		/// Reserve space for additional vertices.
		/// </summary>
		/// <param name="additionalVertices">Number of new vertices going to be inserted.</param>
		public void Reserve(int additionalVertices)
		{
			if (additionalVertices < 0)
				return;

			int newReserve = geometry.Count + additionalVertices;
			geometry.Capacity = newReserve;
			if (normals != null)
				normals.Capacity = newReserve;
			if (colors != null)
				colors.Capacity = newReserve;
			if (txtCoords != null)
				txtCoords.Capacity = newReserve;

			vertexCorners.Capacity = newReserve;

			newReserve = vertexPtr.Count + additionalVertices * 3;
			vertexPtr.Capacity = newReserve;
			if (oppositePtr != null)
				oppositePtr.Capacity = newReserve;
		}

		/// <summary>
		/// Current number of vertices in the scene.
		/// </summary>
		public int Vertices
		{
			get
			{
				return (geometry == null) ? 0 : geometry.Count;
			}
		}

		/// <summary>
		/// Current number of normal vectors in the scene (should be 0 or the same as Vertices).
		/// </summary>
		public int Normals
		{
			get
			{
				return (normals == null) ? 0 : normals.Count;
			}
		}

		public bool HasNormals()
		{
			return (normals != null);
		}

		public int NormalBytes()
		{
			return ((normals != null) ? 3 * sizeof(float) : 0);
		}

		/// <summary>
		/// Current number of vertex colors in the scene (should be 0 or the same as Vertices).
		/// </summary>
		public int Colors
		{
			get
			{
				return (colors == null) ? 0 : colors.Count;
			}
		}

		public bool HasColors()
		{
			return (colors != null);
		}

		public int ColorBytes()
		{
			return ((colors != null) ? 3 * sizeof(float) : 0);
		}

		/// <summary>
		/// Current number of texture coordinates in the scene (should be 0 or the same as Vertices).
		/// </summary>
		public int TxtCoords
		{
			get
			{
				return (txtCoords == null) ? 0 : txtCoords.Count;
			}
		}

		public bool HasTxtCoords()
		{
			return (txtCoords != null);
		}

		public int TxtCoordsBytes()
		{
			return ((txtCoords != null) ? 2 * sizeof(float) : 0);
		}

		/// <summary>
		/// Current number of triangles in the scene.
		/// </summary>
		public int Triangles
		{
			get
			{
				if (vertexPtr == null) return 0;
				Debug.Assert(vertexPtr.Count % 3 == 0, "Invalid V[] size");
				return vertexPtr.Count / 3;
			}
		}

		/// <summary>
		/// Current number of corners in the scene (# of triangles times three).
		/// </summary>
		public int Corners
		{
			get
			{
				return (vertexPtr == null) ? 0 : vertexPtr.Count;
			}
		}

		/// <summary>
		/// Add a new vertex defined by its 3D coordinate.
		/// </summary>
		/// <param name="v">Vertex coordinate in the object space</param>
		/// <returns>Vertex handle</returns>
		public int AddVertex(Vector3 v)
		{
			Debug.Assert(geometry != null);

			int last = geometry.Count;
			int handle = last;
			if (deleted != null &&
				 deleted.Count > 0)
			{
				handle = deleted.GetEnumerator().Current;
				geometry[handle] = v;
				vertexCorners[handle] = new List<int>();
				deleted.Remove(handle);
			}
			else
			{
				geometry.Add(v);
				vertexCorners.Add(new List<int>());
			}	

			if (normals != null &&
				 handle != last)
			{
				Debug.Assert(normals.Count == handle, "Invalid N[] size");
				normals.Add(Vector3.UnitY);
			}

			if (colors != null &&
				 handle != last)
			{
				Debug.Assert(colors.Count == handle, "Invalid C[] size");
				colors.Add(Vector3.One);
			}

			if (txtCoords != null &&
				 handle != last)
			{
				Debug.Assert(txtCoords.Count == handle, "Invalid T[] size");
				txtCoords.Add(Vector2.Zero);
			}

			return handle;
		}

		/// <summary>
		/// Delete the given vertex.
		/// Actually only marks the vertex as invalid (this handle can be reused in the future).
		/// </summary>
		/// <param name="v">Vertex handle to be deleted.</param>
		public void DeleteVertex(int v)
		{
			if (deleted == null)
				deleted = new SortedSet<int>();
			deleted.Add(v);
		}

		/// <summary>
		/// Free triangle triplet (set all handles to -1).
		/// </summary>
		/// <param name="tr">Triangle handle</param>
		public void FreeTriangle(int tr)
		{
			tr *= 3;
			vertexPtr[tr] = oppositePtr[tr] = -1;
			vertexPtr[tr + 1] = oppositePtr[tr + 1] = -1;
			vertexPtr[tr + 2] = oppositePtr[tr + 2] = -1;

			vertexCorners[vertexPtr[tr]].Remove(tr);
			vertexCorners[vertexPtr[tr + 1]].Remove(tr + 1);
			vertexCorners[vertexPtr[tr + 2]].Remove(tr + 2);
		}

		/// <summary>
		/// Returns object-space coordinates of the given vertex.
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <returns>Object-space coordinates</returns>
		public Vector3 GetVertex(int v)
		{
			Debug.Assert(geometry != null, "Invalid G[]");
			Debug.Assert(0 <= v && v < geometry.Count, "Invalid vertex handle");
			return geometry[v];
		}

		/// <summary>
		/// Returns corner handle of the given vertex.
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <returns>Object-space coordinates</returns>
		public int GetVertex(Vector3 v)
		{
			return geometry.IndexOf(v);
		}

		public void SetVertex(int v, Vector3 pos)
		{
			Debug.Assert(geometry != null, "Invalid G[]");
			Debug.Assert(0 <= v && v < geometry.Count, "Invalid vertex handle");
			geometry[v] = pos;
		}

		/// <summary>
		/// Assigns a normal vector to an existing vertex
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <param name="normal">New normal vector</param>
		public void SetNormal(int v, Vector3 normal)
		{
			Debug.Assert(geometry != null, "Invalid G[]");
			Debug.Assert(0 <= v && v < geometry.Count, "Invalid vertex handle");

			int len = Math.Max(geometry.Count, v + 1);

			if (normals == null)
				normals = new List<Vector3>(len);

			len -= normals.Count;
			while (len-- > 0)
				normals.Add(Vector3.UnitX);

			normals[v] = normal;
		}

		/// <summary>
		/// Returns normal vector of the given vertex.
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <returns>Normal vector</returns>
		public Vector3 GetNormal(int v)
		{
			Debug.Assert(normals != null, "Invalid N[]");
			Debug.Assert(0 <= v && v < normals.Count, "Invalid vertex handle");
			return normals[v];
		}

		/// <summary>
		/// Assigns a color to an existing vertex
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <param name="color">New vertex color</param>
		public void SetColor(int v, Vector3 color)
		{
			Debug.Assert(geometry != null, "Invalid G[]");
			Debug.Assert(0 <= v && v < geometry.Count, "Invalid vertex handle");

			int len = Math.Max(geometry.Count, v + 1);

			if (colors == null)
				colors = new List<Vector3>(len);

			len -= colors.Count;
			while (len-- > 0)
				colors.Add(Vector3.One);

			colors[v] = color;
		}

		/// <summary>
		/// Returns color of the given vertex.
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <returns>Vertex color</returns>
		public Vector3 GetColor(int v)
		{
			Debug.Assert(colors != null, "Invalid C[]");
			Debug.Assert(0 <= v && v < colors.Count, "Invalid vertex handle");
			return colors[v];
		}

		/// <summary>
		/// Assigns a texture coordinate to an existing vertex
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <param name="txt">New texture coordinate</param>
		public void SetTxtCoord(int v, Vector2 txt)
		{
			Debug.Assert(geometry != null, "Invalid G[]");
			Debug.Assert(0 <= v && v < geometry.Count, "Invalid vertex handle");

			int len = Math.Max(geometry.Count, v + 1);

			if (txtCoords == null)
				txtCoords = new List<Vector2>(len);

			len -= txtCoords.Count;
			while (len-- > 0)
				txtCoords.Add(Vector2.Zero);

			txtCoords[v] = txt;
		}

		/// <summary>
		/// Returns texture coordinate of the given vertex.
		/// </summary>
		/// <param name="v">Vertex handle</param>
		/// <returns>Texture coordinate</returns>
		public Vector2 GetTxtCoord(int v)
		{
			Debug.Assert(txtCoords != null, "Invalid T[]");
			Debug.Assert(0 <= v && v < txtCoords.Count, "Invalid vertex handle");
			return txtCoords[v];
		}

		/// <summary>
		/// Adds a new triangle face defined by its vertices.
		/// </summary>
		/// <param name="v1">Handle of the 1st vertex</param>
		/// <param name="v2">Handle of the 2nd vertex</param>
		/// <param name="v3">Handle of the 3rd vertex</param>
		/// <returns>Triangle handle</returns>
		public int AddTriangle(int v1, int v2, int v3)
		{
			// !!! TODO: recycle deleted triangles !!!

			Debug.Assert(geometry != null, "Invalid G[] size");
			Debug.Assert(geometry.Count > v1 &&
						  geometry.Count > v2 &&
						  geometry.Count > v3, "Invalid vertex handle");
			Debug.Assert(vertexPtr != null && (vertexPtr.Count % 3 == 0),
						  "Invalid corner-table (V[] size)");

			int handle1 = vertexPtr.Count;
			vertexPtr.Add(v1);
			vertexPtr.Add(v2);
			vertexPtr.Add(v3);

			vertexCorners[v1].Add(handle1);
			vertexCorners[v2].Add(handle1 + 1);
			vertexCorners[v3].Add(handle1 + 2);

			if (oppositePtr != null)
			{
				//Debug.Assert( oppositePtr.Count == handle1, "Invalid O[] size" );
				oppositePtr.Add(NULL);
				oppositePtr.Add(NULL);
				oppositePtr.Add(NULL);
			}

			return handle1 / 3;
		}

		/// <summary>
		/// Returns vertex handles of the given triangle.
		/// </summary>
		/// <param name="tr">Triangle handle</param>
		/// <param name="v1">Variable to receive the 1st vertex handle</param>
		/// <param name="v2">Variable to receive the 2nd vertex handle</param>
		/// <param name="v3">Variable to receive the 3rd vertex handle</param>
		public void GetTriangleVertices(int tr, out int v1, out int v2, out int v3)
		{
			Debug.Assert(geometry != null, "Invalid G[] size");
			tr *= 3;
			Debug.Assert(vertexPtr != null && 0 <= tr && tr + 2 < vertexPtr.Count,
						  "Invalid triangle handle");

			v1 = vertexPtr[tr];
			v2 = vertexPtr[tr + 1];
			v3 = vertexPtr[tr + 2];
		}

		/// <summary>
		/// Gets all triangles containing the given vertex
		/// </summary>
		/// <param name="v">Vertex handle.</param>
		/// <param name="border">True if there is no complete triangle ring.</param>
		/// <returns>List of triangle handles (positive = CCW oriented).</returns>
		public List<int> GetTrianglesFromVertex(int v, out bool border)
		{
			AssertCornerTable();
			List<int> result = new List<int>();      // result array (triangles in CCW order)
			HashSet<int> used = new HashSet<int>();  // already used triangels
			int i, current, resultOrigin;
			border = true;

			for (i = 0; i < vertexPtr.Count; i++)
				if (vertexPtr[i] == v &&
					 !used.Contains(i / 3))
				{
					resultOrigin = result.Count;

					// complete the sequence
					current = i;
					do
					{
						result.Add(current / 3);
						Debug.Assert(!used.Contains(current / 3));
						used.Add(current / 3);
						current = cNext(cLeft(current));
					}
					while (current != NULL &&
							current != i);

					if (current == i)
					{
						border = false;
						return result; // single sequence
					}

					// the opposite direction
					current = i;
					do
					{
						current = cPrev(cRight(current));
						if (current != NULL)
						{
							result.Insert(resultOrigin, current / 3);
							used.Add(current / 3);
						}
					}
					while (current != NULL);
				}

			return result;
		}

		/// <summary>
		/// Returns lists of incident triangles and neighbour vertices. Both in the positive (CCW) order.
		/// </summary>
		/// <param name="v">Central vertex hendle.</param>
		/// <param name="tri">List of triangle handles.</param>
		/// <param name="vert">List of vertex handles.</param>
		/// <returns>True if the lists are not complete rings (the given vertex lies on the mesh border).</returns>
		public bool GetTrianglesAndVerticesFromVertex(int v, out List<int> tri, out List<int> vert)
		{
			bool border;
			tri = GetTrianglesFromVertex(v, out border);
			int A, B, C;

			if (border)
			{
				// only collect the incident vertices unordered
				HashSet<int> vv = new HashSet<int>();
				foreach (var t in tri)
				{
					GetTriangleVertices(t, out A, out B, out C);
					if (A != v) vv.Add(A);
					if (B != v) vv.Add(B);
					if (C != v) vv.Add(C);
				}
				vert = new List<int>(vv);
			}
			else
			{
				// the full ring => compile the complete CCW list of vertices
				vert = new List<int>(tri.Count);

				// the 1st triangle:
				GetTriangleVertices(tri[0], out A, out B, out C);
				if (A != v) vert.Add(A);
				if (B != v) vert.Add(B);
				if (C != v) vert.Add(C);

				// the 2nd triangle
				GetTriangleVertices(tri[1], out A, out B, out C);
				if (A != v)
				{
					if (A == vert[0])
					{
						vert[0] = vert[1];
						vert[1] = A;
					}
					else
					  if (A != vert[1])
						vert.Add(A);
				}
				if (B != v)
				{
					if (B == vert[0])
					{
						vert[0] = vert[1];
						vert[1] = B;
					}
					else
					  if (B != vert[1])
						vert.Add(B);
				}
				if (C != v)
				{
					if (C == vert[0])
					{
						vert[0] = vert[1];
						vert[1] = C;
					}
					else
					  if (C != vert[1])
						vert.Add(C);
				}

				// the rest of triangles:
				for (int i = 2; i < tri.Count - 1; i++)
				{
					GetTriangleVertices(tri[i], out A, out B, out C);
					if (A != v && A != vert[i]) vert.Add(A);
					if (B != v && B != vert[i]) vert.Add(B);
					if (C != v && C != vert[i]) vert.Add(C);
				}
			}

			return border;
		}

		/// <summary>
		/// Changes vertices of an existing triangle.
		/// </summary>
		/// <param name="v1">New handle of the 1st vertex</param>
		/// <param name="v2">New handle of the 2nd vertex</param>
		/// <param name="v3">New handle of the 3rd vertex</param>
		/// <returns>Triangle handle</returns>
		public void SetTriangleVertices(int tr, int v1, int v2, int v3)
		{
			Debug.Assert(geometry != null, "Invalid G[] size");
			tr *= 3;
			Debug.Assert(vertexPtr != null && 0 <= tr && tr + 2 < vertexPtr.Count,
						  "Invalid triangle handle");
			Debug.Assert(geometry.Count > v1 &&
						  geometry.Count > v2 &&
						  geometry.Count > v3, "Invalid vertex handle");

			vertexCorners[vertexPtr[tr]].Remove(tr);
			vertexCorners[vertexPtr[tr + 1]].Remove(tr + 1);
			vertexCorners[vertexPtr[tr + 2]].Remove(tr + 2);

			vertexPtr[tr] = v1;
			vertexPtr[tr + 1] = v2;
			vertexPtr[tr + 2] = v3;

			vertexCorners[v1].Add(tr);
			vertexCorners[v2].Add(tr + 1);
			vertexCorners[v3].Add(tr + 2);
		}

		/// <summary>
		/// Returns vertex coordinates of the given triangle.
		/// </summary>
		/// <param name="tr">Triangle handle</param>
		/// <param name="v1">Variable to receive the 1st vertex coordinates</param>
		/// <param name="v2">Variable to receive the 2nd vertex coordinates</param>
		/// <param name="v3">Variable to receive the 3rd vertex coordinates</param>
		public void GetTriangleVertices(int tr, out Vector3 v1, out Vector3 v2, out Vector3 v3)
		{
			Debug.Assert(geometry != null, "Invalid G[] size");
			tr *= 3;
			Debug.Assert(vertexPtr != null && 0 <= tr && tr + 2 < vertexPtr.Count,
						  "Invalid triangle handle");

			int h1 = vertexPtr[tr];
			int h2 = vertexPtr[tr + 1];
			int h3 = vertexPtr[tr + 2];
			v1 = (h1 < 0 || h1 >= geometry.Count) ? Vector3.Zero : geometry[h1];
			v2 = (h2 < 0 || h2 >= geometry.Count) ? Vector3.Zero : geometry[h2];
			v3 = (h3 < 0 || h3 >= geometry.Count) ? Vector3.Zero : geometry[h3];
		}

		/// <summary>
		/// Returns vertex coordinates of the given triangle.
		/// </summary>
		/// <param name="tr">Triangle handle</param>
		/// <param name="v1">Variable to receive the 1st vertex coordinates</param>
		/// <param name="v2">Variable to receive the 2nd vertex coordinates</param>
		/// <param name="v3">Variable to receive the 3rd vertex coordinates</param>
		public void GetTriangleVertices(int tr, out Vector4 v1, out Vector4 v2, out Vector4 v3)
		{
			Debug.Assert(geometry != null, "Invalid G[] size");
			tr *= 3;
			Debug.Assert(vertexPtr != null && 0 <= tr && tr + 2 < vertexPtr.Count,
						  "Invalid triangle handle");

			int h1 = vertexPtr[tr];
			int h2 = vertexPtr[tr + 1];
			int h3 = vertexPtr[tr + 2];
			v1 = new Vector4((h1 < 0 || h1 >= geometry.Count) ? Vector3.Zero : geometry[h1], 1.0f);
			v2 = new Vector4((h2 < 0 || h2 >= geometry.Count) ? Vector3.Zero : geometry[h2], 1.0f);
			v3 = new Vector4((h3 < 0 || h3 >= geometry.Count) ? Vector3.Zero : geometry[h3], 1.0f);
		}

		/// <summary>
		/// Updates bounding box coordinates (AABB) for the given triangle.
		/// </summary>
		/// <param name="tr">Triangle handle</param>
		/// <param name="min">Minimum-vertex of the AABB</param>
		/// <param name="max">Maximum-vertex of the AABB</param>
		public void TriangleBoundingBox(int tr, ref Vector3 min, ref Vector3 max)
		{
			Vector3 a, b, c;
			GetTriangleVertices(tr, out a, out b, out c);

			if (a.X < min.X) min.X = a.X;
			if (a.X > max.X) max.X = a.X;
			if (a.Y < min.Y) min.Y = a.Y;
			if (a.Y > max.Y) max.Y = a.Y;
			if (a.Z < min.Z) min.Z = a.Z;
			if (a.Z > max.Z) max.Z = a.Z;

			if (b.X < min.X) min.X = b.X;
			if (b.X > max.X) max.X = b.X;
			if (b.Y < min.Y) min.Y = b.Y;
			if (b.Y > max.Y) max.Y = b.Y;
			if (b.Z < min.Z) min.Z = b.Z;
			if (b.Z > max.Z) max.Z = b.Z;

			if (c.X < min.X) min.X = c.X;
			if (c.X > max.X) max.X = c.X;
			if (c.Y < min.Y) min.Y = c.Y;
			if (c.Y > max.Y) max.Y = c.Y;
			if (c.Z < min.Z) min.Z = c.Z;
			if (c.Z > max.Z) max.Z = c.Z;
		}

		/// <summary>
		/// Computes vertex array size (VBO) in bytes.
		/// </summary>
		/// <param name="vertices">Use vertex coordinates?</param>
		/// <param name="txt">Use texture coordinates?</param>
		/// <param name="col">Use vertex colors?</param>
		/// <param name="norm">Use normal vectors?</param>
		/// <returns>Buffer size in bytes</returns>
		public int VertexBufferSize(bool vertices, bool txt, bool col, bool norm)
		{
			Debug.Assert(geometry != null, "Invalid G[]");

			int size = 0;
			if (vertices)
				size += Vertices * 3 * sizeof(float);
			if (txt && TxtCoords > 0)
				size += Vertices * 2 * sizeof(float);
			if (col && Colors > 0)
				size += Vertices * 3 * sizeof(float);
			if (norm && Normals > 0)
				size += Vertices * 3 * sizeof(float);

			return size;
		}

		/// <summary>
		/// Fill vertex data into the provided memory array (VBO after MapBuffer).
		/// </summary>
		/// <param name="ptr">Memory pointer</param>
		/// <param name="vertices">Use vertex coordinates?</param>
		/// <param name="txt">Use texture coordinates?</param>
		/// <param name="col">Use vertex colors?</param>
		/// <param name="norm">Use normal vectors?</param>
		/// <returns>Stride (vertex size) in bytes</returns>
		public unsafe int FillVertexBuffer(float* ptr, bool vertices, bool txt, bool col, bool norm)
		{
			if (geometry == null) return 0;

			if (txt && TxtCoords < Vertices)
				txt = false;

			if (col && Colors < Vertices)
				col = false;

			if (norm && Normals < Vertices)
				norm = false;

			int i;
			for (i = 0; i < Vertices; i++)
			{
				// GL_T2F_C3F_N3F_V3F

				if (txt)
				{
					*ptr++ = txtCoords[i].X;
					*ptr++ = txtCoords[i].Y;
				}
				if (col)
				{
					*ptr++ = colors[i].X;
					*ptr++ = colors[i].Y;
					*ptr++ = colors[i].Z;
				}
				if (norm)
				{
					*ptr++ = normals[i].X;
					*ptr++ = normals[i].Y;
					*ptr++ = normals[i].Z;
				}
				if (vertices)
				{
					*ptr++ = geometry[i].X;
					*ptr++ = geometry[i].Y;
					*ptr++ = geometry[i].Z;
				}
			}

			return sizeof(float) * ((txt ? 2 : 0) + (col ? 3 : 0) + (norm ? 3 : 0) + (vertices ? 3 : 0));
		}

		/// <summary>
		/// Fills index data into provided memory array (VBO after MapBuffer).
		/// </summary>
		/// <param name="ptr">Memory pointer</param>
		public unsafe void FillIndexBuffer(uint* ptr)
		{
			if (vertexPtr == null) return;

			foreach (int i in vertexPtr)
				*ptr++ = (uint)i;
		}

		/// <summary>
		/// Computes center point and diameter of the scene.
		/// </summary>
		/// <param name="center">Center point</param>
		/// <returns>Diameter</returns>
		public float GetDiameter(out Vector3 center)
		{
			if (Vertices < 2)
			{
				center = (Vertices == 1) ? GetVertex(0) : Vector3.Zero;
				return 4.0f;
			}

			// center of the object = point to look at:
			double cx = 0.0;
			double cy = 0.0;
			double cz = 0.0;
			float minx = float.MaxValue;
			float miny = float.MaxValue;
			float minz = float.MaxValue;
			float maxx = float.MinValue;
			float maxy = float.MinValue;
			float maxz = float.MinValue;
			int i;

			for (i = 0; i < Vertices; i++)
			{
				Vector3 vi = GetVertex(i);
				cx += vi.X;
				cy += vi.Y;
				cz += vi.Z;
				if (vi.X < minx) minx = vi.X;
				if (vi.Y < miny) miny = vi.Y;
				if (vi.Z < minz) minz = vi.Z;
				if (vi.X > maxx) maxx = vi.X;
				if (vi.Y > maxy) maxy = vi.Y;
				if (vi.Z > maxz) maxz = vi.Z;
			}
			center = new Vector3((float)(cx / Vertices),
								  (float)(cy / Vertices),
								  (float)(cz / Vertices));
			return (float)Math.Sqrt((maxx - minx) * (maxx - minx) +
									 (maxy - miny) * (maxy - miny) +
									 (maxz - minz) * (maxz - minz));
		}

		/// <summary>
		/// Only computes the diameter of the scene.
		/// </summary>
		/// <returns>Diameter</returns>
		public float GetDiameter()
		{
			return GetDiameter(out Vector3 temp);
		}

		/// <summary>
		/// Generate random vertex colors.
		/// </summary>
		/// <param name="seed">Random seed</param>
		public void GenerateColors(int seed)
		{
			Random rnd = new Random(seed);

			if (colors == null)
				colors = new List<Vector3>(geometry.Count);

			while (Colors < Vertices)
				colors.Add(new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
		}

		/// <summary>
		/// Sets all vertices to have the same color.
		/// </summary>
		/// <param name="newGlobalColor">Color vector</param>
		/// <param name="willOverride">If a scene already has colors, should they be overwritten or not?</param>
		public void SetGlobalColor(Vector3 newGlobalColor, bool overwrite)
		{
			if (overwrite || colors == null)
			{
				colors = new List<Vector3>(geometry.Count);
			}

			while (Colors < Vertices)
				colors.Add(newGlobalColor);
		}

		/// <summary>
		/// Recompute all normals by averaging incident triangles' normals.
		/// Corner table must be valid (BuildCornerTable()).
		/// </summary>
		public void ComputeNormals()
		{
			if (vertexPtr == null) return;

			normals = new List<Vector3>(new Vector3[geometry.Count]);
			int[] n = new int[geometry.Count];
			int ai, bi, ci;

			for (int i = 0; i < vertexPtr.Count; i += 3)               // process one triangle
			{
				Vector3 A = geometry[ai = cVertex(i)];
				Vector3 c = geometry[bi = cVertex(i + 1)] - A;
				Vector3 b = geometry[ci = cVertex(i + 2)] - A;

				A = Vector3.Cross(c, b);

				// degenerated triangles do not have normals
				if (A == Vector3.Zero)
				{
					continue;
				}

				A.Normalize();
		
				normals[ai] += A;
				n[ai]++;
				normals[bi] += A;
				n[bi]++;
				normals[ci] += A;
				n[ci]++;
			}

			// average the normals:
			for (int i = 0; i < geometry.Count; i++)
				if (n[i] > 0)
				{
					normals[i] /= n[i];
					normals[i].Normalize();

					/* strangely, this condition is sometimes true...
					if (normals[i].Length == 0)
					{
						Debugger.Break();
					}
					*/
				}
		}

		#endregion

		#region Corner-table API

		/// <summary>
		/// Simple test if there is a valid corner table.
		/// If not, rebuilds a fresh one.
		/// </summary>
		public void AssertCornerTable()
		{
			if (vertexPtr == null || vertexPtr.Count < 1 ||
				 oppositePtr == null ||
				 vertexPtr.Count == oppositePtr.Count)
				return;

			BuildCornerTable();
		}

		/// <summary>
		/// [Re]builds the mesh topology (corner-table should be consistent after this call).
		/// </summary>
		public void BuildCornerTable()
		{
			if (geometry == null || geometry.Count < 1 ||
				 vertexPtr == null || vertexPtr.Count < 1)
			{
				Reset();
				return;
			}

			// compact vertexPtr array:
			vertexPtr.RemoveAll(x => x == -1);

			int n = vertexPtr.Count;
			oppositePtr = new List<int>(n);
			for (int i = 0; i < n; i++)
				oppositePtr.Add(NULL);
			Dictionary<Edge, int> edges = new Dictionary<Edge, int>();

			statEdges = statShared = 0;
			for (int i = 0; i < n; i++)               // process one corner
			{
				int cmin = cVertex(cPrev(i));
				int cmax = cVertex(cNext(i));
				if (cmin < 0 || cmax < 0) continue;

				if (cmin > cmax)
				{
					int tmp = cmin;
					cmin = cmax;
					cmax = tmp;
				}
				Edge edge = new Edge(cmin, cmax);
				if (edges.ContainsKey(edge))
				{
					int other = edges[edge];
					Debug.Assert(oppositePtr[other] == NULL);
					oppositePtr[other] = i;
					oppositePtr[i] = other;
					edges.Remove(edge);
					statShared++;
				}
				else
				{
					edges.Add(edge, i);
					statEdges++;
				}
			}
		}

		/// <summary>
		/// Returns triangle handle of the given corner
		/// </summary>
		/// <param name="c">Corner handle</param>
		/// <returns>Triangle handle</returns>
		public static int cTriangle(int c)
		{
			return c / 3;
		}

		/// <summary>
		/// Returns handle of the 1st corner of the given triangle
		/// </summary>
		/// <param name="tr">Triangle handle</param>
		/// <returns>Corner handle</returns>
		public static int tCorner(int tr)
		{
			return tr * 3;
		}

		/// <summary>
		/// Returns the next corner inside the same triangle
		/// </summary>
		/// <param name="c">Corner handle</param>
		/// <returns>Handle of the next corner</returns>
		public static int cNext(int c)
		{
			if (c < 0) return NULL;
			return (c % 3 == 2) ? c - 2 : c + 1;
		}

		/// <summary>
		/// Returns the previous corner inside the same triangle
		/// </summary>
		/// <param name="c">Corner handle</param>
		/// <returns>Handle of the previous corner</returns>
		public static int cPrev(int c)
		{
			if (c < 0) return NULL;
			return (c % 3 == 0) ? c + 2 : c - 1;
		}

		/// <summary>
		/// Returns vertex handle of the given corner
		/// </summary>
		/// <param name="c">Corner handle</param>
		/// <returns>Associated vertex's handle</returns>
		public int cVertex(int c)
		{
			if (c < 0) return NULL;

			Debug.Assert(vertexPtr != null, "Invalid V[] array");
			Debug.Assert(c < vertexPtr.Count, "Invalid corner handle");

			return vertexPtr[c];
		}

		/// <summary>
		/// Returns opposite corner to the given corner
		/// </summary>
		/// <param name="c">Corner handle</param>
		/// <returns>Handle of the opposite corner</returns>
		public int cOpposite(int c)
		{
			if (c < 0) return NULL;

			Debug.Assert(oppositePtr != null, "Invalid O[] array");
			Debug.Assert(c < oppositePtr.Count, "Invalid corner handle");

			return oppositePtr[c];
		}

		/// <summary>
		/// Returns the "right" corner from the given corner
		/// </summary>
		/// <param name="c">Corner handle</param>
		/// <returns>Corner handle of the "right" triangle</returns>
		public int cRight(int c)
		{
			return cOpposite(cPrev(c));
		}

		/// <summary>
		/// Returns the "left" corner from the given corner
		/// </summary>
		/// <param name="c">Corner handle</param>
		/// <returns>Corner handle of the "left" triangle</returns>
		public int cLeft(int c)
		{
			return cOpposite(cNext(c));
		}

		/// <summary>
		/// Checks consistency of the Corner-table.
		/// Based on code of Karel Hrkal, 2014.
		/// </summary>
		/// <param name="errors">Optional output stream for detailed error messages</param>
		/// <param name="thorough">Do thorough checks? (might be too strict and too memory intensive in many cases)</param>
		/// <param name="dec">Optional Decoration object for detected errors.</param>
		/// <returns>Number of errors/inconsistencies (0 if everything is Ok)</returns>
		public int CheckCornerTable(StreamWriter errors, bool thorough = false, Decoration dec = null)
		{
			if (errors == null)
			{
				errors = new StreamWriter(Console.OpenStandardOutput());
				errors.AutoFlush = true;
				Console.SetOut(errors);
			}
			int errCount = 0;
			Action<string> log = (s) => { errCount++; errors.WriteLine(s); };
			int i, j, k;

			// 1. check trivial things such as in 1 triangle, all corners and all vertexes are disjont,
			//    cNext and cPevious, etc
			for (i = 0; i < Corners; i++)
			{
				int r1 = cNext(i);
				int r2 = cNext(r1);
				int r3 = cNext(r2);

				if (i == r1 || r1 == r2 || r2 == i || i != r3)
					log("cNext not working properly for corner " + i);

				if (i != cPrev(r1) || r1 != cPrev(r2) || r2 != cPrev(i))
					log("cPrev not working properly for corner " + i);

				int v0 = cVertex(i);
				int v1 = cVertex(r1);
				int v2 = cVertex(r2);
				if (v0 == v1 || v1 == v2 || v2 == v0)
					log("Duplicate vertex in triangle with corner " + i);
			}

			// 2. check corner <-> opposite validity
			for (i = 0; i < Corners; i++)
			{
				int other = cOpposite(i);
				if (other != NULL)
					if (cOpposite(other) != i)
						log("Corner " + i + " has an opposite " + other + " but not vice versa!");
					else
					{
						// while we are at it, check if 2 corners are linked as opposite
						// they also have same neighbour vertexes
						int a, b, c, d;
						a = cVertex(cNext(i));
						b = cVertex(cPrev(i));
						c = cVertex(cNext(other));
						d = cVertex(cPrev(other));
						bool correct = (a == d) && (b == c);
						bool semiCorrect = (a == c) && (b == d);

						if (!correct)
							if (semiCorrect)
								// this is the case where the triangles indeed have same neighbours,
								// but one is facing the other way than the other (and that is at least suspicious)
								//  makes sense only for one-sided faces, disable this otherwise
								log("Opposite corners " + i + " and " + other +
									 " have the same neighbour, but are facing opposite directions!");
							else
								log("Opposite corners " + i + " and " + other + " does not have the same neighbours!");
					}
			}

			if (thorough)
			{
				// 3. now let's check that the cRight works properly
				//    we will use cPrev(cOpposite(cPrev()))
				//    also, this whole test assumes that neighbour triangles are facing the same way
				//    if triangles have both faces (front and back) visible, this test doesn't make much sence
				int[] temp = new int[Triangles + 1]; // corners will be saved here

				for (i = 0; i < Corners; i++)
				{
					temp[0] = i;
					for (j = 0; j < Triangles; j++)
					{
						int right = cOpposite(cPrev(temp[j]));
						if (right != NULL)
						{
							right = cPrev(right);
							temp[j + 1] = right;
						}

						if (right == i || right == NULL)
						{
							// test vertex equality
							for (k = 0; k < j - 1; k++)
								if (cVertex(temp[k]) != cVertex(temp[k + 1]))
									log("Traversing right corners from " + i + " resolved into differrent vertices at " + temp[k]);

							break;
						}

						for (k = 0; k <= j; k++)
							if (temp[k] == right)
							{
								log("Starting in corner " + i + " we went right into corner " + right + " twice before returning to " + i);
								j = Triangles;
								break;
							}
					}
				}

				// 4. finally check if 2 triangles share the 2 vertices, then they have properly set opposite corners
				//    moreover, at most 2 triangles can share an edge (2-manifold)
				int[] vf = new int[Vertices];
				// vertex frequencies
				int[,] arr = new int[Vertices, Vertices];
				// for edge i<j, at position [i,j] is which corner is first found opposite corner to this edge
				// at position [j,i] is how many edges in triangles are there

				for (i = 0; i < Corners; i++)
				{
					vf[cVertex(i)]++;
					int a = cVertex(cNext(i));
					int b = cVertex(cPrev(i));
					// ensure a < b
					if (a > b)
					{
						int tmp = a;
						a = b;
						b = tmp;
					}

					if (arr[b, a] == 0)
					{
						// for the 1st time at this edge
						arr[a, b] = i;
						arr[b, a] = 1;
					}
					else
					if (arr[b, a] == 1)
					{
						// for the 2nd time at this edge
						if (cOpposite(i) != arr[a, b])
						{
							log("Corners " + i + " and " + arr[a, b] + " have the same opposite side, but are not linked together! [" + a + ',' + b + ']');
							if (dec != null)
								dec.AddEdge(a, b);
						}
						arr[b, a] = 2;
					}
					else
					// broken 2-manifold
					{
						log("Corner " + i + " has an opposide side thas was already used at least twice, 2-manifold is broken! [" + a + ',' + b + ']');
						if (dec != null)
							dec.AddEdge(a, b);
					}
				}

				// vf analysis:
				if (dec != null)
					for (i = 0; i < Vertices; i++)
						if (vf[i] > 0 && vf[i] < 3)
							dec.AddPoint(i);
			}

			return errCount;
		}

		#endregion

		#region Additional Functions for MeshDiff

		/// <summary>
		/// Returns the handles of all immediate neighbours of vertex v.
		/// </summary>
		public IEnumerable<int> GetVertexNeighbours(int vertexHandle)
		{
			HashSet<int> neighboursSet = new HashSet<int>();

			// all corner handles associated with vertexHandle
			List<int> cornerHandles = vertexCorners[vertexHandle];

			// get one other vertex for each corner's triangle to get all the neighbours of v
			foreach (var cornerHandle in cornerHandles)
			{
				neighboursSet.Add(vertexPtr[cPrev(cornerHandle)]);
				neighboursSet.Add(vertexPtr[cNext(cornerHandle)]);
			}

			return neighboursSet;
		}

		/// <summary>
		/// Returns an approximation of the density of the mesh around the given vertex.
		/// </summary>
		public float GetMeshResolution(int vertexHandle)
		{
			// all corner handles associated with vertexHandle
			List<int> cornerHandles = vertexCorners[vertexHandle];

			float totalEdgeLengths = 0f;
			foreach (var cornerHandle in cornerHandles)
			{
				Vector3 thisVertex = geometry[vertexHandle];
				Vector3 vertex1 = geometry[vertexPtr[cPrev(cornerHandle)]];
				Vector3 vertex2 = geometry[vertexPtr[cNext(cornerHandle)]];

				float edge1Length = (vertex1 - thisVertex).Length;
				float oppositeEdgeLength = (vertex2 - vertex1).Length;

				totalEdgeLengths += (edge1Length + oppositeEdgeLength);
			}

			float meanEdgeLength = totalEdgeLengths / (cornerHandles.Count * 2);

			if (meanEdgeLength == 0)
			{
				return 0;
			}

			return 1 / meanEdgeLength;
		}

		/// <summary>
		/// Returns handles to all pairs of vertices which form a triangle with a given vertex from the scene.
		/// </summary>
		public IEnumerable<Tuple<int, int>> GetPairsForTriangle(int vertexHandle)
		{
			List<Tuple<int, int>> trianglePairs = new List<Tuple<int, int>>();

			// all corner handles associated with vertexHandle
			List<int> cornerHandles = vertexCorners[vertexHandle];

			foreach (var cornerHandle in cornerHandles)
			{
				trianglePairs.Add(new Tuple<int, int>(
					vertexPtr[cPrev(cornerHandle)],
					vertexPtr[cNext(cornerHandle)]
				));
			}

			return trianglePairs;
		}
		/// <summary>
		/// Returns handles to all vertices which form a triangle with a given pair of vertices from the scene.
		/// </summary>
		public IEnumerable<int> GetVerticesForTriangle(int vertexHandle1, int vertexHandle2)
		{
			HashSet<int> neighbours1 = (HashSet<int>)GetVertexNeighbours(vertexHandle1);
			HashSet<int> neighbours2 = (HashSet<int>)GetVertexNeighbours(vertexHandle2);

			neighbours1.Remove(vertexHandle2);
			neighbours2.Remove(vertexHandle1);

			neighbours1.IntersectWith(neighbours2);

			return neighbours1;
		}

		#endregion

		#region Debugging

		public class Decoration
		{
			public HashSet<int> points = new HashSet<int>();

			public HashSet<Tuple<int, int>> edges = new HashSet<Tuple<int, int>>();

			public void AddPoint(int v)
			{
				points.Add(v);
			}

			public void AddEdge(int a, int b)
			{
				if (b > a)
				{
					int tmp = a;
					a = b;
					b = tmp;
				}
				edges.Add(new Tuple<int, int>(a, b));
			}

			public void Clear()
			{
				points.Clear();
				edges.Clear();
			}
		}

		#endregion
	}
}

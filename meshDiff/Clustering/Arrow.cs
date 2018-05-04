// author: Jan Horesovsky

using OpenTK;

namespace meshDiff
{
	public class Arrow
	{
		/// <summary>
		/// A point in space where the arrow begins.
		/// </summary>
		public Vector3 Origin { get; }

		/// <summary>
		/// The arrow vector.
		/// </summary>
		public Vector3 Direction { get; }

		/// <summary>
		/// If Origin is a vertex in a scene, this is its handle, otherwise it is -1.
		/// </summary>
		public int VertexHandle { get; }

		/// <summary>
		/// Assuming that "outwards" is where the normal is pointing.
		/// </summary>
		public ArrowOrientation Orientation { get; }

		/// <summary>
		/// Next to Direction, this is another way to express the significance of the Arrow.
		/// Is expected to be in [0,1]
		/// </summary>
		public float WidthRatio { get; set; }

		/// <summary>
		/// Next to Direction, this is another way to express the significance of the Arrow.
		/// Is expected to be in [0,1]
		/// </summary>
		public float HeightRatio { get; set; }

		/// <summary>
		/// This has a public setter because one Arrow can belong to many clusters.
		/// -1 means that no cluster is assigned.
		/// </summary>
		public int ClusterId { get; set; }

		/// <param name="origin">A point in space where the arrow begins.</param>
		/// <param name="direction">The arrow vector.</param>
		/// <param name="vertexHandle">If Origin is a vertex in a scene, this is its handle, otherwise it is -1.</param>
		/// <param name="orientation">TRUE - "outwards", FALSE - "inwards"</param>
		public Arrow(Vector3 origin, Vector3 direction, int vertexHandle, ArrowOrientation orientation)
		{
			Origin = origin;
			Direction = direction;
			VertexHandle = vertexHandle;
			Orientation = orientation;
			WidthRatio = 0;
			HeightRatio = 0;
			ClusterId = -1;
		}
	}
}

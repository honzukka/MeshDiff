// author: Jan Horesovsky

using System;

namespace meshDiff
{
	class MinHeap<T> : Heap<T> where T : IComparable<T>
	{
		/// <summary>
		/// Constructs a min heap.
		/// </summary>
		public MinHeap() : base(true)
		{

		}

		public T PeekMin()
		{
			if (elements.Count == 0)
				throw new InvalidOperationException("Heap is empty.");

			return elements[0];
		}

		public T ExtractMin()
		{
			if (elements.Count == 0)
				throw new InvalidOperationException("Heap is empty.");

			T min = elements[0];
			RemoveMin();

			return min;
		}

		public bool RemoveMin()
		{
			return RemoveRoot();
		}
	}
}

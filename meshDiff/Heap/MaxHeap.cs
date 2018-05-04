// author: Jan Horesovsky

using System;

namespace meshDiff
{
	class MaxHeap<T> : Heap<T> where T : IComparable<T>
	{
		/// <summary>
		/// Constructs a max heap.
		/// </summary>
		public MaxHeap() : base(false)
		{

		}

		public T PeekMax()
		{
			if (elements.Count == 0)
				throw new InvalidOperationException("Heap is empty.");

			return elements[0];
		}

		public T ExtractMax()
		{
			if (elements.Count == 0)
				throw new InvalidOperationException("Heap is empty.");

			T max = elements[0];
			RemoveMax();

			return max;
		}

		public bool RemoveMax()
		{
			return RemoveRoot();
		}
	}
}

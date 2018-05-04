// author: Jan Horesovsky

using System;
using System.Collections;
using System.Collections.Generic;

namespace meshDiff
{
	/// <summary>
	/// An abstract base for a heap which can be used for both a min heap and a max heap based on the argument in the constructor.
	/// </summary>
	abstract class Heap<T> : IEnumerable<T> where T : IComparable<T>
	{
		private delegate bool Comparer(T element1, T element2);

		protected List<T> elements;

		/// <summary>
		/// Tells whether element1 one belongs above element2 in the heap tree.
		/// </summary>
		private Comparer BelongsAbove;

		public int Count
		{
			get { return elements.Count; }
		}

		protected Heap(bool isMin)
		{
			elements = new List<T>();

			if (isMin)
			{
				BelongsAbove = (T element1, T element2) => element1.CompareTo(element2) < 0;
			}
			else
			{
				BelongsAbove = (T element1, T element2) => element1.CompareTo(element2) > 0;
			}
		}

		protected bool RemoveRoot()
		{
			if (elements.Count == 0)
				return false;

			if (elements.Count == 1)
			{
				elements.RemoveAt(0);
				return true;
			}

			// replace the root with the last element
			int lastIndex = elements.Count - 1;
			elements[0] = elements[lastIndex];
			elements.RemoveAt(lastIndex);

			// fix the heap
			bool placeFound = false;
			int currentIndex = 0;
			while (!placeFound)
			{
				int leftChildIndex = (currentIndex * 2) + 1;
				int rightChildIndex = (currentIndex * 2) + 2;
				int swapIndex = -1;

				// if this node can be swapped with the left child
				if (leftChildIndex < lastIndex && BelongsAbove(elements[leftChildIndex], elements[currentIndex]))
				{
					swapIndex = leftChildIndex;
				}

				// if this node can be swapped with the right child and the right child is smaller than the left child
				if (rightChildIndex < lastIndex && BelongsAbove(elements[rightChildIndex], elements[currentIndex]) &&
					BelongsAbove(elements[rightChildIndex], elements[leftChildIndex]))
				{
					swapIndex = rightChildIndex;
				}

				// no swap can be performed -> the remove operation is complete
				if (swapIndex == -1)
				{
					placeFound = true;
				}
				// otherwise perform the swap and continue
				else
				{
					T temp = elements[swapIndex];
					elements[swapIndex] = elements[currentIndex];
					elements[currentIndex] = temp;

					currentIndex = swapIndex;
				}
			}

			return true;
		}

		public void Insert(T newElement)
		{
			elements.Add(newElement);

			if (elements.Count == 1)
			{
				return;
			}

			int newElementIndex = elements.Count - 1;
			bool placeFound = false;
			while (!placeFound)
			{
				int parentIndex = (newElementIndex - 1) / 2;

				// if the new element isn't in the root yet and if it's parent is bigger
				if (newElementIndex > 0 && !BelongsAbove(elements[parentIndex], elements[newElementIndex]))
				{
					// perform the swap
					T temp = elements[parentIndex];
					elements[parentIndex] = elements[newElementIndex];
					elements[newElementIndex] = temp;

					newElementIndex = parentIndex;
				}
				else
				{
					// the insert operation is complete
					placeFound = true;
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)elements).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)elements).GetEnumerator();
		}
	}
}

using System;

namespace Dast
{
    // http://en.wikipedia.org/wiki/Heap_(data_structure)
    // http://en.wikipedia.org/wiki/Binary_heap

    public class BinaryMinHeap<T> : IHeap<T> where T : IComparable<T>
    {
        private readonly DynamicArray<T> _array = new DynamicArray<T>();
        
        /// <summary>
        ///     Returns the number of items in this collection.
        /// </summary>
        public int Count { get; private set; }
        
        /// <summary>
        ///     Returns the minimum item in the collection
        ///     O(1)
        /// </summary>
        /// <remarks>
        ///     I went with a method instead of a property because this can throw an exception if the collection is empty
        /// </remarks>
        /// <returns></returns>
        public T PeekMin()
        {
            return _array[0];
        }

        /// <summary>
        ///     Removes and returns the minimum item in the collection
        ///     O(log n)
        /// </summary>
        /// <returns></returns>
        public T RemoveMin()
        {
            var item = PeekMin();
            Swap(0, _array.Count - 1); // Replace the first item with the last one
            _array.RemoveAt(_array.Count - 1); // Remove the last one
            Count--; // Reduce the count
            BubbleDown(0); // Bubble the item down to its correct position
            return item;
        }

        /// <summary>
        ///     Adds an item to the collection
        ///     O(log n)
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _array.Add(item); // Add the item to the end
            Count++; // Increase our count
            BubbleUp(_array.Count - 1); // Bubble it up to its correct position
        }

        private void BubbleDown(int i)
        {
            // Move item at i down
            while (true)
            {
                var leftChildIndex = LeftChildIndexOf(i);
                var rightChildIndex = RightChildIndexOf(i);
                int smallestChildIndex;

                // If we don't have a right child
                if (rightChildIndex > Count - 1)
                {
                    // And don't have a left child
                    if (leftChildIndex > Count - 1)
                        // We are done?
                        return;
                    // And do have a left child
                    // The smallest child is the left child
                    smallestChildIndex = leftChildIndex;
                }
                // Else if we don't have a left child
                else if (leftChildIndex > Count - 1)
                    // The right child is the smallest
                    smallestChildIndex = rightChildIndex;
                // Else we have both children
                else
                    // Compare the children to get the smallest one
                    smallestChildIndex = (_array[leftChildIndex].CompareTo(_array[rightChildIndex]) < 0) ? leftChildIndex : rightChildIndex;
                // If the item we are moving is smaller than the smallest child, we are done
                if (_array[i].CompareTo(_array[smallestChildIndex]) < 0) return;
                // Else we swap it with the smallest child and keep going
                Swap(i, smallestChildIndex);
                i = smallestChildIndex;
            }
        }

        private void BubbleUp(int i)
        {
            while (true)
            {
                // If we are at the top, we are done
                if (i == 0) return;
                // Find the parent
                var parentIndex = ParentIndexOf(i);
                // If the parent is smaller than this item, we are done
                if (_array[parentIndex].CompareTo(_array[i]) <= -1)
                    return; // Done
                // Else, swap this item with the parent and keep going
                Swap(parentIndex, i);
                i = parentIndex;
            }
        }

        private void Swap(int x, int y)
        {
            var temp = _array[x];
            _array[x] = _array[y];
            _array[y] = temp;
        }

        private static int LeftChildIndexOf(int i)
        {
            return 2*i + 1;
        }

        private static int RightChildIndexOf(int i)
        {
            return 2*i + 2;
        }

        private static int ParentIndexOf(int i)
        {
            return (i - 1)/2;
        }
    }
}
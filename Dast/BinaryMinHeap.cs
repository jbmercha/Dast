using System;
using System.Threading;

namespace Dast
{
    // http://en.wikipedia.org/wiki/Heap_(data_structure)
    // http://en.wikipedia.org/wiki/Binary_heap

    public class BinaryMinHeap<T> : IHeap<T> where T : IComparable<T>
    {
        private readonly DynamicArray<T> _array = new DynamicArray<T>();
        public int Count { get; private set; }
        public T PeekMin()
        {
            return _array[0];
        }

        public T RemoveMin()
        {
            var item = PeekMin();
            Swap(0, _array.Count - 1);
            _array.RemoveAt(_array.Count - 1);
            Count--;
            BubbleDown(0);
            return item;
        }

        public void Add(T item)
        {
            _array.Add(item);
            BubbleUp(_array.Count - 1);
            Count++;
        }

        private void BubbleDown(int i)
        {
            while (true)
            {
                var l = LeftChildOf(i);
                var r = RightChildOf(i);
                int smallest;
                if (r > Count - 1)
                {
                    if (l > Count - 1)
                        return;
                    smallest = l;
                }
                else if (l > Count - 1)
                    smallest = r;
                else
                    smallest = (_array[l].CompareTo(_array[r]) < 0) ? l : r;
                if (_array[i].CompareTo(_array[smallest]) < 0) return;
                Swap(i, smallest);
                i = smallest;
            }
        }

        private void BubbleUp(int i)
        {
            while (true)
            {
                if (i == 0) return;
                var parent = ParentOf(i);
                if (_array[parent].CompareTo(_array[i]) <= -1)
                    return; // Done
                Swap(parent, i);
                i = parent;
            }
        }

        private void Swap(int x, int y)
        {
            var temp = _array[x];
            _array[x] = _array[y];
            _array[y] = temp;
        }

        private static int LeftChildOf(int i)
        {
            return 2*i + 1;
        }

        private static int RightChildOf(int i)
        {
            return 2*i + 2;
        }

        private static int ParentOf(int i)
        {
            return (i - 1)/2;
        }
    }
}
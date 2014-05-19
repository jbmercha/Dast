using System;
using System.Collections.Generic;

namespace Dast
{
    public class DynamicArray<T> : IDynamicArray<T>
    {
        private T[] _array = new T[4];

        /// <summary>
        ///     Returns the number of items in this collection
        ///     O(1)
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     Gets or sets the item at the specified index
        ///     O(1)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (index >= Count || index < 0) throw new IndexOutOfRangeException();
                return _array[index];
            }
            set
            {
                if (index >= Count || index < 0) throw new IndexOutOfRangeException();
                _array[index] = value;
            }
        }

        /// <summary>
        ///     Adds an item
        ///     O(1) * Amortized
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)

        {
            if (Count == _array.Length) GrowArray();
            _array[Count] = item;
            Count++;
        }

        /// <summary>
        ///     Removes an item
        ///     O(2n)
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            var index = IndexOf(item);
            if(index == -1)
                throw new Exception("Item not found");
            RemoveAt(index);
        }

        /// <summary>
        ///     Removes an item at the specified index.
        ///     O(n)
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            Array.Copy(_array, index + 1, _array, index, _array.Length - index - 1);
            Count--;
        }

        /// <summary>
        ///     Gets the index for the specified item
        ///     O(n)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(_array[i], item)) return i;
            }
            return -1;
        }

        private void GrowArray()
        {
            var newArray = new T[_array.Length*2];
            Array.Copy(_array, 0, newArray, 0, _array.Length);
            _array = newArray;
        }
    }
}
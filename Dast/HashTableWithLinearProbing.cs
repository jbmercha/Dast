using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Dast
{
    public class HashTableWithLinearProbing<TKey, TValue> : IHashTable<TKey, TValue>
    {
        [DebuggerDisplay("{Full ? \"Key = \" + Key.ToString() + \" Value = \" + Value.ToString() : Gap ? \"Gap\" : \"Empty\"}")]
        private struct HashEntry
        {
            public int HashCode;
            public bool Full;
            public bool Gap;
            public TKey Key;
            public TValue Value;
        }
        private HashEntry[] _array;
        private int _capacity;
        private readonly EqualityComparer<TKey> _comparer;

        public HashTableWithLinearProbing()
        {
            Count = 0;
            _capacity = 4;
            _array = new HashEntry[_capacity];
            _comparer = EqualityComparer<TKey>.Default;
        }

        /// <summary>
        ///     Returns the number of items in this collection
        ///     O(1)
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     Get will return the element in the table.
        ///     O(1*)
        ///     Set will add or update the element in the table.
        ///     O(1*)
        /// </summary>
        /// <param name="key">The key of the element</param>
        /// <returns>The element found by the key</returns>
        public TValue this[TKey key]
        {
            get {
                return GetItem(key);
            }
            set {
                InsertItem(key, value);
            }
        }

        /// <summary>
        ///     Removes an item from the table base off the key.
        ///     O(1*)
        /// </summary>
        /// <param name="key">The key used to locate the item in the table</param>
        public void Remove(TKey key)
        {
            var i = GetIndexOfItem(key);
            _array[i] = new HashEntry();
            Count--;
            var nextItem = _array[(i + 1)%_capacity];
            if (nextItem.Full || nextItem.Gap)
                _array[i].Gap = true;
        }

        private TValue GetItem(TKey key)
        {
            return _array[GetIndexOfItem(key)].Value;
        }

        private int GetIndexOfItem(TKey key)
        {
            var hashCode = HashCode(key);
            var initialIndex = hashCode % _capacity;
            var i = initialIndex;
            do
            {
                var item = _array[i];
                if (!item.Full && !item.Gap)
                    throw new KeyNotFoundException();
                if (hashCode == item.HashCode && _comparer.Equals(item.Key, key))
                    return i;
                i = (i + 1) % _capacity;
            } while (i != initialIndex);
            throw new KeyNotFoundException();
        }

        private void InsertItem(TKey key, TValue value)
        {
            var hashCode = HashCode(key);
            var initialIndex = hashCode % _capacity;
            var i = initialIndex;
            while (true)
            {
                var item = _array[i];
                if (!item.Full)
                {
                    _array[i] = new HashEntry { HashCode = hashCode, Key = key, Value = value, Full = true };
                    Count++;
                    if (Count * 2 >= _capacity) GrowArray();
                    return;
                }
                if (hashCode == item.HashCode && _comparer.Equals(item.Key, key))
                {
                    _array[i].Value = value;
                    return;
                }
                i = (i + 1) % _capacity;
            }
        }

        private int HashCode(TKey key)
        {
            return _comparer.GetHashCode(key);
        }

        private void GrowArray()
        {
            _capacity *= 2;
            var oldArray = _array;
            _array = new HashEntry[_capacity];
            foreach (var item in oldArray.Where(item => item.Full))
            {
                InsertItemDangerous(item);
            }
        }

        private void InsertItemDangerous(HashEntry itemToInsert)
        {
            var initialIndex = itemToInsert.HashCode % _capacity;
            var i = initialIndex;
            while (true)
            {
                var item = _array[i];
                if (!item.Full)
                {
                    _array[i] = itemToInsert;
                    return;
                }
                i = (i + 1) % _capacity;
            }
        }
    }
}
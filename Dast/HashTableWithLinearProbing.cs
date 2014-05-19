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
        private readonly EqualityComparer<TKey> _comparer;

        public HashTableWithLinearProbing()
        {
            Count = 0;
            Capacity = 4;
            _array = new HashEntry[Capacity];
            _comparer = EqualityComparer<TKey>.Default;
        }

        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public TValue this[TKey key]
        {
            get {
                return GetItem(key);
            }
            set {
                InsertItem(key, value);
            }
        }

        private TValue GetItem(TKey key)
        {
            var hashCode = HashCode(key);
            var initialIndex = hashCode%Capacity;
            var i = initialIndex;
            do
            {
                var item = _array[i];
                if (!item.Full && !item.Gap)
                    throw new KeyNotFoundException();
                if (hashCode == item.HashCode && _comparer.Equals(item.Key, key))
                    return item.Value;
                i = (i + 1)%Capacity;
            } while (i != initialIndex);
            throw new KeyNotFoundException();
        }

        private void InsertItem(TKey key, TValue value)
        {
            var hashCode = HashCode(key);
            var initialIndex = hashCode % Capacity;
            var i = initialIndex;
            while (true)
            {
                var item = _array[i];
                if (!item.Full)
                {
                    _array[i] = new HashEntry { HashCode = hashCode, Key = key, Value = value, Full = true };
                    Count++;
                    if (Count * 2 >= Capacity) GrowArray();
                    return;
                }
                if (hashCode == item.HashCode && _comparer.Equals(item.Key, key))
                {
                    _array[i].Value = value;
                    return;
                }
                i = (i + 1) % Capacity;
            }
        }

        public void Remove(TKey key)
        {
            var hashCode = HashCode(key);
            var initialIndex = hashCode % Capacity;
            var i = initialIndex;
            do
            {
                var item = _array[i];
                if (hashCode == item.HashCode && _comparer.Equals(item.Key, key))
                {
                    _array[i] = new HashEntry();
                    Count--;
                    if (_array[(i + 1)%Capacity].Full || _array[(i + 1)%Capacity].Gap)
                        _array[i].Gap = true;
                    return;
                }
                if (!item.Full && !item.Gap)
                    throw new KeyNotFoundException();
                i = (i + 1) % Capacity;
            } while (i != initialIndex);
            throw new KeyNotFoundException();
        }

        private int HashCode(TKey key)
        {
            return _comparer.GetHashCode(key);
        }

        private void GrowArray()
        {
            Capacity *= 2;
            var oldArray = _array;
            _array = new HashEntry[Capacity];
            foreach (var item in oldArray.Where(item => item.Full))
            {
                InsertItemDangerous(item);
            }
        }

        private void InsertItemDangerous(HashEntry itemToInsert)
        {
            var initialIndex = itemToInsert.HashCode % Capacity;
            var i = initialIndex;
            while (true)
            {
                var item = _array[i];
                if (!item.Full)
                {
                    _array[i] = itemToInsert;
                    return;
                }
                i = (i + 1) % Capacity;
            }
        }

    }
}
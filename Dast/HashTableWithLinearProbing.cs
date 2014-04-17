using System;
using System.Collections.Generic;

namespace Dast
{
    public class HashTableWithLinearProbing<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private struct HashEntry
        {
            public TKey Key;
            public TValue Value;
            public bool Full;
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
        public int Capacity { get; set; }
        public TValue this[TKey key]
        {
            get
            {
                var initialIndex = HashKey(key);
                var i = initialIndex;
                do
                {
                    var item = _array[i];
                    if(!item.Full)
                        throw new KeyNotFoundException();
                    if (_comparer.Equals(item.Key, key)) 
                        return item.Value;
                    i = (i + 1) % Capacity;
                } while (i != initialIndex);
                throw new KeyNotFoundException();
            }
            set
            {
                while (true)
                {
                    var initialIndex = HashKey(key);
                    var i = initialIndex;
                    do
                    {
                        var item = _array[i];
                        if (!item.Full)
                        {
                            _array[i] = new HashEntry {Key = key, Value = value, Full = true};
                            Count++;
                            if(Count*2 >= Capacity / 3) GrowArray();
                            return;
                        }
                        if (_comparer.Equals(item.Key, key))
                        {
                            _array[i] = new HashEntry { Key = key, Value = value, Full = true };
                            return;
                        }
                        i = (i + 1)%Capacity;
                    } while (i != initialIndex);
                    GrowArray();
                }
            }
        }

        public void Remove(TKey key)
        {
            throw new NotImplementedException();
            // This could leave GAPS, so the probing won't find the right item anymore!!!!
            var initialIndex = HashKey(key);
            var i = initialIndex;
            do
            {
                var item = _array[i];
                if (_comparer.Equals(item.Key, key))
                {
                    _array[i] = new HashEntry();
                    Count--;
                    return;
                }
                if (_comparer.Equals(item.Key, default(TKey)))
                    throw new KeyNotFoundException();
                i = (i + 1) % Capacity;
            } while (i != initialIndex);
            throw new KeyNotFoundException();
        }

        private int HashKey(TKey key)
        {
            var code = _comparer.GetHashCode(key);
            return code & (Capacity-1); // Capacity is a power of 2
        }

        private void GrowArray()
        {
            Count = 0;
            Capacity *= 2;
            var oldArray = _array;
            _array = new HashEntry[Capacity];
            foreach (var item in oldArray)
            {
                if (!_comparer.Equals(item.Key, default(TKey)))
                    this[item.Key] = item.Value;
            }
        }

    }
}
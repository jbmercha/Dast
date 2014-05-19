using System;
using System.Collections.Generic;
using System.Linq;

namespace Dast
{
    public class HashTableWithChaining<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private class HashEntry
        {
            public TKey Key;
            public TValue Value;
        }
        private readonly EqualityComparer<TKey> _comparer;

        private List<HashEntry>[] _array;
        private int _longestChain;

        public HashTableWithChaining()
        {
            Count = 0;
            Capacity = 4;
            _comparer = EqualityComparer<TKey>.Default;
            _array = new List<HashEntry>[Capacity];
        }

        public int Capacity { get; private set; }

        public int Count { get; private set; }

        public TValue this[TKey key]
        {
            get
            {
                var index = HashKey(key);
                if(_array[index] == null)
                    throw new KeyNotFoundException();

                foreach (var item in _array[index].Where(item => _comparer.Equals(item.Key, key)))
                {
                    return item.Value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                var index = HashKey(key);
                if (_array[index] == null)
                    _array[index] = new List<HashEntry>();

                _array[index].Add(new HashEntry {Key = key, Value = value});
                Count++;
                _longestChain = Math.Max(_longestChain, _array[index].Count);
                if (_longestChain > Capacity)
                    GrowArray();
            }
        }

        private void GrowArray()
        {
            Count = 0;
            Capacity *= 2;
            var oldArray = _array;
            _array = new List<HashEntry>[Capacity];
            foreach (var item in oldArray.Where(x => x != null).SelectMany(x => x))
            {
                this[item.Key] = item.Value;
            }
        }


        public int HashKey(TKey key)
        {
            var code = _comparer.GetHashCode(key);
            return code & (Capacity - 1); // Capacity is a power of 2 so this is like code % Capacity
        }
           

        public void Remove(TKey key)
        {
            var index = HashKey(key);
            if (_array[index] == null)
                throw new KeyNotFoundException();

            for(var i = 0; i < _array[index].Count; i++)
            {
                if (!_comparer.Equals(_array[index][i].Key, key)) continue;
                _array[index].RemoveAt(i);
                Count--;
                if(_array[index].Count == _longestChain - 1)
                    _longestChain = _array.Where(x => x != null).Max(x => x.Count);
                return;
            }
            throw new KeyNotFoundException();
        }
    }
}

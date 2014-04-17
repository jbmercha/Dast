using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dast
{
    public class HashTableWithChaining<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private struct HashEntry
        {
            public TKey Key;
            public List<TValue> ValueList;
        }
        private EqualityComparer<TKey> _comparer;

        private HashEntry[] _array;

        public HashTableWithChaining()
        {
            Count = 0;
            Capacity = 32;
            _comparer = EqualityComparer<TKey>.Default;
            _array = new HashEntry[Capacity];
        }

        public int Capacity { get; private set; }

        public int Count { get; private set; }

        public TValue this[TKey key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public int HashKey(TKey key)
        {
            return _comparer.GetHashCode(key) % Capacity;
        }
           

        public void Remove(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}

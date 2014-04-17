using System;
using System.Collections.Generic;

namespace Dast
{
    public class HashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private KeyValuePair<TKey, TValue>[] _array;

        public HashTable()
        {
            Count = 0;
            Capacity = 4;
            _array = new KeyValuePair<TKey, TValue>[Capacity];
        }

        public int Count { get; private set; }
        public int Capacity { get; set; }
        public TValue this[TKey key]
        {
            get
            {
                if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                    throw new Exception("Invalid key.");
                var initialIndex = GetIndexForKey(key);
                var i = initialIndex;
                do
                {
                    var item = _array[i];
                    if (EqualityComparer<TKey>.Default.Equals(item.Key , key)) 
                        return item.Value;
                    if (EqualityComparer<TKey>.Default.Equals(item.Key, default(TKey)))
                        throw new KeyNotFoundException();
                    i = (i + 1) % Capacity;
                } while (i != initialIndex);
                throw new KeyNotFoundException();
            }
            set
            {
                var d = default(TKey);
                if (EqualityComparer<TKey>.Default.Equals(key, d))
                    throw new Exception("Invalid key.");
                while (true)
                {
                    var initialIndex = GetIndexForKey(key);
                    var i = initialIndex;
                    do
                    {
                        var item = _array[i];
                        if (EqualityComparer<TKey>.Default.Equals(item.Key, d))
                        {
                            _array[i] = new KeyValuePair<TKey, TValue>(key, value);
                            Count++;
                            if(Count >= Capacity / 2) GrowArray();
                            return;
                        }
                        if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                        {
                            _array[i] = new KeyValuePair<TKey, TValue>(key, value);
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
            var initialIndex = GetIndexForKey(key);
            var i = initialIndex;
            do
            {
                var item = _array[i];
                if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                {
                    _array[i] = new KeyValuePair<TKey, TValue>();
                    Count--;
                    return;
                }
                if (EqualityComparer<TKey>.Default.Equals(item.Key, default(TKey)))
                    throw new KeyNotFoundException();
                i = (i + 1) % Capacity;
            } while (i != initialIndex);
            throw new KeyNotFoundException();
        }


        private int GetIndexForKey(TKey key)
        {
            return key.GetHashCode() % Capacity;
        }

        private void GrowArray()
        {
            Count = 0;
            Capacity *= 2;
            var oldArray = _array;
            _array = new KeyValuePair<TKey, TValue>[Capacity];
            foreach (var item in oldArray)
            {
                if (!EqualityComparer<TKey>.Default.Equals(item.Key, default(TKey)))
                    this[item.Key] = item.Value;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dast
{

    public interface IQueue<T>
    {
        int Count { get; }
        void Enqueue(T item);
        T Dequeue();
    }

    public class CircularBufferQueue<T> : IQueue<T>
    {
        private T[] _array;
        private int _capacity;
        private int _start;
        private int _end;
        public CircularBufferQueue()
        {
            _capacity = 4;
            _array = new T[_capacity];
            _start = 0;
            _end = 0;
        }

        public int Count { get; private set; }
        
        public void Enqueue(T item)
        {
            if (Count == _capacity)
                GrowArray();

            _array[_end] = item;
            _end = (_end + 1) % _capacity;
            Count++;
        }

        public T Dequeue()
        {
            if(Count == 0)
                throw new Exception("Queue is empty. Cannot Dequeue");
            var item = _array[_start];
            _array[_start] = default (T);
            _start = (_start + 1) % _capacity;
            Count--;
            return item;
        }

        private void GrowArray()
        {
            _capacity *= 2;
            var newArray = new T[_capacity];
            
            Array.Copy(_array, _start, newArray, 0, _array.Length - _start);
            Array.Copy(_array, 0, newArray, _array.Length - _start, _start);
            _end = Count;
            _start = 0;
            _array = newArray;
        }
    }
}

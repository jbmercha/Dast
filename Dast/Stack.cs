using System;

namespace Dast
{

    public interface IStack<T>
    {
        int Count { get; }
        void Push(T item);
        T Pop();
    }


    public class ArrayStack<T> : IStack<T>
    {
        private T[] _array;
        private int _capacity;

        public ArrayStack()
        {
            _capacity = 4;
            _array = new T[_capacity];
        }

        public int Count { get; private set; }
        public void Push(T item)
        {
            if (Count == _capacity)
                GrowArray();
            _array[Count] = item;
            Count++;
        }

        private void GrowArray()
        {
            _capacity *= 2;
            var newArray = new T[_capacity];
            Array.Copy(_array, 0, newArray, 0, _array.Length);
            _array = newArray;
        }

        public T Pop()
        {
            if(Count == 0)
                throw new Exception("Stack is empty");
            Count--;
            var item = _array[Count];
            _array[Count] = default (T);
            return item;
        }
    }
}

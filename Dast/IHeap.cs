using System;

namespace Dast
{
    public interface IHeap<T> where T : IComparable<T>
    {
        int Count { get; }
        T PeekMin();
        T RemoveMin();
        void Add(T item);
    }
}
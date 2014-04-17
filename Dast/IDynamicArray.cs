namespace Dast
{
    public interface IDynamicArray<T>
    {
        int Count { get; }
        T this[int index] { get; set; }
        void Add(T item);
        void Remove(T item);
        void RemoveAt(int index);
        int IndexOf(T item);
    }
}
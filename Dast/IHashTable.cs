namespace Dast
{
    public interface IHashTable<in TKey, TValue>
    {
        int Count { get; }
        TValue this[TKey key] { get; set; }
        void Remove(TKey key);
    }
}

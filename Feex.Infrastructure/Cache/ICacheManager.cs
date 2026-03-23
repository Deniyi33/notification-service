
namespace Feex.Infrastructure.Cache
{
    public interface ICacheManager<TKey, TValue>
    {
        void AddOrUpdate(TKey key, TValue value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null);
        void Clear();
        void Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }
}
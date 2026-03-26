using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feex.Infrastructure.Cache
{
    public class CacheItem<T>
    {
        public T Value { get; set; }
        public DateTime AbsoluteExpiration { get; set; }
        public Timer SlidingExpirationTimer { get; set; }
        public TimeSpan? SlidingExpiration { get; set; } // Store the sliding expiration duration
    }


    public class CacheManager<TKey, TValue> : ICacheManager<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, CacheItem<TValue>> _cache;

        public CacheManager()
        {
            _cache = new ConcurrentDictionary<TKey, CacheItem<TValue>>();
        }

        public void AddOrUpdate(TKey key, TValue value, TimeSpan? slidingExpiration = null, TimeSpan? absoluteExpiration = null)
        {
            var cacheItem = new CacheItem<TValue>
            {
                Value = value,
                AbsoluteExpiration = absoluteExpiration.HasValue
                    ? DateTime.UtcNow.Add(absoluteExpiration.Value)
                    : DateTime.MaxValue,
                SlidingExpiration = slidingExpiration
            };

            if (slidingExpiration.HasValue)
            {
                cacheItem.SlidingExpirationTimer = new Timer(
                    _ => Remove(key),
                    null,
                    slidingExpiration.Value,
                    Timeout.InfiniteTimeSpan);
            }

            _cache.AddOrUpdate(key, cacheItem, (k, existingItem) =>
            {
                existingItem.SlidingExpirationTimer?.Dispose();
                existingItem.Value = value;
                existingItem.AbsoluteExpiration = cacheItem.AbsoluteExpiration;
                existingItem.SlidingExpiration = cacheItem.SlidingExpiration;

                if (slidingExpiration.HasValue)
                {
                    existingItem.SlidingExpirationTimer = new Timer(
                        _ => Remove(k),
                        null,
                        slidingExpiration.Value,
                        Timeout.InfiniteTimeSpan);
                }

                return existingItem;
            });
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_cache.TryGetValue(key, out var cacheItem))
            {
                // Check absolute expiration
                if (DateTime.UtcNow >= cacheItem.AbsoluteExpiration)
                {
                    Remove(key);
                    value = default(TValue);
                    return false;
                }

                // Reset the sliding expiration timer if applicable
                if (cacheItem.SlidingExpirationTimer != null && cacheItem.SlidingExpiration.HasValue)
                {
                    cacheItem.SlidingExpirationTimer.Change(
                        cacheItem.SlidingExpiration.Value,
                        Timeout.InfiniteTimeSpan);
                }

                value = cacheItem.Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public void Remove(TKey key)
        {
            if (_cache.TryRemove(key, out var cacheItem))
            {
                cacheItem.SlidingExpirationTimer?.Dispose();
            }
        }

        public void Clear()
        {
            foreach (var key in _cache.Keys)
            {
                Remove(key);
            }
        }
    }


}

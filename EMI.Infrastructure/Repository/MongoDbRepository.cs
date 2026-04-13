using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using EMI.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Infrastructure.Repository
{
    public class MongoDbRepository<T> : IMongoDbRepository<T>
    {
        private MongoDbContext _context;
        private IMongoCollection<T> _collection;

        public MongoDbRepository(MongoDbContext context)
        {
            _context = context;
            _collection = context.GetCollection<T>(typeof(T).Name);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetMany(string filterBy, string filterValue)
        {
            var filter = Builders<T>.Filter.Eq(filterBy, filterValue);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (Exception)
            {
            }
        }

        public async Task<bool> UpdateAsync(string id, T entity)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                var result = await _collection.ReplaceOneAsync(filter, entity);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                var result = await _collection.DeleteOneAsync(filter);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

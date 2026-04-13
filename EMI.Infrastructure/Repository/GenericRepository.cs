using Microsoft.EntityFrameworkCore;
using EMI.Domain.Entities;
using EMI.Infrastructure.DbContexts;
using EMI.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly EMIContext _context;
        private DbSet<TEntity> DbSet;

        public GenericRepository(EMIContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Insert single record
        /// </summary>
        /// <param name="entity">entity to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<TEntity> InsertAsync(TEntity entity, bool commitTransaction = true)
        {
            try
            {
                entity.CreationDate = DateTime.Now;
                entity.IsDeleted = false;

                var result = DbSet.Add(entity);


                if (commitTransaction)
                {
                    await _context.SaveChangesAsync();
                }

                return result.Entity;
            }
            catch (Exception)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }


        /// <summary>
        /// Insert multiple record
        /// </summary>
        /// <param name="entities">entities to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<List<TEntity>> InsertAsync(List<TEntity> entities, bool commitTransaction = true)
        {
            try
            {
                var now = DateTime.Now;

                foreach (var e in entities)
                {
                    e.CreationDate = now;
                    e.IsDeleted = false;
                }

                DbSet.AddRange(entities);

                if (!commitTransaction)
                {
                    if (_context.Database.CurrentTransaction == null)
                    {
                        await _context.Database.BeginTransactionAsync();
                    }
                }

                if (commitTransaction)
                {
                    await _context.SaveChangesAsync();
                }

                return entities;
            }
            catch (Exception)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }


        /// <summary>
        /// Update single record
        /// </summary>
        /// <param name="entity">entity to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<TEntity> UpdateAsync(TEntity entity, bool commitTransaction = true)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                entity.LastModifiedDate = DateTime.Now;
                DbSet.Update(entity);

                if (commitTransaction)
                {
                    int rowsAffected = await _context.SaveChangesAsync();
                    if (rowsAffected > 0)
                    {
                        return entity;
                    }
                    return null;
                }

                return entity;
            }
            catch (Exception ex)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        /// <summary>
        /// Update multiple record
        /// </summary>
        /// <param name="entities">entities to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<List<TEntity>> UpdateAsync(List<TEntity> entities, bool commitTransaction = true)
        {
            if (entities == null || !entities.Any())
            {
                return new List<TEntity>();
            }

            try
            {
                foreach (var entity in entities)
                {
                    entity.LastModifiedDate = DateTime.Now;
                    DbSet.Update(entity);
                }

                if (commitTransaction)
                {
                    int rowsAffected = await _context.SaveChangesAsync();
                    if (rowsAffected > 0)
                    {
                        return entities;
                    }
                    return new List<TEntity>();
                }

                return entities;
            }
            catch (Exception)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        public async Task<TEntity> GetByIdAsync(long Id)
        {
            try
            {
                var result = await DbSet.FirstOrDefaultAsync(p => p.Id == Id && !p.IsDeleted);
                
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TEntity GetOne(Func<TEntity, bool> predicate)
        {
            try
            {
                return DbSet.Where(p => !p.IsDeleted)?.Where(predicate)?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await DbSet.Where(p => !p.IsDeleted).Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var result = await DbSet.ToListAsync();
            
            result = result.Where(p => !p.IsDeleted).ToList();

            if (result == null)
            {
                return null;
            }
            return result;
        }

        public (List<TEntity> records, int totalRecords) Paginate(int page, int pageSize, Func<TEntity, Boolean> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var result = DbSet
                        .Where(p => !p.IsDeleted)
                        .Where<TEntity>(predicate)
                        .Skip((page) * pageSize)
                        .Take(pageSize)
                        .ToList();

            int count = DbSet.Where(p=>!p.IsDeleted).Count();

            return (result, count);
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return _context
                .Set<TEntity>()
                .Where(c => !c.IsDeleted)
                .Where(predicate);
        }
        public async Task<List<TEntity>> GetManyByIdsAsync(IEnumerable<long> ids)
        {
            try
            {
                return await DbSet
                    .Where(e => ids.Contains(e.Id) && !e.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception)
            {
                _context.ChangeTracker.Clear(); // Optional: Clear tracking if needed after failure
                throw;
            }
        }

        public async Task<TEntity> GetWithIncludesAsync(
            Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            IQueryable<TEntity> query = DbSet.Where(e => !e.IsDeleted).Where(predicate);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync();
        }
        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return DbSet.AsNoTracking()
                        .Where(e => !e.IsDeleted)
                        .AnyAsync(predicate);
        }
    }
}

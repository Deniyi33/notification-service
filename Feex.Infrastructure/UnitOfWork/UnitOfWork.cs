using Feex.Core.Entities;
using Feex.Core.Entities;
using Feex.Infrastructure.DbContexts;
using Feex.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Feex.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EMIContext _context;

        public UnitOfWork(EMIContext context)
        {
            _context = context;
        }
        #region Declarations -- Expand to see more
        private IGenericRepository<Tenant> _tenantRepository;



        #endregion


        #region Implementation -- Expand to see more

        public IGenericRepository<Tenant> Tenant => _tenantRepository ??= new GenericRepository<Tenant>(_context);
        
        #endregion



        #region Execution Strategy / Transactions
        public async Task ExecuteWithStrategyAsync(Func<Task> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(operation);
        }

        public async Task<T> ExecuteWithStrategyAsync<T>(Func<Task<T>> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(operation);
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            await ExecuteWithStrategyAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await operation();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        #endregion

        #region Commit / Rollback
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RollBackAsync()
        {
            if (_context.Database.CurrentTransaction != null)
                await _context.Database.RollbackTransactionAsync();
        }
        #endregion

    }
}

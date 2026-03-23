using Feex.Core.Entities;
using Feex.Core.Entities;
using Feex.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace Feex.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Tenant> Tenant { get; }

        


        Task ExecuteWithStrategyAsync(Func<Task> operation);
        Task<T> ExecuteWithStrategyAsync<T>(Func<Task<T>> operation);

        Task ExecuteInTransactionAsync(Func<Task> operation);

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CommitAsync();
        Task RollBackAsync();
    }
}

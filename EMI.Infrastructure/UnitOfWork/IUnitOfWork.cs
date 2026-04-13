using EMI.Domain.Entities;
using EMI.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace EMI.Infrastructure.UnitOfWork
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

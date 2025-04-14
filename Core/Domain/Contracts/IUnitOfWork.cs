using Domain.Entities;

namespace Domain.Contracts;

public interface IUnitOfWork
{
    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : EntityBase<TKey>;
    Task<int> CompleteAsync();

}

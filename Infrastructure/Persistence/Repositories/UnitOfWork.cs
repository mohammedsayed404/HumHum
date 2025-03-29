using Domain.Contracts;
using Domain.Entities;
using System.Collections.Concurrent;

namespace Persistence.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly HumHumContext _dbContext;
    private readonly ConcurrentDictionary<string, object> _repositories;

    public UnitOfWork(HumHumContext dbContext)
    {
        _dbContext = dbContext;
        _repositories = new ConcurrentDictionary<string, object>();
    }

    public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();

    public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : EntityBase<TKey>
     => (IRepository<TEntity, TKey>)
        _repositories.GetOrAdd(typeof(TEntity).Name, _ => new Repository<TEntity, TKey>(_dbContext));
}

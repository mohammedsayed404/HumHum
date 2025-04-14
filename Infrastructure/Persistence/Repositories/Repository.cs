using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    private protected readonly HumHumContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(HumHumContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _dbSet.Where(entity => !entity.IsDeleted).AsNoTracking().ToListAsync();

    public async Task<TEntity?> GetByIdAsync(TKey key) => await _dbSet.FindAsync(key)!;
    public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Remove(TEntity entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }

    public void Update(TEntity entity) => _dbSet.Update(entity);
}

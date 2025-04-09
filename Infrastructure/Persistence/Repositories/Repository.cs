using Domain.Common;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{
    private readonly HumHumContext _dbContext;
    private protected readonly DbSet<TEntity> _dbSet;

    public Repository(HumHumContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> spec)
       => await ApplySpecification(spec).AsNoTracking().ToListAsync();



    public async Task<TEntity?> GetByIdAsync(TKey key) => await _dbSet.FindAsync(key)!;

    public async Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity> spec)
      => await ApplySpecification(spec).FirstOrDefaultAsync();

    public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public void Remove(TEntity entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }

    public void Update(TEntity entity) => _dbSet.Update(entity);


    private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity> spec)
        => SpecificationEvaluator.GetQuery(_dbSet, spec);

}

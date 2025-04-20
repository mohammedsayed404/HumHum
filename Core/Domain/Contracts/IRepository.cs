using Domain.Common;

namespace Domain.Contracts;

public interface IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{

    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity> spec);

    Task<TEntity?> GetByIdAsync(TKey key);
    Task<TEntity?> GetByIdWithSpecAsync(ISpecifications<TEntity> spec);
    Task InsertAsync(TEntity entity);

    void Remove(TEntity entity);
    void Update(TEntity entity);





}

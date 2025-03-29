using Domain.Entities;
namespace Domain.Contracts;

public interface IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
{

    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task<TEntity?> GetByIdAsync(TKey key);
    Task InsertAsync(TEntity entity);

    void Remove(TEntity entity);
    void Update(TEntity entity);


}

using System.Linq.Expressions;

namespace Domain.Contracts;

public interface ISpecifications<TEntity>  //need to modifying it again
{

    public Expression<Func<TEntity, bool>> Criteria { get; }
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes { get; }
    public Expression<Func<TEntity, object>> OrderBy { get; }
    public Expression<Func<TEntity, object>> OrderByDescending { get; }

    public int? Take { get; }
    public int? Skip { get; }

}

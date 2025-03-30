using System.Linq.Expressions;

namespace Domain.Contracts;

public interface ISpecifications<TEntity>  //need to modifying it again
{

    public Expression<Func<TEntity, bool>> Criteria { get; }
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes { get; }
}

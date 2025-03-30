using System.Linq.Expressions;

namespace Domain.Contracts;

public abstract class SpecificationsBase<TEntity> : ISpecifications<TEntity>
{
    public Expression<Func<TEntity, bool>> Criteria { get; private set; } = null!;

    private readonly List<Expression<Func<TEntity, object>>> _includes = new();
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes => _includes.AsReadOnly();


    protected SpecificationsBase() { }


    protected SpecificationsBase(Expression<Func<TEntity, bool>> predicate)
    {
        Criteria = predicate;
    }

    protected void AddIncludes(Expression<Func<TEntity, object>> expression)
        => _includes.Add(expression);



}

using System.Linq.Expressions;

namespace Domain.Contracts;

public abstract class SpecificationsBase<TEntity> : ISpecifications<TEntity>
{
    public Expression<Func<TEntity, bool>> Criteria { get; private set; } = null!;

    private readonly List<Expression<Func<TEntity, object>>> _includes = new();
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes => _includes.AsReadOnly();

    public Expression<Func<TEntity, object>> OrderBy { get; private set; } = null!;

    public Expression<Func<TEntity, object>> OrderByDescending { get; private set; } = null!;

    public int? Take { get; private set; } = null!;

    public int? Skip { get; private set; } = null!;

    protected SpecificationsBase() { }


    protected SpecificationsBase(Expression<Func<TEntity, bool>> predicate)
    {
        Criteria = predicate;
    }

    protected void AddIncludes(Expression<Func<TEntity, object>> expression)
        => _includes.Add(expression);

    protected void ApplyOrderBy(Expression<Func<TEntity, object>> expression)
        => OrderBy = expression;

    protected void ApplyOrderByDescending(Expression<Func<TEntity, object>> expression)
        => OrderByDescending = expression;

    protected void ApplyTake(int count)
        => Take = count;

    protected void ApplySkip(int count)
        => Skip = count;
}

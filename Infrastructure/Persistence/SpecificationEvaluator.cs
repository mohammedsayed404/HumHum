using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
namespace Persistence;

internal static class SpecificationEvaluator
{

    public static IQueryable<TEntity> GetQuery<TEntity>
        (IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec) where TEntity : class
    {

        var query = inputQuery;

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);


        if (spec.Includes is not null)
            query = spec.Includes.Aggregate(query, (current, expressions) => current.Include(expressions));

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);

        else if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.Take is not null)
            query = query.Take(spec.Take ?? 0);

        if (spec.Skip is not null)
            query = query.Skip(spec.Skip ?? 0);

        return query;


    }

}

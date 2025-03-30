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
            query.Where(spec.Criteria);


        if (spec.Includes is not null)
            query = spec.Includes.Aggregate(query, (current, expressions) => current.Include(expressions));




        return query;


    }

}

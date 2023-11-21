using Light.Core.Extensions.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Light.Infrastructure.Extensions.Extensions;

public static class QuerySpecificationExtensions
{
    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class
    {
        var queryWithIncludes = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        var queryWithStringIncludes =
            spec.IncludeStrings.Aggregate(queryWithIncludes, (current, include) => current.Include(include));

        if (spec.Criteria != null)
        {
            return queryWithStringIncludes.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            queryWithStringIncludes = queryWithStringIncludes.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            queryWithStringIncludes = queryWithStringIncludes.OrderByDescending(spec.OrderByDescending);
        }

        if (spec.GroupBy != null)
        {
            queryWithStringIncludes = queryWithStringIncludes.GroupBy(spec.GroupBy).SelectMany(x => x);
        }

        if (spec.IsPagingEnabled)
        {
            queryWithStringIncludes = queryWithStringIncludes.Skip(spec.Skip).Take(spec.Take);
        }

        return queryWithStringIncludes;
    }
}
using System.Linq.Expressions;

namespace Light.Core.Extensions.Specifications.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    Expression<Func<T, object>>? GroupBy { get; }
    ICollection<Expression<Func<T, object>>> Includes { get; }
    ICollection<string> IncludeStrings { get; }
    bool IsPagingEnabled { get; }
    int Take { get; }
    int Skip { get; }
}
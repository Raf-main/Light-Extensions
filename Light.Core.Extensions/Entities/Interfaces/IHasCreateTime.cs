namespace Light.Core.Extensions.Entities.Interfaces;

public interface IHasCreateTime
{
    DateTimeOffset CreatedOn { get; }
}
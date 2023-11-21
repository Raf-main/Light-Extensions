namespace Light.Core.Extensions.Entities.Interfaces;

public interface IHasUpdateTime
{
    DateTimeOffset UpdatedOn { get; }
}
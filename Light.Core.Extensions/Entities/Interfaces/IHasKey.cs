namespace Light.Core.Extensions.Entities.Interfaces;

public interface IHasKey<out T> where T : struct
{
    T Id { get; }
}
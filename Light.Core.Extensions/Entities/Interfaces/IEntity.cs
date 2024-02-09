namespace Light.Core.Extensions.Entities.Interfaces;

public interface IEntity;

public interface IEntity<out TKey> : IHasKey<TKey> where TKey : struct;
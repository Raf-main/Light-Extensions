namespace Light.Core.Extensions.Entities.Interfaces;

public interface IEntity { }

public interface IEntity<TKey> : IHasKey<TKey> where TKey : struct { }
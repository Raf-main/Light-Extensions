namespace Light.MessageBrokers.Interfaces;

public interface IMessageBroker
{
    void Subscribe(Type type, Action<string?> callback, CancellationToken ct);
    void Subscribe<T>(Action<string?> callback, CancellationToken ct) where T : class;
    Task Publish<T>(T entity) where T : class;
    Task Publish(string message, string type);
}
namespace Light.MessageBrokers;

public static class MessageBrokersHelper
{
    public static string GetTypeName(Type type)
    {
        var name = type.FullName?.ToLower();

        return name ?? "default";
    }

    public static string GetTypeName<T>()
    {
        return GetTypeName(typeof(T));
    }
}

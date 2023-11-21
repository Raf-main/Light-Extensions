using Light.Core.Extensions.Entities.Interfaces;

namespace Light.Core.Extensions.Entities;

public abstract class Entity<TKey> : IEntity<TKey> where TKey : struct
{
    public virtual TKey Id { get; set; }

    public static bool operator ==(Entity<TKey>? first, Entity<TKey>? second)
    {
        if (first is null && second is null)
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        var result = first.Equals(second);

        return result;
    }

    public static bool operator !=(Entity<TKey>? first, Entity<TKey>? second)
    {
        var result = !(first == second);

        return result;
    }

    public bool Equals(IEntity<TKey>? other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        var result = Id.Equals(other.Id);

        return result;
    }

    public override bool Equals(object? obj)
    {
        var other = obj as Entity<TKey>;
        var result = Equals(other);

        return result;
    }

    public override int GetHashCode()
    {
        var result = HashCode.Combine(Id);

        return result;
    }
}
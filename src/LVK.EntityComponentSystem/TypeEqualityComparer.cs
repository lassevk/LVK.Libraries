namespace LVK.EntityComponentSystem;

internal sealed class TypeEqualityComparer : IEqualityComparer<object>
{
    bool IEqualityComparer<object>.Equals(object? x, object? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.GetType() == y.GetType();
    }

    int IEqualityComparer<object>.GetHashCode(object obj) => obj.GetType().GetHashCode();
}
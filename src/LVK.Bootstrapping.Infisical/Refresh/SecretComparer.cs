namespace LVK.Bootstrapping.Infisical.Refresh;

internal class SecretComparer : IEqualityComparer<Secret>
{
    public bool Equals(Secret? x, Secret? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null)
        {
            return false;
        }

        if (y is null)
        {
            return false;
        }

        if (x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Id == y.Id && x.ProjectId == y.ProjectId && x.Environment == y.Environment
            && x.Version == y.Version && x.SecretKey == y.SecretKey && x.SecretValue == y.SecretValue && x.SecretComment == y.SecretComment && x.SecretReminderNote == y.SecretReminderNote && x.SecretReminderRepeatDays == y.SecretReminderRepeatDays && x.SkipMultilineEncoding == y.SkipMultilineEncoding && x.IsRotatedSecret == y.IsRotatedSecret && x.RotationId == y.RotationId && x.SecretPath == y.SecretPath;
    }

    public int GetHashCode(Secret obj)
    {
        var hashCode = new HashCode();
        hashCode.Add(obj.Id);
        hashCode.Add(obj.ProjectId);
        hashCode.Add(obj.Environment);
        hashCode.Add(obj.Version);
        hashCode.Add(obj.SecretKey);
        hashCode.Add(obj.SecretValue);
        hashCode.Add(obj.SecretComment);
        hashCode.Add(obj.SecretReminderNote);
        hashCode.Add(obj.SecretReminderRepeatDays);
        hashCode.Add(obj.SkipMultilineEncoding);
        hashCode.Add(obj.IsRotatedSecret);
        hashCode.Add(obj.RotationId);
        hashCode.Add(obj.SecretPath);
        return hashCode.ToHashCode();
    }
}
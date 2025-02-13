using JetBrains.Annotations;

namespace LasseVK.Ssh;

[PublicAPI]
public class SshProxyValidationErrors : Exception
{
    public SshProxyValidationErrors(string message, IEnumerable<string> errors)
        : base(message)
    {
        Errors = errors.ToArray();
    }

    public string[] Errors { get; }
}
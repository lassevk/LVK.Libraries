using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace LasseVK.Ssh;

[PublicAPI]
public class SshProxyPort
{
    public SshProxyHost? Host { get; set; }

    public required int LocalPort { get; set; }

    public int? RemotePort { get; set; }

    public string? RemoteHost { get; set; }

    public IEnumerable<string> GetValidationErrors()
    {
        if (LocalPort <= 0 || LocalPort > 65535)
        {
            yield return "Local port must be in the range 1 to 65535";
        }

        if (RemotePort != null && (RemotePort <= 0 || RemotePort > 65535))
        {
            yield return "Remote port must be in the range 1 to 65535";
        }

        if (RemoteHost != null && string.IsNullOrWhiteSpace(RemoteHost))
        {
            yield return "Remote host can be null for default, but not whitespace only";
        }
    }
}
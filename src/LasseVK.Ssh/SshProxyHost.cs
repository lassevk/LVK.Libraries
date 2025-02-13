using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace LasseVK.Ssh;

[PublicAPI]
[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class SshProxyHost
{
    public required string Hostname { get; set; }

    public required int Port { get; set; } = 22;

    public SshProxyAuthentication? Authentication { get; set; }

    internal IEnumerable<string> GetValidationErrors()
    {
        if (string.IsNullOrWhiteSpace(Hostname))
        {
            yield return "Hostname is required";
        }

        if (Port <= 0 || Port > 65535)
        {
            yield return "Port must be in the range 1 to 65535";
        }

        if (Authentication == null)
        {
            yield return "Authentication is required";
        }
        else
        {
            foreach (string error in Authentication.GetValidationErrors())
            {
                yield return error;
            }
        }
    }
}
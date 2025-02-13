using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace LasseVK.Ssh;

[PublicAPI]
public class SshProxyOptions
{
    public const string ConfigurationKey = "Ssh:Proxy";

    public SshProxyHost? Host { get; set; }

    public List<SshProxyPort>? Ports { get; set; }

    internal void Validate()
    {
        var errors = new List<string>();
        if (Host != null)
        {
            foreach (string error in Host.GetValidationErrors())
            {
                errors.Add(error);
            }
        }

        if (Ports != null)
        {
            foreach (SshProxyPort port in Ports)
            {
                if (port.Host == null && Host == null)
                {
                    errors.Add("Port is missing host, and no common host is specified.");
                }

                foreach (string error in port.GetValidationErrors())
                {
                    errors.Add(error);
                }
            }
        }

        switch (errors.Count)
        {
            case 1:
                throw new SshProxyValidationErrors(errors[0], errors);

            case > 1:
                throw new SshProxyValidationErrors(errors[0] + " + more", errors);
        }
    }
}
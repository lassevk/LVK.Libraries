using System.Text.Json.Serialization;

using JetBrains.Annotations;

namespace LasseVK.Ssh;

[PublicAPI]
public class SshProxyAuthentication
{
    public required string Username { get; set; }

    public string? Password { get; set; }

    public string? CertificatePath { get; set; }

    public string? Certificate { get; set; }

    public string? CertificatePassword { get; set; }

    public IEnumerable<string> GetValidationErrors()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            yield return "Username is required";
        }

        int methodsSpecified = (!string.IsNullOrWhiteSpace(Password) ? 1 : 0) + (!string.IsNullOrWhiteSpace(CertificatePath) ? 1 : 0) + (!string.IsNullOrWhiteSpace(Certificate) ? 1 : 0);
        switch (methodsSpecified)
        {
            case 0:
                yield return "Password, Certificate or CertificatePath is required";

                break;

            case 1:
                break;

            case > 1:
                yield return "Only one of Password, Certificate or CertificatePath can be specified";

                break;
        }
    }
}
namespace LVK.Bootstrapping.Infisical.Configuration;

public class InfisicalOptions
{
    public string? HostUri { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? ProjectId { get; set; }
    public string Environment { get; set; } = "dev";
    public string SecretPath { get; set; } = "/";
    public long? RefreshIntervalSeconds { get; set; } = 60 * 5;

    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(HostUri))
        {
            return false;
        }

        bool isValid = true;
        if (string.IsNullOrWhiteSpace(ClientId))
        {
            isValid = false;
            Console.Error.WriteLine("infisical: missing client id");
        }

        if (string.IsNullOrWhiteSpace(ClientSecret))
        {
            Console.Error.WriteLine("infisical: missing client secret");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(ProjectId))
        {
            Console.Error.WriteLine("infisical: missing project id");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Environment))
        {
            Console.Error.WriteLine("infisical: missing environment");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(SecretPath))
        {
            Console.Error.WriteLine("infisical: missing secret path");
            isValid = false;
        }

        return isValid;
    }
}
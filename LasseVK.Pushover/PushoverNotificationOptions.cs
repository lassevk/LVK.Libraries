namespace LasseVK.Pushover;

public class PushoverNotificationOptions
{
    public const string SectionName = "Pushover";

    public string? DefaultUser { get; set; }
    public string? ApiToken { get; set; }

    public PushoverNotificationOptions UseApiToken(string apiToken)
    {
        ApiToken = apiToken;
        return this;
    }

    public PushoverNotificationOptions UseDefaultUser(string defaultUser)
    {
        DefaultUser = defaultUser;
        return this;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiToken))
        {
            throw new ArgumentException("ApiToken is required");
        }
    }
}
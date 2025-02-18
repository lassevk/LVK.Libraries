namespace LasseVK.AiExtensions;

public interface IAIToolProvider
{
    public string Name { get; }
    public string Description { get; }
    Delegate Delegate { get; }
}

public class CurrentDateAiToolProvider : IAIToolProvider
{
    public string Name => "GetCurrentDate";
    public string Description => "Gets the current date in 'yyyy-mm-dd' format";
    public Delegate Delegate => GetCurrentDate;

    private string  GetCurrentDate() => DateTime.Now.ToString("yyyy-MM-dd");
}
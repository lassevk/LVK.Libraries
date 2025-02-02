using System.Diagnostics;
using System.Text.Json.Serialization;

namespace LasseVK.Jobs;

[DebuggerDisplay("Job (Id={Id})")]
public abstract class Job
{
    [JsonPropertyName("id")]
    public string Id { get; internal set; } = Guid.CreateVersion7().ToString("N");

    [JsonPropertyName("ex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ExceptionSnapshot? Exception { get; internal set; }

    [JsonIgnore]
    public virtual string Group => "";

    public void EnsureSuccess()
    {
        if (Exception != null)
        {
            throw new InvalidOperationException("Job failed");
        }
    }

    public override string ToString() => $"{GetType().Name}#{Id}";
}
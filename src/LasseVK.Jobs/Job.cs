using System.Diagnostics;
using System.Text.Json.Serialization;

namespace LasseVK.Jobs;

[DebuggerDisplay("Job (Id={Id})")]
public abstract class Job
{
    [JsonPropertyName("id")]
    public string Id { get; internal set; } = Guid.CreateVersion7().ToString("N");

    [JsonPropertyName("ex")]
    public Exception? Exception { get; internal set; }
}
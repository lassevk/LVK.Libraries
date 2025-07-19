using System.Diagnostics;
using System.Text.Json.Serialization;

namespace LVK.Jobs;

[DebuggerDisplay("Job (Id={Id})")]
public abstract class Job
{
    [JsonPropertyName("id")]
    public string Id { get; internal set; } = Guid.CreateVersion7().ToString("N");

    [JsonPropertyName("ex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ExceptionSnapshot? Exception { get; set; }

    [JsonIgnore]
    public virtual string Group => "";

    public override string ToString() => $"{(string.IsNullOrWhiteSpace(Group) ? "*" : Group)}:{GetType().Name}#{Id}";
}
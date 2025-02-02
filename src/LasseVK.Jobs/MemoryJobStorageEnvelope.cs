using System.Diagnostics;

namespace LasseVK.Jobs;

[DebuggerDisplay("Envelope (Id={Id}, Status={Status})")]
internal class MemoryJobStorageEnvelope
{
    public required string Id { get; init; }
    public required SerializedJob Job { get; internal set; }
    public required List<string> Dependencies { get; init; }

    public JobStatus Status { get; set; } = JobStatus.Queued;
}
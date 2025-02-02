namespace LasseVK.Jobs;

public class SerializedJob
{
    public required string Json { get; init; }
    public required string Identifier { get; init; }
    public required string Group { get; init; }
}
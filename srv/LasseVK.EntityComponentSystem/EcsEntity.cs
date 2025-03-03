using System.Diagnostics.CodeAnalysis;

namespace LasseVK.EntityComponentSystem;

public readonly record struct EcsEntity
{
    private readonly EcsContext _context;

    internal EcsEntity(EcsContext context, int id)
    {
        _context = context;
        Id = id;
    }

    public int Id { get; }

    public void SetComponent<T>(T component)
        where T : class
        => _context.SetComponent(Id, component);

    public bool TryRemoveComponent<T>()
        where T : class
        => _context.TryRemoveComponent<T>(Id);

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component)
        where T : class
        => _context.TryGetComponent(Id, out component);

    public T GetComponent<T>()
        where T : class
        => _context.TryGetComponent(Id, out T? component) ? component : throw new MissingMemberException();

    public override string ToString() => $"entity#{Id}";

    public void SetComponents<T>(T components)
        where T : notnull
        => _context.SetComponents(Id, components);
}
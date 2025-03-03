using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace LasseVK.EntityComponentSystem;

public readonly record struct EcsEntity
{
    private readonly EcsContext _context;
    private readonly int _id;

    internal EcsEntity(EcsContext context, int id)
    {
        _context = context;
        _id = id;
    }

    public int Id => _id;

    public void SetComponent<T>(T component)
        where T : class
        => _context.SetComponent(_id, component);

    public bool TryRemoveComponent<T>()
        where T : class
        => _context.TryRemoveComponent<T>(_id);

    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component)
        where T : class
        => _context.TryGetComponent(_id, out component);

    public T GetComponent<T>()
        where T : class
        => _context.TryGetComponent(_id, out T? component) ? component : throw new MissingMemberException();

    public override string ToString() => $"entity#{Id}";

    public void SetComponents<T>(T components)
        where T : notnull
        => _context.SetComponents(_id, components);
}
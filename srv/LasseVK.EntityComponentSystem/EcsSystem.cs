namespace LasseVK.EntityComponentSystem;

public class EcsSystem : IDisposable
{
    private readonly HashSet<int> _entityIds = new();
    private readonly EcsContext _context;

    internal EcsSystem(EcsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected internal void AddEntity(int entityId)
    {
        if (_entityIds.Add(entityId))
        {
            EntityAdded?.Invoke(this, entityId);
        }
    }

    protected internal void RemoveEntity(int entityId)
    {
        if (_entityIds.Remove(entityId))
        {
            EntityRemoved?.Invoke(this, entityId);
        }
    }

    internal bool ContainsEntityId(int entityId) => _entityIds.Contains(entityId);
    public bool ContainsEntity(EcsEntity entity) => _entityIds.Contains(entity.Id);

    public EcsEntity[] GetEntities() => _entityIds.OrderBy(e => e).Select(id => new EcsEntity(_context, id)).ToArray();

    internal event EntityEventHandler? EntityAdded;
    internal event EntityEventHandler? EntityRemoved;

    public virtual void Dispose()
    {
        _context.RemoveSystem(this);
        _entityIds.Clear();
    }

    public EcsSystem And(EcsSystem other) => new EcsAndOperatorSystem(_context, this, other);
    public EcsSystem Or(EcsSystem other) => new EcsOrOperatorSystem(_context, this, other);
    public EcsSystem Xor(EcsSystem other) => new EcsXorOperatorSystem(_context, this, other);
}
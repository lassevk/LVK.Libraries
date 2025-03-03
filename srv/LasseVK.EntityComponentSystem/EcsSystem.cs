namespace LasseVK.EntityComponentSystem;

public class EcsSystem : IDisposable
{
    private readonly HashSet<int> _entityIds = new();
    private readonly EcsContext _context;

    internal EcsSystem(EcsContext context)
    {
        _context = context;
    }

    protected internal void AddEntity(int entityId)
    {
        if (_entityIds.Add(entityId))
        {
            EntityAdded?.Invoke(this, new EcsEntity(_context, entityId));
        }
    }

    protected internal void RemoveEntity(int entityId)
    {
        if (_entityIds.Remove(entityId))
        {
            EntityRemoved?.Invoke(this, new EcsEntity(_context, entityId));
        }
    }

    public bool ContainsEntity(EcsEntity entity) => _entityIds.Contains(entity.Id);
    public EcsEntity[] GetEntities() => _entityIds.OrderBy(e => e).Select(id => new EcsEntity(_context, id)).ToArray();

    public event EntityEventHandler? EntityAdded;
    public event EntityEventHandler? EntityRemoved;

    public virtual void Dispose()
    {
        _context.RemoveSystem(this);
        _entityIds.Clear();
    }
}
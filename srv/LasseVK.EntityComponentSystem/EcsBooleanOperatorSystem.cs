namespace LasseVK.EntityComponentSystem;

public abstract class EcsBooleanOperatorSystem : EcsSystem
{
    private readonly EcsSystem _left;
    private readonly EcsSystem _right;

    internal EcsBooleanOperatorSystem(EcsContext context, EcsSystem left, EcsSystem right)
        : base(context)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));

        left.EntityAdded += OnEntityAdded;
        left.EntityRemoved += OnEntityRemoved;

        right.EntityAdded += OnEntityAdded;
        right.EntityRemoved += OnEntityRemoved;
    }

    protected abstract bool Operator(bool isInLeft, bool isInRight);

    private void OnEntityAdded(EcsSystem sender, int entityId)
    {
        if (Operator(_left.ContainsEntityId(entityId), _right.ContainsEntityId(entityId)))
        {
            AddEntity(entityId);
        }
    }

    private void OnEntityRemoved(EcsSystem sender, int entityId)
    {
        if (!Operator(_left.ContainsEntityId(entityId), _right.ContainsEntityId(entityId)))
        {
            RemoveEntity(entityId);
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        _left.EntityAdded -= OnEntityAdded;
        _left.EntityRemoved -= OnEntityRemoved;

        _right.EntityAdded -= OnEntityAdded;
        _right.EntityRemoved -= OnEntityRemoved;
    }
}
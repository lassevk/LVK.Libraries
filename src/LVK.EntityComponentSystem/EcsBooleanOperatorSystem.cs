namespace LVK.EntityComponentSystem;

public abstract class EcsBooleanOperatorSystem : EcsSystem
{
    private readonly EcsSystem _left;
    private readonly EcsSystem _right;

    internal EcsBooleanOperatorSystem(EcsContext context, EcsSystem left, EcsSystem right)
        : base(context)
    {
        _left = left ?? throw new ArgumentNullException(nameof(left));
        _right = right ?? throw new ArgumentNullException(nameof(right));

        left.EntityAdded += OnEntityAddedOrRemoved;
        left.EntityRemoved += OnEntityAddedOrRemoved;

        right.EntityAdded += OnEntityAddedOrRemoved;
        right.EntityRemoved += OnEntityAddedOrRemoved;
    }

    protected abstract bool Operator(bool isInLeft, bool isInRight);

    private void OnEntityAddedOrRemoved(EcsSystem sender, int entityId)
    {
        if (Operator(_left.ContainsEntityId(entityId), _right.ContainsEntityId(entityId)))
        {
            AddEntity(entityId);
        }
        else
        {
            RemoveEntity(entityId);
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        _left.EntityAdded -= OnEntityAddedOrRemoved;
        _left.EntityRemoved -= OnEntityAddedOrRemoved;

        _right.EntityAdded -= OnEntityAddedOrRemoved;
        _right.EntityRemoved -= OnEntityAddedOrRemoved;
    }
}
namespace LVK.EntityComponentSystem;

internal class EcsOrOperatorSystem : EcsBooleanOperatorSystem
{
    public EcsOrOperatorSystem(EcsContext context, EcsSystem left, EcsSystem right)
        : base(context, left, right) { }

    protected override bool Operator(bool isInLeft, bool isInRight) => isInLeft || isInRight;
}
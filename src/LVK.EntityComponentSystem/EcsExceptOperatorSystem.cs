namespace LVK.EntityComponentSystem;

internal class EcsExceptOperatorSystem : EcsBooleanOperatorSystem
{
    public EcsExceptOperatorSystem(EcsContext context, EcsSystem left, EcsSystem right)
        : base(context, left, right) { }

    protected override bool Operator(bool isInLeft, bool isInRight) => isInLeft && !isInRight;
}
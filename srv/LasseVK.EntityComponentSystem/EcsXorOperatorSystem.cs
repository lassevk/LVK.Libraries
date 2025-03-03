namespace LasseVK.EntityComponentSystem;

internal class EcsXorOperatorSystem : EcsBooleanOperatorSystem
{
    public EcsXorOperatorSystem(EcsContext context, EcsSystem left, EcsSystem right)
        : base(context, left, right) { }

    protected override bool Operator(bool isInLeft, bool isInRight) => isInLeft ^ isInRight;
}
namespace LVK.EntityComponentSystem.Tests;

public class AndOperatorSystemTests
{
    [Test]
    public void GetEntities_EntityHasNeitherComponent_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.And(system2);

        EcsEntity entity = context.CreateEntity();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityHasFirstComponent_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.And(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityHasSecondComponent_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.And(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component2("2"));

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityHasBothComponents_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.And(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.EqualTo(new[] { entity }));
    }

    [Test]
    public void GetEntities_EntityLostFirstComponent_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.And(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));
        entity.TryRemoveComponent<Component1>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityLostSecondComponent_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.And(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));
        entity.TryRemoveComponent<Component2>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }
}
namespace LasseVK.EntityComponentSystem.Tests;

public class XorOperatorSystemTests
{
    [Test]
    public void GetEntities_EntityHasNeitherComponent_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);

        context.CreateEntity();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityHasFirstComponent_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.EqualTo(new[] { entity }));
    }

    [Test]
    public void GetEntities_EntityHasSecondComponent_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component2("2"));

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.EqualTo(new[] { entity }));
    }

    [Test]
    public void GetEntities_EntityHasBothComponents_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityLostFirstComponent_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));
        entity.TryRemoveComponent<Component1>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.EqualTo(new[] { entity }));
    }

    [Test]
    public void GetEntities_EntityLostSecondComponent_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));
        entity.TryRemoveComponent<Component2>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.EqualTo(new[] { entity }));
    }

    [Test]
    public void GetEntities_EntityLostBothComponents_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));
        entity.TryRemoveComponent<Component1>();
        entity.TryRemoveComponent<Component2>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityLostFirstComponentButSystemHasBeenDisposed_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsSystem system1 = context.CreateSystem<Component1>();
        EcsSystem system2 = context.CreateSystem<Component2>();
        EcsSystem system = system1.Xor(system2);
        system.Dispose();

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent(new Component1(1));
        entity.SetComponent(new Component2("2"));
        entity.TryRemoveComponent<Component1>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }
}
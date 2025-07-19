namespace LVK.EntityComponentSystem.Tests;

public class SystemTests
{
    [Test]
    public void CreateSystem_CreatesEmptySystem()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityWithComponent_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.EqualTo(new[]
        {
            entity
        }));
    }

    [Test]
    public void GetEntities_EntityWithComponentButSystemWasCreatedAfterEntity_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");
        EcsSystem system = context.CreateSystem<string>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.EqualTo(new[]
        {
            entity
        }));
    }

    [Test]
    public void GetEntities_EntityGotItsComponentRemoved_NoLongerReturnsEntity()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");
        entity.TryRemoveComponent<string>();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_AfterSystemWasDisposed_NoLongerReactsToEntityChanges()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();
        system.Dispose();

        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");

        EcsEntity[] entities = system.GetEntities();
        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_AfterSystemWasDisposed_NoLongerReturnsEntities()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");
        system.Dispose();

        EcsEntity[] entities = system.GetEntities();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void ContainsEntity_WhenEntityIsAdded_ReturnsTrue()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");

        Assert.That(system.ContainsEntity(entity), Is.True);
    }

    [Test]
    public void ContainsEntity_WhenEntityIsNotAdded_ReturnsFalse()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();
        EcsEntity entity = context.CreateEntity();

        Assert.That(system.ContainsEntity(entity), Is.False);
    }

    [Test]
    public void ContainsEntity_WhenEntityIsAddedThenRemoved_ReturnsFalse()
    {
        var context = new EcsContext();
        EcsSystem system = context.CreateSystem<string>();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");
        entity.TryRemoveComponent<string>();

        Assert.That(system.ContainsEntity(entity), Is.False);
    }
}
namespace LasseVK.EntityComponentSystem.Tests;

public class ContextTests
{
    [Test]
    public void GetEntities_NoEntitiesWithComponent_ReturnsEmptyList()
    {
        var context = new EcsContext();

        EcsEntity[] entities = context.GetEntities<string>();

        Assert.That(entities, Is.Empty);
    }

    [Test]
    public void GetEntities_EntityWithComponent_ReturnsEntity()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");

        EcsEntity[] entities = context.GetEntities<string>();

        Assert.That(entities, Is.EqualTo(new[] { entity }));
    }

    [Test]
    public void GetEntities_EntitiesWithComponent_ReturnsEntities()
    {
        var context = new EcsContext();
        EcsEntity entity1 = context.CreateEntity();
        entity1.SetComponent("test");
        EcsEntity entity2 = context.CreateEntity();
        entity2.SetComponent("test2");

        EcsEntity[] entities = context.GetEntities<string>();

        Assert.That(entities, Is.EquivalentTo(new[] { entity1, entity2 }));
    }

    [Test]
    public void GetEntities_WhenComponentWasRemoved_DoesNotReturnEntity()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");
        entity.TryRemoveComponent<string>();

        EcsEntity[] entities = context.GetEntities<string>();

        Assert.That(entities, Is.Empty);
    }
}
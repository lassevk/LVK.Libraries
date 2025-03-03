namespace LasseVK.EntityComponentSystem.Tests;

public class EntityTests
{
    [Test]
    public void GetComponent_ComponentNotSet_ThrowsMissingMemberException()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        Assert.Throws<MissingMemberException>(() => entity.GetComponent<string>());
    }

    [Test]
    public void TryGetComponent_ComponentNotSet_ReturnsFalse()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        bool result = entity.TryGetComponent(out string? _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void TryGetComponent_ComponentSet_ReturnsTrueAndComponent()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");

        bool result = entity.TryGetComponent(out string? value);

        Assert.That(result, Is.True);
        Assert.That(value, Is.EqualTo("test"));
    }

    [Test]
    public void GetComponent_ComponentSet_ReturnsComponent()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");

        string value = entity.GetComponent<string>();

        Assert.That(value, Is.EqualTo("test"));
    }

    [Test]
    public void GetComponent_ComponentWasOverwritten_ReturnsNewComponent()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");
        entity.SetComponent("test2");

        string value = entity.GetComponent<string>();

        Assert.That(value, Is.EqualTo("test2"));
    }

    [Test]
    public void GetComponent_ComponentWasRemoved_ThrowsMissingMemberException()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");
        entity.TryRemoveComponent<string>();

        Assert.Throws<MissingMemberException>(() => entity.GetComponent<string>());
    }

    [Test]
    public void GetComponent_OtherComponentWasAdded_ThrowsMissingMemberException()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        entity.SetComponent("test");

        Assert.Throws<MissingMemberException>(() => entity.GetComponent<Stream>());
    }

    public void RemoveComponent_ComponentWasSet_ReturnsTrue()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();
        entity.SetComponent("test");

        bool value = entity.TryRemoveComponent<string>();

        Assert.That(value, Is.True);
    }

    [Test]
    public void RemoveComponent_ComponentWasNotSet_ReturnsFalse()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        bool value = entity.TryRemoveComponent<string>();

        Assert.That(value, Is.False);
    }
}
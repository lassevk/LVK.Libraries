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

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(value, Is.EqualTo("test"));
        });
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

    [Test]
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

    [Test]
    public void GetComponent_OnTwoEntitiesWithDifferentComponentValues_ReturnsCorrectComponents()
    {
        var context = new EcsContext();
        EcsEntity entity1 = context.CreateEntity();
        entity1.SetComponent("test");
        EcsEntity entity2 = context.CreateEntity();
        entity2.SetComponent("test2");

        string value1 = entity1.GetComponent<string>();
        string value2 = entity2.GetComponent<string>();

        Assert.Multiple(() =>
        {
            Assert.That(value1, Is.EqualTo("test"));
            Assert.That(value2, Is.EqualTo("test2"));
        });
    }

    [Test]
    public void Set_NoProperties_DoesNotAddComponents()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        entity.SetComponents(new
        {
        });

        bool value1 = entity.TryGetComponent<Component1>(out _);
        bool value2 = entity.TryGetComponent<Component2>(out _);

        Assert.Multiple(() =>
        {
            Assert.That(value1, Is.False);
            Assert.That(value2, Is.False);
        });
    }

    [Test]
    public void Set_OneProperty_AddsOneComponent()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        entity.SetComponents(new
        {
            c1 = new Component1(1),
        });

        bool value1 = entity.TryGetComponent<Component1>(out _);
        bool value2 = entity.TryGetComponent<Component2>(out _);

        Assert.Multiple(() =>
        {
            Assert.That(value1, Is.True);
            Assert.That(value2, Is.False);
        });
    }

    [Test]
    public void Set_TwoProperty_AddsTwoComponents()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        entity.SetComponents(new
        {
            c1 = new Component1(1),
            c2 = new Component2("2"),
        });

        bool value1 = entity.TryGetComponent<Component1>(out _);
        bool value2 = entity.TryGetComponent<Component2>(out _);

        Assert.Multiple(() =>
        {
            Assert.That(value1, Is.True);
            Assert.That(value2, Is.True);
        });
    }

    [Test]
    public void Set_ClearTwoProperty_RemovesTwoComponents()
    {
        var context = new EcsContext();
        EcsEntity entity = context.CreateEntity();

        entity.SetComponents(new
        {
            c1 = new Component1(1),
            c2 = new Component2("2"),
        });

        entity.SetComponents(new
        {
            c1 = (Component1?)null,
            c2 = (Component2?)null,
        });

        bool value1 = entity.TryGetComponent<Component1>(out _);
        bool value2 = entity.TryGetComponent<Component2>(out _);

        Assert.Multiple(() =>
        {
            Assert.That(value1, Is.False);
            Assert.That(value2, Is.False);
        });
    }

    [Test]
    public void ToString_TwoEntities_ReturnsDifferentStrings()
    {
        var context = new EcsContext();
        EcsEntity entity1 = context.CreateEntity();
        EcsEntity entity2 = context.CreateEntity();

        string value1 = entity1.ToString();
        string value2 = entity2.ToString();

        Assert.That(value1, Is.Not.EqualTo(value2));
    }
}
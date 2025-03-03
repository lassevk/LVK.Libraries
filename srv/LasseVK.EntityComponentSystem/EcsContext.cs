using System.Diagnostics.CodeAnalysis;

namespace LasseVK.EntityComponentSystem;

public class EcsContext
{
    private int _nextId;

    private readonly Dictionary<Type, HashSet<int>> _entitiesByComponent = new();
    private readonly Dictionary<int, Dictionary<Type, object>> _componentsByEntity = new();

    public EcsEntity CreateEntity() => new(this, _nextId++);

    public EcsEntity[] GetEntities<T>()
        where T : class
    {
        if (!_entitiesByComponent.TryGetValue(typeof(T), out HashSet<int>? entities))
        {
            return [];
        }

        return entities.OrderBy(i => i).Select(id => new EcsEntity(this, id)).ToArray();
    }

    internal void SetComponent<T>(int entityId, T component)
        where T : class
    {
        if (!_componentsByEntity.TryGetValue(entityId, out Dictionary<Type, object>? components))
        {
            components = [];
            _componentsByEntity.Add(entityId, components);
        }

        if (!_entitiesByComponent.TryGetValue(typeof(T), out HashSet<int>? entities))
        {
            entities = new HashSet<int>();
            _entitiesByComponent.Add(typeof(T), entities);
        }

        _entitiesByComponent[typeof(T)].Add(entityId);
        bool wasAdded = components.TryAdd(typeof(T), component);
        switch (wasAdded)
        {
            case true:
                // TODO: Notify systems
                break;

            case false:
                components[typeof(T)] = component;
                break;
        }
    }

    internal bool TryRemoveComponent<T>(int entityId)
        where T : class
    {
        if (!_componentsByEntity.TryGetValue(entityId, out Dictionary<Type, object>? components))
        {
            return false;
        }

        components.Remove(typeof(T));
        if (components.Count == 0)
        {
            _componentsByEntity.Remove(entityId);
        }

        HashSet<int> entitiesByComponent = _entitiesByComponent[typeof(T)];
        entitiesByComponent.Remove(entityId);
        if (entitiesByComponent.Count == 0)
        {
            _entitiesByComponent.Remove(typeof(T));
        }

        return true;
    }

    public bool TryGetComponent<T>(int entityId, [NotNullWhen(true)] out T? component)
        where T : class
    {
        if (!_componentsByEntity.TryGetValue(entityId, out Dictionary<Type, object>? components))
        {
            component = null;
            return false;
        }

        bool exists = components.TryGetValue(typeof(T), out object? value);
        if (!exists)
        {
            component = null;
            return false;
        }

        component = (T)value!;
        return true;
    }
}
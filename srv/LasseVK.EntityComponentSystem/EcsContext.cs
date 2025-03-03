using System.Diagnostics.CodeAnalysis;

namespace LasseVK.EntityComponentSystem;

public class EcsContext
{
    private int _nextId;

    private readonly Dictionary<Type, HashSet<int>> _entitiesByComponent = new();
    private readonly Dictionary<int, Dictionary<Type, object>> _componentsByEntity = new();

    private readonly Dictionary<Type, List<EcsSystem>> _systemsByComponent = new();
    private readonly Dictionary<EcsSystem, Type> _componentsBySystem = new();

    public EcsEntity CreateEntity() => new(this, _nextId++);

    public EcsSystem CreateSystem<T>()
    {
        var system = new EcsSystem(this);
        if (!_systemsByComponent.TryGetValue(typeof(T), out List<EcsSystem>? systems))
        {
            systems = new List<EcsSystem>();
            _systemsByComponent.Add(typeof(T), systems);
        }

        if (_entitiesByComponent.TryGetValue(typeof(T), out HashSet<int>? entities))
        {
            foreach (int entityId in entities)
            {
                system.AddEntity(entityId);
            }
        }

        _componentsBySystem[system] = typeof(T);
        systems.Add(system);
        return system;
    }

    public void RemoveSystem(EcsSystem ecsSystem)
    {
        if (!_componentsBySystem.TryGetValue(ecsSystem, out Type? componentType))
        {
            return;
        }

        _systemsByComponent[componentType].Remove(ecsSystem);
        _componentsBySystem.Remove(ecsSystem);
    }

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
                NotifySystems<T>(system => system.AddEntity(entityId));
                break;

            case false:
                components[typeof(T)] = component;
                break;
        }
    }

    private void NotifySystems<T>(Action<EcsSystem> notify)
    {
        if (!_systemsByComponent.TryGetValue(typeof(T), out List<EcsSystem>? systems))
        {
            return;
        }

        foreach (EcsSystem system in systems)
        {
            notify(system);
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

        NotifySystems<T>(system => system.RemoveEntity(entityId));

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
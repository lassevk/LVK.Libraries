using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LVK.EntityComponentSystem;

public class EcsContext
{
    private int _nextId;

    private readonly Dictionary<Type, HashSet<int>> _entitiesByComponent = new();
    private readonly Dictionary<int, Dictionary<Type, object>> _componentsByEntity = new();

    private readonly Dictionary<Type, List<EcsSystem>> _systemsByComponent = new();
    private readonly Dictionary<EcsSystem, Type> _componentsBySystem = new();

    private readonly Dictionary<Type, PropertyInfo[]> _componentsProperties = new();

    public EcsEntity CreateEntity() => new(this, _nextId++);

    public void RemoveEntity(EcsEntity entity)
    {
        if (!_componentsByEntity.TryGetValue(entity.Id, out Dictionary<Type, object>? components))
        {
            return;
        }

        foreach (KeyValuePair<Type, object> kvp in components)
        {
            TryRemoveComponent(entity.Id, kvp.Key);
        }
    }

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

    private void SetComponent(int entityId, object component, Type componentType)
    {
        if (!_componentsByEntity.TryGetValue(entityId, out Dictionary<Type, object>? components))
        {
            components = [];
            _componentsByEntity.Add(entityId, components);
        }

        if (!_entitiesByComponent.TryGetValue(componentType, out HashSet<int>? entities))
        {
            entities = new HashSet<int>();
            _entitiesByComponent.Add(componentType, entities);
        }

        _entitiesByComponent[componentType].Add(entityId);
        bool wasAdded = components.TryAdd(componentType, component);
        switch (wasAdded)
        {
            case true:
                NotifySystems(componentType, system => system.AddEntity(entityId));
                break;

            case false:
                components[componentType] = component;
                break;
        }
    }

    internal void SetComponent<T>(int entityId, T component)
        where T : class
    {
        SetComponent(entityId, component, typeof(T));
    }

    private void NotifySystems(Type componentType, Action<EcsSystem> notify)
    {
        if (!_systemsByComponent.TryGetValue(componentType, out List<EcsSystem>? systems))
        {
            return;
        }

        foreach (EcsSystem system in systems)
        {
            notify(system);
        }
    }

    private bool TryRemoveComponent(int entityId, Type componentType)
    {
        if (!_componentsByEntity.TryGetValue(entityId, out Dictionary<Type, object>? components))
        {
            return false;
        }

        components.Remove(componentType);
        if (components.Count == 0)
        {
            _componentsByEntity.Remove(entityId);
        }

        HashSet<int> entitiesByComponent = _entitiesByComponent[componentType];
        entitiesByComponent.Remove(entityId);
        if (entitiesByComponent.Count == 0)
        {
            _entitiesByComponent.Remove(componentType);
        }

        NotifySystems(componentType, system => system.RemoveEntity(entityId));

        return true;
    }

    internal bool TryRemoveComponent<T>(int entityId)
        where T : class
        => TryRemoveComponent(entityId, typeof(T));

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

    public void SetComponents<T>(int entityId, T components)
        where T: notnull
    {
        PropertyInfo[] properties = GetProperties(components.GetType());
        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(components);
            if (value is not null)
            {
                SetComponent(entityId, value, property.PropertyType);
            }
            else
            {
                TryRemoveComponent(entityId, property.PropertyType);
            }
        }
    }

    private PropertyInfo[] GetProperties(Type type)
    {
        if (_componentsProperties.TryGetValue(type, out PropertyInfo[]? properties))
        {
            return properties;
        }

        properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            if (!property.CanRead)
            {
                throw new InvalidOperationException("All properties on components object must be readable");
            }

            if (!property.PropertyType.IsClass)
            {
                throw new InvalidOperationException("All properties on components object must be reference types (classes)");
            }

            if (property.GetIndexParameters().Length > 0)
            {
                throw new InvalidOperationException("All properties on components object must be non-indexed");
            }
        }

        _componentsProperties.Add(type, properties);
        return properties;
    }
}
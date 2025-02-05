using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace LasseVK.Jobs;

public static class JobSerializer
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions;
    private static readonly Dictionary<Type, PropertyInfo[]> _dependencyProperties = new();

    static JobSerializer()
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    item =>
                    {
                        if (item.Type == typeof(Job))
                        {
                            // Ensure we can serialize descendants without having explicit polymorphism attributes in pplace
                            item.PolymorphismOptions = new JsonPolymorphismOptions
                            {
                                TypeDiscriminatorPropertyName = "$type", IgnoreUnrecognizedTypeDiscriminators = true, UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                            };

                            IEnumerable<Type> derivedTypes =
                                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                from type in assembly.GetTypes()
                                where type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Job))
                                select type;

                            foreach (Type derivedType in derivedTypes)
                            {
                                GetIdentifier(derivedType, out string identifier);
                                item.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(derivedType, identifier));
                            }

                            item.Properties.Remove(item.Properties.First(p => p.Name == "id"));
                        }
                        else if (item.Type.IsSubclassOf(typeof(Job)))
                        {
                            item.Properties.Remove(item.Properties.First(p => p.Name == "id"));

                            // Remove job dependencies from serialized object
                            foreach (JsonPropertyInfo property in item.Properties.ToArray())
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(Job)))
                                {
                                    item.Properties.Remove(property);
                                }
                                else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)
                                                                             && property.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(Job)))
                                {
                                    item.Properties.Remove(property);
                                }
                            }
                        }
                    },
                },
            },
        };
    }

    public static SerializedJob Serialize(Job job)
    {
        string json = JsonSerializer.Serialize(job, _jsonSerializerOptions);
        return new SerializedJob
        {
            Json = json, Group = job.Group,
        };
    }

    private static void GetIdentifier(Type jobType, out string identifier)
    {
        if (jobType.GetCustomAttribute<JobIdentifierAttribute>() is { } identifierAttribute)
        {
            identifier = identifierAttribute.Identifier;
            return;
        }

        identifier = jobType.FullName!;
    }

    public static Job Deserialize(SerializedJob envelopeJob, string jobId)
    {
        Job? job = JsonSerializer.Deserialize<Job>(envelopeJob.Json, _jsonSerializerOptions);
        if (job is null)
        {
            throw new InvalidOperationException("Could not deserialize job");
        }

        job.Id = jobId;
        return job;
    }

    public static List<Job> GetDependencies(Job job)
    {
        var result = new List<Job>();

        PropertyInfo[] properties = GetDependencyProperties(job.GetType());
        foreach (PropertyInfo property in properties)
        {
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                if (property.GetValue(job) is IEnumerable<Job> collection)
                {
                    result.AddRange(collection);
                }
            }
            else
            {
                if (property.GetValue(job) is Job dependency)
                {
                    result.Add(dependency);
                }
            }
        }

        return result;
    }

    public static void PopulateDependencies(Job job, List<Job> dependencies)
    {
        ILookup<Type, Job> lookup = dependencies.ToLookup(x => x.GetType());

        PropertyInfo[] properties = GetDependencyProperties(job.GetType());
        foreach (PropertyInfo property in properties)
        {
            if (property.PropertyType.IsSubclassOf(typeof(Job)))
            {
                Job? dependency = lookup[property.PropertyType].SingleOrDefault();

                if (dependency is null)
                {
                    throw new InvalidOperationException($"Could not find dependency for property {property.Name}");
                }

                property.SetValue(job, dependency);
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)
                                                         && property.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(Job)))
            {
                var propertyDependencies = lookup[property.PropertyType.GetGenericArguments()[0]].ToList();
                object? listValue = property.GetValue(job);
                if (propertyDependencies.Count > 0)
                {
                    if (listValue is null)
                    {
                        throw new InvalidCastException($"Unable to populate dependencies for {property.Name}, no list instance");
                    }

                    foreach (Job dependency in propertyDependencies)
                    {
                        ((dynamic)listValue).Add((dynamic)dependency);
                    }
                }
            }
        }
    }

    private static PropertyInfo[] GetDependencyProperties(Type jobType)
    {
        if (_dependencyProperties.TryGetValue(jobType, out PropertyInfo[]? properties))
        {
            return properties;
        }

        var temp = new List<PropertyInfo>();
        foreach (PropertyInfo property in jobType.GetProperties())
        {
            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            if (property.PropertyType.IsSubclassOf(typeof(Job)) && property is { CanRead: true, CanWrite: true })
            {
                temp.Add(property);
            }
            else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>)
                                                         && property.PropertyType.GetGenericArguments()[0].IsSubclassOf(typeof(Job)))
            {
                temp.Add(property);
            }
        }

        properties = temp.ToArray();
        _dependencyProperties[jobType] = properties;
        return properties;
    }
}
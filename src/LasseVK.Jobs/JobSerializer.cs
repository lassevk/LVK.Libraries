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
                Modifiers = {
                    item =>
                    {
                        if (item.Type == typeof(Job))
                        {
                            // Ensure we can serialize descendants without having explicit polymorphism attributes in pplace
                            item.PolymorphismOptions = new JsonPolymorphismOptions
                            {
                                TypeDiscriminatorPropertyName = "$type",
                                IgnoreUnrecognizedTypeDiscriminators = true,
                                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
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
                        }
                        else if (item.Type.IsSubclassOf(typeof(Job)))
                        {
                            // Remove job dependencies from serialized object
                            foreach (JsonPropertyInfo property in item.Properties.ToArray())
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(Job)))
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
        GetIdentifier(job.GetType(), out string identifier);

        string json = JsonSerializer.Serialize(job, _jsonSerializerOptions);
        return new SerializedJob { Json = json, Identifier = identifier, Group = job.Group };
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

    public static void PopulateDependencies(Job job, List<Job> dependencies)
    {
        var lookup = dependencies.ToDictionary(x => x.GetType());

        PropertyInfo[] properties = GetDependencyProperties(job.GetType());
        foreach (PropertyInfo property in properties)
        {
            if (!lookup.TryGetValue(property.PropertyType, out Job? dependency))
            {
                throw new InvalidOperationException($"Could not find dependency for property {property.Name}");
            }

            property.SetValue(job, dependency);
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
            if (property is not { CanRead: true, CanWrite: true })
            {
                continue;
            }

            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            if (!property.PropertyType.IsSubclassOf(typeof(Job)))
            {
                continue;
            }

            temp.Add(property);
        }

        properties = temp.ToArray();
        _dependencyProperties[jobType] = properties;
        return properties;
    }
}
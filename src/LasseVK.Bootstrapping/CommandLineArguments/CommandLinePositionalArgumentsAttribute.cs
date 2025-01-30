using JetBrains.Annotations;

namespace LasseVK.Bootstrapping.CommandLineArguments;

[PublicAPI]
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class CommandLinePositionalArgumentsAttribute : Attribute;
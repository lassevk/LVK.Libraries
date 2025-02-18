using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace LasseVK.Bootstrapping.CommandLineArguments;

public static class CommandLineArgumentsHelper
{
    public static T CreateArguments<T>(IServiceProvider serviceProvider)
        where T : class, new()
    {
        var commandLineArguments = new T();
        string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();

        ICommandLineArgumentProperty? lastProperty = null;
        bool restIsPositional = false;

        foreach (string arg in args)
        {
            if (arg.StartsWith('-') && !restIsPositional)
            {
                int equalIndex = arg.IndexOf('=');
                if (equalIndex != -1)
                {
                    string option = arg[..equalIndex];
                    string argument = arg[(equalIndex + 1)..];
                    if (!processArg(option))
                    {
                        return commandLineArguments;
                    }

                    if (!processArg(argument))
                    {
                        return commandLineArguments;
                    }
                }
                else if (arg.EndsWith('+') || (arg.EndsWith('-') && arg != "--"))
                {
                    if (!processArg(arg[..^1]))
                    {
                        return commandLineArguments;
                    }

                    if (!processArg(arg[^1..]))
                    {
                        return commandLineArguments;
                    }
                }
                else
                {
                    if (!processArg(arg))
                    {
                        return commandLineArguments;
                    }
                }
            }
            else
            {
                if (!processArg(arg))
                {
                    return commandLineArguments;
                }
            }
        }

        lastProperty?.ValidateEnd();
        return commandLineArguments;

        bool processArg(string arg)
        {
            if (arg == "--" && !restIsPositional)
            {
                restIsPositional = true;
                lastProperty = null;
                return true;
            }

            if (arg.StartsWith('-') && !restIsPositional)
            {
                if (!(lastProperty?.ValidateEnd() ?? true))
                {
                    return false;
                }

                (bool success, lastProperty) = HandleCommandLineOption(commandLineArguments, arg);
                return success;
            }

            if (lastProperty == null)
            {
                return HandleOverflowArgument(commandLineArguments, arg);
            }

            (bool finalSuccess, lastProperty) = lastProperty.HandleArgument(arg);
            return finalSuccess;
        }
    }

    private static bool HandleOverflowArgument(object commandLineArguments, string arg)
    {
        foreach (PropertyInfo property in commandLineArguments.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (property.GetCustomAttribute<CommandLinePositionalArgumentsAttribute>() != null)
            {
                if (property.GetValue(commandLineArguments) is not ICollection<string> instance)
                {
                    throw new InvalidOperationException("CommandLinePositionalArgumentsAttribute has to be applied to a property containing a string collection that can be added to");
                }

                instance.Add(arg);
                return true;
            }
        }

        Console.Error.WriteLine("This application does not support positional arguments on the command line");
        return false;
    }

    private static (bool success, ICommandLineArgumentProperty? lastProperty) HandleCommandLineOption(object commandLineArguments, string arg)
    {
        PropertyInfo? property = (
            from propertyInfo in commandLineArguments.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            from attribute in propertyInfo.GetCustomAttributes<CommandLineOptionAttribute>()
            where attribute.Option == arg
            select propertyInfo).FirstOrDefault();

        if (property == null)
        {
            Console.Error.WriteLine($"This application does not have an option named '{arg}'");
            return (false, null);
        }

        ICommandLineArgumentProperty? handler = GetOptionHandler(property, arg, commandLineArguments);
        if (handler != null)
        {
            return (true, handler);
        }

        throw new InvalidOperationException("The CommandLineOptionAttribute for '{arg}' option is attached to an invalid property type");
    }

    public static IEnumerable<string> GetHelp<T>(T commandLineArguments)
        where T : class
    {
        bool hasPositionalArguments = commandLineArguments.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
           .Any(property => property.GetCustomAttribute<CommandLinePositionalArgumentsAttribute>() != null);

        if (hasPositionalArguments)
        {
            yield return $"help: {Process.GetCurrentProcess().ProcessName} [options] <arguments>";
        }
        else
        {
            yield return $"help: {Process.GetCurrentProcess().ProcessName} [options]";
        }

        IEnumerable<(PropertyInfo property, string[] options, string description, ICommandLineArgumentProperty handler)> propertyOptions
            =
            from property in commandLineArguments.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            let options = property.GetCustomAttributes<CommandLineOptionAttribute>().Select(cloa => cloa.Option).ToArray()
            where options.Any()
            let handler = GetOptionHandler(property, options.First(), commandLineArguments)
            where handler is not null
            let description = property.GetCustomAttribute<DescriptionAttribute>()?.Description
            orderby options.FirstOrDefault()?.TrimStart('-')
            select (property, options, description, handler);

        foreach ((PropertyInfo property, string[] options, string description, ICommandLineArgumentProperty handler) propertyOption in propertyOptions)
        {
            yield return "";

            string? argumentsHelp = propertyOption.handler.GetArgumentHelp();

            foreach (string option in propertyOption.options.OrderBy(o => o.Length - o.TrimStart('-').Length))
            {
                yield return $"   {option} {argumentsHelp}".TrimEnd();
            }

            if (!string.IsNullOrWhiteSpace(propertyOption.description))
            {
                yield return $"      {propertyOption.description}";
            }

            bool first = true;
            foreach (string line in propertyOption.handler.GetHelpLines())
            {
                if (first)
                {
                    yield return "";

                    first = false;
                }

                yield return $"      {line}";
            }
        }

        if (hasPositionalArguments)
        {
            yield return "";
            yield return "   <arguments>";
            yield return "      positional arguments that are not tied to any specific option";
            yield return "      if you use the specific separator '--', the remaining arguments will be treated as positional, even if they start with a dash";
        }
    }

    private static ICommandLineArgumentProperty? GetOptionHandler(PropertyInfo propertyInfo, string option, object commandLineArguments)
    {
        if (propertyInfo.PropertyType == typeof(string))
        {
            return new StringCommandLineArgumentProperty(option, propertyInfo, commandLineArguments);
        }

        if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
        {
            return new BooleanCommandLineArgumentProperty(propertyInfo, commandLineArguments);
        }

        if (propertyInfo.GetValue(commandLineArguments) is ICollection<string> stringCollection)
        {
            return new StringCollectionCommandLineArgumentProperty(option, stringCollection);
        }

        return null;
    }
}
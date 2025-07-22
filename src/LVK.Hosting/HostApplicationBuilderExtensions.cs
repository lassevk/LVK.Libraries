using System.Diagnostics;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace LVK.Hosting;

[PublicAPI]
public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddStandardConfigurationSources<TProgram>(this IHostApplicationBuilder builder)
        where TProgram : class
    {
        IConfigurationSource[] beforeJson = builder.Configuration.Sources.TakeWhile(source => source is not JsonConfigurationSource).ToArray();
        IConfigurationSource[] afterJson = builder.Configuration.Sources.SkipWhile(source => source is not JsonConfigurationSource).SkipWhile(source => source is JsonConfigurationSource).ToArray();
        builder.Configuration.Sources.Clear();
        foreach (IConfigurationSource source in beforeJson)
        {
            builder.Configuration.Sources.Add(source);
        }

        string environmentName = builder.Environment.EnvironmentName;

        builder.Configuration.SetBasePath(Path.GetDirectoryName(typeof(TProgram).Assembly.Location)!);
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{Environment.MachineName}.{environmentName}.json", optional: true, reloadOnChange: true);

        foreach (IConfigurationSource source in afterJson)
        {
            builder.Configuration.Sources.Add(source);
        }

        if (builder.Environment.IsDevelopment() || Debugger.IsAttached)
        {
            builder.Configuration.AddUserSecrets<TProgram>();
        }

        return builder;
    }
}
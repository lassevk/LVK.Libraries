using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Jobs;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddJobManager(this IHostApplicationBuilder builder, Action<JobManagerConfiguration> configure)
    {
        var configuration = new JobManagerConfiguration { Builder = builder };
        builder.Configuration.GetSection(JobManagerConfiguration.ConfigurationKey).Bind(configuration.Options);
        configure(configuration);

        builder.Services.AddSingleton<IJobManager, JobManager>();
        builder.Services.AddSingleton(configuration.Options);
        builder.Services.AddSingleton<IJobStorage>(sp => configuration.JobStorageFactory(sp));

        builder.Services.AddScoped<JobLogger>();
        builder.Services.AddScoped<IJobLogger>(sp => sp.GetRequiredService<JobLogger>());

        return builder;
    }
}
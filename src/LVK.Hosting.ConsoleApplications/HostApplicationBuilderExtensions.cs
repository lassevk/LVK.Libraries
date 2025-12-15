using JetBrains.Annotations;

using LVK.Hosting.ConsoleApplications.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Hosting.ConsoleApplications;

[PublicAPI]
public static class HostApplicationBuilderExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddConsoleApplication<T>(Action<T>? configure = null)
            where T : class, IConsoleApplication
        {
            builder.Services.AddHostedService<ConsoleApplicationHostedService>();
            builder.Services.Configure<ConsoleApplicationHostedServiceOptions>(options => options.SetConsoleApplication(configure));
            return builder;
        }

        public IHostApplicationBuilder AddConsoleCommand<T>(Action<T>? configure = null)
            where T : class, IConsoleApplication
        {
            builder.Services.AddHostedService<ConsoleApplicationHostedService>();
            builder.Services.Configure<ConsoleApplicationHostedServiceOptions>(options => options.AddCommand(configure));
            return builder;
        }

        public IHostApplicationBuilder AddConsoleCommands<TProgram>()
            where TProgram : class
        {
            builder.Services.AddHostedService<ConsoleApplicationHostedService>();

            foreach (Type type in typeof(TProgram).Assembly.GetTypes())
            {
                if (type.IsAssignableTo(typeof(IConsoleApplication)) && !type.IsAbstract)
                {
                    builder.Services.Configure<ConsoleApplicationHostedServiceOptions>(options => options.AddCommand(type));
                }
            }

            return builder;
        }
    }
}
using LasseVK.Bootstrapping;

using Microsoft.Extensions.Hosting;

using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Bootstrap(new ModuleBootstrapper());

IHost host = builder.Build();

await host.InitializeAsync();
await host.RunAsConsoleApplicationAsync<Application>();
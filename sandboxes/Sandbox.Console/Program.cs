using LasseVK.Bootstrapping;
using LasseVK.Configuration;
using LasseVK.Jobs;
using LasseVK.Jobs.PostgreSQL;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddStandardConfigurationSources<Program>();

builder.Services.AddJobHandlers<Program>();

IHost host = builder.Build();

await host.InitializeAsync();
await host.RunAsConsoleApplicationAsync<Application>();
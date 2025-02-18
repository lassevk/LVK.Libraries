using LasseVK.Bootstrapping;
using LasseVK.Configuration;
using LasseVK.Jobs;
using LasseVK.Pushover;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddStandardConfigurationSources<Program>();

builder.Services.AddJobHandlers<Program>();
builder.Services.AddPushoverClient(configure =>
{
    builder.Configuration.GetSection(PushoverNotificationOptions.SectionName).Bind(configure);
});

IHost host = builder.Build();

await host.InitializeAsync();
await host.RunAsConsoleApplicationAsync<Application>();
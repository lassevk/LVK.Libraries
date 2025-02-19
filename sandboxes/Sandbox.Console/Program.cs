using LasseVK.Bootstrapping;
using LasseVK.Configuration;
using LasseVK.Jobs;
using LasseVK.Pushover;

using Microsoft.Extensions.Hosting;
using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddStandardConfigurationSources<Program>();
builder.Services.AddConsoleApplication<Application>();

builder.Services.AddJobHandlers<Program>();
builder.Services.AddPushoverClient(builder.Configuration.GetSection(PushoverNotificationOptions.SectionName));

IHost host = builder.Build();

await host.InitializeAsync();
await host.RunAsync();
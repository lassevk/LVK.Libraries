using LasseVK.Jobs;

using LVK.Bootstrapping;
using LVK.Configuration;
using LVK.RazorTemplates;

using Microsoft.Extensions.Hosting;
using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddStandardConfigurationSources<Program>();
builder.Services.AddConsoleApplication<Application>();
builder.Services.AddRazorRenderer();

IHost host = builder.Build();

await host.InitializeAsync();
await host.RunAsync();
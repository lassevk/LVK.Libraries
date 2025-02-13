using LasseVK.Bootstrapping;
using LasseVK.Ssh;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddConsoleApplication<Application>();
builder.AddSshProxy();

IHost host = builder.Build();

await host.RunAsyncEx();
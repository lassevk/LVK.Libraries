using LasseVK.Bootstrapping;
using LasseVK.Jobs;
using LasseVK.Jobs.PostgreSQL;
using LasseVK.Pushover;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddJobHandlers<Program>();
builder.Services.AddPushoverClient(configure =>
{
    builder.Configuration.GetSection(PushoverNotificationOptions.SectionName).Bind(configure);
});

string connectionString = builder.Configuration.GetConnectionString("Jobs") ?? throw new InvalidOperationException("No jobs connection string");
builder.AddJobManager(configuration =>
{
    configuration.UsePostgreSql(connectionString);
});

IHost host = builder.Build();

await host.InitializeAsync();
await host.RunAsConsoleApplicationAsync<Application>();
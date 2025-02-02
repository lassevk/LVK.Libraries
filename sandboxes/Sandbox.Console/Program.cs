using LasseVK.Bootstrapping;

using Microsoft.Extensions.Hosting;
using Sandbox.Console;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Bootstrap(new Sandbox.Console.ModuleBootstrapper());

IHost host = builder.Build();

foreach (string filePath in Directory.GetFiles(@"D:\Temp", "*.checksum"))
{
    File.Delete(filePath);
}

await host.InitializeAsync();
await host.RunAsConsoleApplicationAsync<Application>();
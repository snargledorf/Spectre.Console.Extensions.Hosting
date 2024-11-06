# SpectreConsoleHost

A Generic Host builder and extensions for [Spectre.Console](https://spectreconsole.net/)

[Examples](https://github.com/snargledorf/SpectreConsoleHost.Examples)

## Extension Methods

Calling the `AddSpectreConsole` service collection extension methods will add a Spectre.Console command app to be executed.

The extension methods can be called multiple times to add multiple apps to be executed, although this is generally not recommended.

The application will exit once all the added command apps complete execution with the exit code being set by the last command app to finish execution.

### Basic Usage

#### No default command
```c#
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSpectreConsole(args, configurator =>
{
    configurator.AddCommand<ExampleCommand>("example");
});

await builder.Build().RunAsync();
```

#### Default command
Without additional configuration
```c#
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSpectreConsole<DefaultCommand>(args);

await builder.Build().RunAsync();
```
With additional configuration
```c#
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSpectreConsole<DefaultCommand>(args, configurator =>
{
    configurator.AddCommand<ExampleCommand>("example");
});

await builder.Build().RunAsync();
```

## Host Builder

The host builder adds a command app with the configuration from the builders `Configurator` property.

### Basic usage

#### No default command
```c#
SpectreConsoleHostBuilder builder = SpectreConsoleHost.CreateBuilder(args);

builder.Configurator.AddCommand<SomeCommand>("somecommand");

await builder.Build().RunAsync();
```

#### Default command
```c#
SpectreConsoleHostBuilder builder = SpectreConsoleHost.CreateBuilder<DefaultCommand>(args);

builder.Configurator.AddCommand<SomeCommand>("somecommand");

await builder.Build().RunAsync();
```
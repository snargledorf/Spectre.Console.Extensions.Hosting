# SpectreConsoleHost

![NuGet Version](https://img.shields.io/nuget/v/SpectreConsoleHost)

A Generic Host builder for Spectre.Console

[Examples](https://github.com/snargledorf/SpectreConsoleHost.Examples)

## Basic Usage

### No default command
```c#
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using Spectre.Console.Builder;

SpectreConsoleHostBuilder builder = SpectreConsoleHost.CreateBuilder(args);

builder.Configurator.AddCommand<SomeCommand>("somecommand");

await builder.Build().RunAsync();
```

### Default command
```c#
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using Spectre.Console.Builder;

SpectreConsoleHostBuilder builder = SpectreConsoleHost.CreateBuilder<DefaultCommand>(args);

await builder.Build().RunAsync();
```
# SpectreConsoleHost

![NuGet Version](https://img.shields.io/nuget/v/SpectreConsoleHost?link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FSpectreConsoleHost)

A Generic Host builder for [Spectre.Console](https://spectreconsole.net/)

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

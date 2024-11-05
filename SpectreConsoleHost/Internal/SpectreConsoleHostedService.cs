using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostedService : IHostedService
    {
        private int _exitCode;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IOptions<SpectreConsoleHostedServiceOptions> _options;
        private readonly ILogger<SpectreConsoleHostedService> _logger;

        public SpectreConsoleHostedService(IHostApplicationLifetime applicationLifetime,
            IOptions<SpectreConsoleHostedServiceOptions> options,
            ILogger<SpectreConsoleHostedService> logger)
        {
            _applicationLifetime = applicationLifetime;
            _options = options;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = RunCommandAppAsync();

            _logger.LogDebug("Spectre Console Hosted service is started.");

            return Task.CompletedTask;
        }

        private async Task RunCommandAppAsync()
        {
            ICommandApp commandApp = _options.Value.CommandApp;
            if (commandApp is null)
                throw new InvalidOperationException("CommandApp must be specified.");

            IEnumerable<string> args = _options.Value.Args;
            if (args is null)
                throw new InvalidOperationException("Args must be specified.");

            try
            {
                _logger.LogDebug("Running command app");
                _exitCode = await commandApp.RunAsync(args);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                _logger.LogError(ex, "Unhandled exception");
                _exitCode = -1;
            }
            finally
            {
                _logger.LogDebug("Stopping application");
                _applicationLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Setting exit code: {exitCode}", _exitCode);

            Environment.ExitCode = _exitCode;

            return Task.CompletedTask;
        }
    }
}
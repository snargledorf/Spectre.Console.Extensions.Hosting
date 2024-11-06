using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Spectre.Console.Extensions.Hosting.Internal
{
    internal class SpectreConsoleHostedService : IHostedService
    {
        private int _exitCode;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IEnumerable<ExecuteCommandAppDelegate> _commandAppExecuteDelegates;
        private readonly ILogger<SpectreConsoleHostedService> _logger;

        public SpectreConsoleHostedService(IHostApplicationLifetime applicationLifetime,
            IEnumerable<ExecuteCommandAppDelegate> commandAppExecuteDelegates,
            ILogger<SpectreConsoleHostedService> logger)
        {
            _applicationLifetime = applicationLifetime;
            _commandAppExecuteDelegates = commandAppExecuteDelegates;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = RunCommandAppsAsync();

            _logger.LogDebug("Spectre Console Hosted service is started.");

            return Task.CompletedTask;
        }

        private async Task RunCommandAppsAsync()
        {
            try
            {
                IEnumerable<Task> tasks = _commandAppExecuteDelegates.Select(RunCommandAppAsync);
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            finally
            {
                _applicationLifetime.StopApplication();
            }
        }

        private async Task RunCommandAppAsync(ExecuteCommandAppDelegate executeCommandAppDelegate)
        {
            try
            {
                _logger.LogDebug("Running command app");
                _exitCode = await executeCommandAppDelegate().ConfigureAwait(false);
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                _logger.LogError(ex, "Unhandled exception");
                _exitCode = -1;
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
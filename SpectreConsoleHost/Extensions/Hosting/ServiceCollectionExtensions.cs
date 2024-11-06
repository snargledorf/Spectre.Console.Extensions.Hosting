using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Spectre.Console.Extensions.Hosting.Internal;

namespace Spectre.Console.Extensions.Hosting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSpectreConsole(this IServiceCollection services, IEnumerable<string> args,
            Action<IConfigurator> configureApp)
        {
            if (configureApp is null)
                throw new ArgumentNullException(nameof(configureApp));
            
            return services.AddSpectreConsole(args, tr => new CommandApp(tr), configureApp);
        }

        public static IServiceCollection AddSpectreConsole<TDefaultCommand>(this IServiceCollection services,
            IEnumerable<string> args, Action<IConfigurator> configureApp = null) where TDefaultCommand : class, ICommand
        {
            return services.AddSpectreConsole(args, tr => new CommandApp<TDefaultCommand>(tr), configureApp);
        }
    }
}
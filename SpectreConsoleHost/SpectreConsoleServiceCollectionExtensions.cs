using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Builder.Internal;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder
{
    public static class SpectreConsoleServiceCollectionExtensions
    {
        public static IServiceCollection AddSpectreConsole(this IServiceCollection services, IEnumerable<string> args,
            Action<IConfigurator> configureApp)
        {
            if (configureApp is null)
                throw new ArgumentNullException(nameof(configureApp));
            
            return services.AddSpectreConsoleInternal(args, tr => new CommandApp(tr), configureApp);
        }

        public static IServiceCollection AddSpectreConsole<TDefaultCommand>(this IServiceCollection services,
            IEnumerable<string> args, Action<IConfigurator> configureApp = null) where TDefaultCommand : class, ICommand
        {
            return services.AddSpectreConsoleInternal(args, tr => new CommandApp<TDefaultCommand>(tr), configureApp);
        }

        internal static IServiceCollection AddSpectreConsoleInternal(this IServiceCollection services, IEnumerable<string> args,
            Func<ITypeRegistrar, ICommandApp> commandAppFactory, Action<IConfigurator> configureApp = null)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            IServiceProvider serviceProvider = null;
            var tr = new SpectreConsoleHostTypeRegistrar(() => serviceProvider ?? throw new InvalidOperationException("ServiceProvider not built"));
            
            ICommandApp commandApp = commandAppFactory(tr);
            
            return services.AddSpectreConsoleInternal(args, commandApp, configureApp, sp => serviceProvider = sp);
        }
        
        internal static IServiceCollection AddSpectreConsoleInternal(this IServiceCollection services,
            IEnumerable<string> args, ICommandApp commandApp, Action<IConfigurator> configure, Action<IServiceProvider> provideServiceProvider = null)
        {
            services.AddHostedService<SpectreConsoleHostedService>();

            services.Configure<SpectreConsoleHostedServiceOptions>(options =>
            {
                options.Args = args;
                if (configure != null)
                    commandApp.Configure(configure);
            });
            
            services.AddSingleton(sp =>
            {
                provideServiceProvider?.Invoke(sp);
                return commandApp;
            });
            
            return services;
        }
    }
}
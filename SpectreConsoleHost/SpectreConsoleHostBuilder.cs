using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console.Builder.Internal;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder
{
    internal class SpectreConsoleHostBuilder<TDefaultCommand> : SpectreConsoleHostBuilder where TDefaultCommand : class, ICommand
    {
        public SpectreConsoleHostBuilder(params string[] args) : base(args)
        {
        }

        protected override ICommandApp CommandApp => new CommandApp<TDefaultCommand>(TypeRegistrar);
    }

    public class SpectreConsoleHostBuilder : IHostApplicationBuilder, IServicesProvider
    {
        private readonly HostApplicationBuilder _builder;
        private CommandApp _commandApp;

        private IHost _builtHost;
    
        private SpectreConsoleHostConfigurator _configurator;
        private SpectreConsoleHostTypeRegistrar _typeRegistrar;

        internal SpectreConsoleHostBuilder(params string[] args)
        {
            _builder = Host.CreateApplicationBuilder(args);

            Services.AddHostedService<SpectreConsoleHostedService>();

            Services.Configure<SpectreConsoleHostedServiceOptions>(options =>
            {
                if (_configurator is SpectreConsoleHostConfigurator configurator)
                    CommandApp.Configure(configurator.Configure);
            
                options.CommandApp = CommandApp;
                options.Args = args;
            });
        }

        protected ITypeRegistrar TypeRegistrar => _typeRegistrar = _typeRegistrar ?? new SpectreConsoleHostTypeRegistrar(this);
    
        protected virtual ICommandApp CommandApp => _commandApp = _commandApp ?? new CommandApp(TypeRegistrar);

        void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(
            IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder> configure) =>
            _builder.ConfigureContainer(factory, configure);

        IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_builder).Properties;

        public ConfigurationManager Configuration => _builder.Configuration;
        IConfigurationManager IHostApplicationBuilder.Configuration => Configuration;

        public IHostEnvironment Environment => _builder.Environment;

        public ILoggingBuilder Logging => _builder.Logging;

        public IMetricsBuilder Metrics => _builder.Metrics;

        public IServiceCollection Services => _builder.Services;

        public IConfigurator Configurator => _configurator = _configurator ?? new SpectreConsoleHostConfigurator(TypeRegistrar);

        public IHost Build()
        {
            return _builtHost = _builder.Build();
        }

        IServiceProvider IServicesProvider.Services
        {
            get
            {
                IHost builtHost = _builtHost;
                if (builtHost is null)
                    throw new InvalidOperationException("Host has not been built");
            
                return builtHost.Services;
            }
        }
    }
}
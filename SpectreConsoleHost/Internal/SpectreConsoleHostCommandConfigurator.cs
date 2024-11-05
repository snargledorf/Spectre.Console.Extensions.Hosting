using System;
using System.Collections.Generic;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostCommandConfigurator : ICommandConfigurator
    {
        private readonly List<Func<ICommandConfigurator, ICommandConfigurator>> _commandConfigureActions = new List<Func<ICommandConfigurator, ICommandConfigurator>>();
    
        public ICommandConfigurator WithExample(params string[] args)
        {
            _commandConfigureActions.Add(configurator => configurator.WithExample(args));
            return this;
        }

        public ICommandConfigurator WithAlias(string name)
        {
            _commandConfigureActions.Add(configurator => configurator.WithAlias(name));
            return this;
        }

        public ICommandConfigurator WithDescription(string description)
        {
            _commandConfigureActions.Add(configurator => configurator.WithDescription(description));
            return this;
        }

        public ICommandConfigurator WithData(object data)
        {
            _commandConfigureActions.Add(configurator => configurator.WithData(data));
            return this;
        }

        public ICommandConfigurator IsHidden()
        {
            _commandConfigureActions.Add(configurator => configurator.IsHidden());
            return this;
        }

        public void Configure(ICommandConfigurator commandConfigurator)
        {
            foreach (Func<ICommandConfigurator,ICommandConfigurator> commandConfigureAction in _commandConfigureActions)
                commandConfigureAction(commandConfigurator);
        }
    }
}
using System;
using System.Collections.Generic;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostBranchConfigurator : IBranchConfigurator
    {
        private readonly List<Func<IBranchConfigurator, IBranchConfigurator>> _configureActions = new List<Func<IBranchConfigurator, IBranchConfigurator>>();
        public IBranchConfigurator WithAlias(string name)
        {
            _configureActions.Add(configurator => configurator.WithAlias(name));
            return this;
        }

        public void Configure(IBranchConfigurator branchConfigurator)
        {
            foreach (Func<IBranchConfigurator,IBranchConfigurator> configureAction in _configureActions) 
                configureAction(branchConfigurator);
        }
    }
}
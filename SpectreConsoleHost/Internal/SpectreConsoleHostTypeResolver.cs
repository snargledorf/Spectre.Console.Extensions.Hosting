using System;
using System.Collections.ObjectModel;
using System.Linq;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostTypeResolver : ITypeResolver
    {
        private readonly ReadOnlyDictionary<Type, ReadOnlyCollection<Func<object>>> _serviceFactories;
        private readonly Func<IServiceProvider> _getServiceProvider;

        public SpectreConsoleHostTypeResolver(ReadOnlyDictionary<Type, ReadOnlyCollection<Func<object>>> serviceFactories, Func<IServiceProvider> getServiceProvider)
        {
            _serviceFactories = serviceFactories;
            _getServiceProvider = getServiceProvider;
        }

        public object Resolve(Type type)
        {
            if (type is null)
                return null;

            Func<object> typeFactory = _serviceFactories.GetValueOrDefault(type)?.LastOrDefault();
            return typeFactory is null ? _getServiceProvider().GetService(type) : typeFactory();
        }
    }
}
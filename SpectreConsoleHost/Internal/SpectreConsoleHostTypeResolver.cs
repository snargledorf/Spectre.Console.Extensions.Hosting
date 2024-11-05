using System;
using System.Collections.ObjectModel;
using System.Linq;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostTypeResolver : ITypeResolver
    {
        private readonly ReadOnlyDictionary<Type, ReadOnlyCollection<Func<object>>> _serviceFactories;
        private readonly IServicesProvider _servicesProvider;

        public SpectreConsoleHostTypeResolver(ReadOnlyDictionary<Type, ReadOnlyCollection<Func<object>>> serviceFactories, IServicesProvider servicesProvider)
        {
            _serviceFactories = serviceFactories;
            _servicesProvider = servicesProvider;
        }

        public object Resolve(Type type)
        {
            if (type is null)
                return null;

            Func<object> typeFactory = _serviceFactories.GetValueOrDefault(type)?.LastOrDefault();
            return typeFactory is null ? _servicesProvider.Services.GetService(type) : typeFactory();
        }
    }
}
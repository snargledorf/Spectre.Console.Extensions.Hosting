using System;
using System.Collections.ObjectModel;
using System.Linq;
using Spectre.Console.Cli;

namespace Spectre.Console.Extensions.Hosting.Internal
{
    internal class SpectreConsoleHostTypeResolver : ITypeResolver
    {
        private readonly ReadOnlyDictionary<Type, ReadOnlyCollection<Func<IServiceProvider, object>>> _serviceFactories;
        private readonly IServiceProvider _serviceProvider;

        public SpectreConsoleHostTypeResolver(ReadOnlyDictionary<Type, ReadOnlyCollection<Func<IServiceProvider, object>>> serviceFactories, IServiceProvider serviceProvider)
        {
            _serviceFactories = serviceFactories;
            _serviceProvider = serviceProvider;
        }

        public object Resolve(Type type)
        {
            if (type is null)
                return null;

            Func<IServiceProvider, object> typeFactory = _serviceFactories.GetValueOrDefault(type)?.LastOrDefault();
            return typeFactory is null ? _serviceProvider.GetService(type) : typeFactory(_serviceProvider);
        }
    }
}
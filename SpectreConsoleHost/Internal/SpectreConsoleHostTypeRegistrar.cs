﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostTypeRegistrar : ITypeRegistrar
    {
        private readonly Dictionary<Type, List<Func<object>>> _factories = new Dictionary<Type, List<Func<object>>>();
        private readonly IServicesProvider _servicesProvider;

        public SpectreConsoleHostTypeRegistrar(IServicesProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
        }

        public void Register(Type service, Type implementation)
        {
            GetFactoryList(service).Add(() =>
                ActivatorUtilities.GetServiceOrCreateInstance(_servicesProvider.Services, implementation));
        }

        public void RegisterInstance(Type service, object implementation)
        {
            GetFactoryList(service).Add(() => implementation);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            GetFactoryList(service).Add(factory);   
        }

        public ITypeResolver Build()
        {
            return new SpectreConsoleHostTypeResolver(
                _factories.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly()).AsReadOnly(),
                _servicesProvider);
        }

        private List<Func<object>> GetFactoryList(Type service)
        {
            List<Func<object>> factories = _factories.GetValueOrDefault(service);
            if (factories is null)
                _factories[service] = factories = new List<Func<object>>();
            return factories;
        }
    }
}
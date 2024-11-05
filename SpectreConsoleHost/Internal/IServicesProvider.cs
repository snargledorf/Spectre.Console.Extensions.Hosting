using System;

namespace Spectre.Console.Builder.Internal
{
    internal interface IServicesProvider
    {
        IServiceProvider Services { get; }
    }
}
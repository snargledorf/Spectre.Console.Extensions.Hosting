using System.Collections.Generic;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostedServiceOptions
    {
        public IEnumerable<string> Args { get; set; }
    }
}
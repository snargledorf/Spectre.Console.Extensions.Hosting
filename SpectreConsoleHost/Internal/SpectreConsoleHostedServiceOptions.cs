using System.Collections.Generic;
using Spectre.Console.Cli;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostedServiceOptions
    {
        public ICommandApp CommandApp { get; set; }
        public IEnumerable<string> Args { get; set; }
    }
}
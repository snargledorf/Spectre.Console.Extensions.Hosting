using System;
using System.Globalization;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace Spectre.Console.Builder.Internal
{
    internal class SpectreConsoleHostCommandAppSettings : ICommandAppSettings
    {
        public SpectreConsoleHostCommandAppSettings(ITypeRegistrar typeRegistrar)
        {
            Registrar = new SpectreConsoleHostTypeRegistrarFrontend(typeRegistrar);
        }

        public CultureInfo Culture { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public int MaximumIndirectExamples { get; set; }
        public bool ShowOptionDefaultValues { get; set; }
        public bool TrimTrailingPeriod { get; set; }
        public HelpProviderStyle HelpProviderStyles { get; set; }
        public IAnsiConsole Console { get; set; }
        public ICommandInterceptor Interceptor { get; set; }
        public ITypeRegistrarFrontend Registrar { get; }
        public CaseSensitivity CaseSensitivity { get; set; }
        public bool StrictParsing { get; set; }
        public bool ConvertFlagsToRemainingArguments { get; set; }
        public bool PropagateExceptions { get; set; }
        public bool ValidateExamples { get; set; }
        public Func<Exception, ITypeResolver, int> ExceptionHandler { get; set; }
    }
}
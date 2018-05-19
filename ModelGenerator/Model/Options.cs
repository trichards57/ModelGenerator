using CommandLine;
using JetBrains.Annotations;

namespace ModelGenerator.Model
{
    [UsedImplicitly]
    internal class Options
    {
        [Option('c', "csharp", HelpText = "Output CSharp Model")]
        public bool OutputCSharp { get; set; }

        [Value(1, HelpText = "Output Folder", Required = true, MetaName = "Output Folder")]
        public string OutputFolder { get; set; }

        [Option('t', "typescript", HelpText = "Output Typescript Model")]
        public bool OutputTypescript { get; set; }

        [Option('o', "components", HelpText = "Output Typescript Components Model")]
        public bool OutputTypescriptComponents { get; set; }

        [Value(0, HelpText = "Source Model File", Default = "model.xml", Required = true, MetaName = "Source Folder")]
        public string SourceFile { get; set; }
    }
}
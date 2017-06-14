﻿using CommandLine;

namespace ModelGenerator.Model
{
    internal class Options
    {
        [Option('c', "csharp", HelpText = "Output CSharp Model")]
        public bool OutputCSharp { get; set; }

        [Value(1, HelpText = "Output Folder", Required = true)]
        public string OutputFolder { get; set; }

        [Option('t', "typescript", HelpText = "Output Typescript Model")]
        public bool OutputTypescript { get; set; }

        [Value(0, HelpText = "Source Model File", Default = "model.xml", Required = true)]
        public string SourceFile { get; set; }
    }
}
}
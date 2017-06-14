using ModelGenerator.Model;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using CommandLine;
using JetBrains.Annotations;

[assembly: InternalsVisibleTo("ModelGenerator.Tests")]

namespace ModelGenerator
{
    [UsedImplicitly]
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<Options>(args);

            if (options is NotParsed<Options>)
                return;

            var optionsValue = ((Parsed<Options>)options).Value;

            var sourcePath = optionsValue.SourceFile;
            var outputFolder = optionsValue.OutputFolder.Trim('\\').Replace('\\', Path.PathSeparator).Replace('/', Path.PathSeparator);

            var inFile = File.OpenRead(sourcePath);

            var serializer = new XmlSerializer(typeof(Classes));
            var model = serializer.Deserialize(new StreamReader(inFile)) as Classes;

            if (model == null)
            {
                Console.WriteLine("Could not process XML file");
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
                return;
            }

            var modelOutputFolder = Path.Combine(outputFolder, model.ModelsFolder, model.ModelNamespace);
            var viewmodelOutputFolder = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace);
            var typescriptOutputFolder = Path.Combine(outputFolder, model.TypescriptFolder);

            if (optionsValue.OutputCSharp)
            {
                Directory.CreateDirectory(modelOutputFolder);
                Directory.CreateDirectory(viewmodelOutputFolder);
            }
            if (optionsValue.OutputTypescript)
                Directory.CreateDirectory(typescriptOutputFolder);

            CreateFiles(outputFolder, model, OutputMode.Model, optionsValue);
            if (model.Items.Any(i => i.GenerateCreateModel))
                CreateFiles(outputFolder, model, OutputMode.Create, optionsValue);
            if (model.Items.Any(i => i.GenerateDetailModel))
                CreateFiles(outputFolder, model, OutputMode.Details, optionsValue);
            if (model.Items.Any(i => i.GenerateSummaryModel))
                CreateFiles(outputFolder, model, OutputMode.Summary, optionsValue);
            if (model.Items.Any(i => i.GenerateUpdateModel))
                CreateFiles(outputFolder, model, OutputMode.Update, optionsValue);
        }

        private static void CreateFiles(string outputFolder, Classes model, OutputMode mode, Options options)
        {
            if (options.OutputCSharp)
            {
                Console.WriteLine($"Outputting C# {mode} Classes to {Path.Combine(options.OutputFolder, model.ModelsFolder)}");
                var modelGenerator = new Generator.CSharp.ClassGenerator(mode);
                var outputModel = new StringBuilder();
                modelGenerator.CreateClasses(model, outputModel);

                var outputPath = string.Empty;

                switch (mode)
                {
                    case OutputMode.Model:
                        outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ModelNamespace, "Models.cs");
                        break;

                    case OutputMode.Create:
                        outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace,
                            "CreateModels.cs");
                        break;

                    case OutputMode.Details:
                        outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace,
                            "DetailsModels.cs");
                        break;

                    case OutputMode.Summary:
                        outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace,
                            "SummaryModels.cs");
                        break;

                    case OutputMode.Update:
                        outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace,
                            "UpdateModels.cs");
                        break;
                }

                File.WriteAllText(outputPath, outputModel.ToString());
            }
            if (options.OutputTypescript)
            {
                Console.WriteLine($"Outputting TypeScript {mode} Classes to {Path.Combine(options.OutputFolder, model.TypescriptFolder)}");
                if (mode != OutputMode.Model && !string.IsNullOrWhiteSpace(model.TypescriptFolder))
                {
                    var tsGenerator = new Generator.Typescript.ClassGenerator(mode);
                    var outputTs = new StringBuilder();
                    tsGenerator.CreateClasses(model, outputTs);
                    var outputPath = string.Empty;

                    switch (mode)
                    {
                        case OutputMode.Create:
                            outputPath = Path.Combine(outputFolder, model.TypescriptFolder, "CreateModels.ts");
                            break;

                        case OutputMode.Details:
                            outputPath = Path.Combine(outputFolder, model.TypescriptFolder, "DetailsModels.ts");
                            break;

                        case OutputMode.Summary:
                            outputPath = Path.Combine(outputFolder, model.TypescriptFolder, "SummaryModels.ts");
                            break;

                        case OutputMode.Update:
                            outputPath = Path.Combine(outputFolder, model.TypescriptFolder, "UpdateModels.ts");
                            break;
                    }

                    File.WriteAllText(outputPath, outputTs.ToString());
                }
            }
        }
    }
}

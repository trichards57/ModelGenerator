using ModelGenerator.Model;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using CommandLine;
using JetBrains.Annotations;
using ModelGenerator.Generator;

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
            var outputFolder = optionsValue.OutputFolder.Trim('\\');

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

        private static void CreateFile(string folder, string extension, OutputMode mode, Classes model, ClassGeneratorBase generator)
        {
            var outputModel = new StringBuilder();
            generator.CreateClasses(model, outputModel);

            var outputPath = string.Empty;

            switch (mode)
            {
                case OutputMode.Model:
                    outputPath = Path.Combine(folder, $"Models.{extension}");
                    break;

                case OutputMode.Create:
                    outputPath = Path.Combine(folder, $"CreateModels.{extension}");
                    break;

                case OutputMode.Details:
                    outputPath = Path.Combine(folder, $"DetailsModels.{extension}");
                    break;

                case OutputMode.Summary:
                    outputPath = Path.Combine(folder, $"SummaryModels.{extension}");
                    break;

                case OutputMode.Update:
                    outputPath = Path.Combine(folder, $"UpdateModels.{extension}");
                    break;
            }

            File.WriteAllText(outputPath, outputModel.ToString());
        }

        private static void CreateFiles(string outputFolder, Classes model, OutputMode mode, Options options)
        {
            if (options.OutputCSharp)
            {
                Console.WriteLine($"Outputting C# {mode} Classes to {Path.Combine(options.OutputFolder, model.ModelsFolder)}");
                var modelGenerator = new Generator.CSharp.ClassGenerator(mode);

                CreateFile(mode == OutputMode.Model
                        ? Path.Combine(outputFolder, model.ModelsFolder, model.ModelNamespace)
                        : Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace), "cs", mode, model,
                    modelGenerator);
            }
            if (options.OutputTypescript)
            {
                if (mode != OutputMode.Model && !string.IsNullOrWhiteSpace(model.TypescriptFolder))
                {
                    Console.WriteLine($"Outputting TypeScript {mode} Classes to {Path.Combine(options.OutputFolder, model.TypescriptFolder)}");
                    var tsGenerator = new Generator.Typescript.ClassGenerator(mode);
                    CreateFile(Path.Combine(outputFolder, model.TypescriptFolder), "ts", mode, model, tsGenerator);
                }
            }
            if (options.OutputTypescriptComponents && mode == OutputMode.Model)
            {
                Console.WriteLine($"Outputting TypeScript Component {mode} Classes to {Path.Combine(options.OutputFolder, model.TypescriptFolder)}");
                if (!string.IsNullOrWhiteSpace(model.TypescriptFolder))
                {
                    var tsGenerator = new Generator.TypescriptComponents.ClassGenerator(mode);
                    var outputTs = new StringBuilder();
                    tsGenerator.CreateClasses(model, outputTs);
                    var outputPath = Path.Combine(outputFolder, model.TypescriptFolder, "Components.ts");

                    File.WriteAllText(outputPath, outputTs.ToString());
                }
            }
        }
    }
}

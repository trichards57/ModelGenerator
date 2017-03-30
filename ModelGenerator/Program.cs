using ModelGenerator.Generator;
using ModelGenerator.Model;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ModelGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Arguments");
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
                return;
            }

            var sourcePath = args[0];
            var outputFolder = args[1].Trim('\\');

            var inFile = File.OpenRead(sourcePath);

            var serializer = new XmlSerializer(typeof(Classes));
            var model = serializer.Deserialize(new StreamReader(inFile)) as Classes;

            var modelOutputFolder = Path.Combine(outputFolder, model.ModelsFolder, model.ModelNamespace);
            var viewmodelOutputFolder = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace);
            var typescriptOutputFolder = Path.Combine(outputFolder, model.TypescriptFolder);

            Directory.CreateDirectory(modelOutputFolder);
            Directory.CreateDirectory(viewmodelOutputFolder);
            Directory.CreateDirectory(typescriptOutputFolder);

            CreateFiles(outputFolder, model, OutputMode.Model);
            if (model.Items.Any(i => i.GenerateCreateModel))
                CreateFiles(outputFolder, model, OutputMode.Create);
            if (model.Items.Any(i => i.GenerateDetailModel))
                CreateFiles(outputFolder, model, OutputMode.Details);
            if (model.Items.Any(i => i.GenerateSummaryModel))
                CreateFiles(outputFolder, model, OutputMode.Summary);
            if (model.Items.Any(i => i.GenerateUpdateModel))
                CreateFiles(outputFolder, model, OutputMode.Update);
        }

        private static void CreateFiles(string outputFolder, Classes model, OutputMode mode)
        {
            var modelGenerator = new ClassGenerator(mode);
            var outputModel = new StringBuilder();
            modelGenerator.CreateClasses(model, outputModel);

            var outputPath = string.Empty;

            switch (mode)
            {
                case OutputMode.Model:
                    outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ModelNamespace, "Models.cs");
                    break;

                case OutputMode.Create:
                    outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace, "CreateModels.cs");
                    break;

                case OutputMode.Details:
                    outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace, "DetailsModels.cs");
                    break;

                case OutputMode.Summary:
                    outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace, "SummaryModels.cs");
                    break;

                case OutputMode.Update:
                    outputPath = Path.Combine(outputFolder, model.ModelsFolder, model.ViewModelNamespace, "UpdateModels.cs");
                    break;
            }

            File.WriteAllText(outputPath, outputModel.ToString());

            if (mode != OutputMode.Model && !string.IsNullOrWhiteSpace(model.TypescriptFolder))
            {
                var tsGenerator = new TypescriptGenerator(mode);
                var outputTs = new StringBuilder();
                tsGenerator.CreateClasses(model, outputTs);

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

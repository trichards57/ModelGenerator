using ModelGenerator.Generator;
using ModelGenerator.Model;
using System;
using System.IO;
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

            var modelGenerator = new ClassGenerator(OutputMode.Model);
            var outputModel = new StringBuilder();
            modelGenerator.CreateClasses(model, outputModel);

            var modelOutputFolder = Path.Combine(outputFolder, model.ModelNamespace);
            Directory.CreateDirectory(modelOutputFolder);

            File.WriteAllText(Path.Combine(modelOutputFolder, "Models.cs"), outputModel.ToString());

            modelGenerator = new ClassGenerator(OutputMode.Create);
            outputModel = new StringBuilder();
            modelGenerator.CreateClasses(model, outputModel);

            modelOutputFolder = Path.Combine(outputFolder, model.ViewModelNamespace);
            Directory.CreateDirectory(modelOutputFolder);

            File.WriteAllText(Path.Combine(modelOutputFolder, "CreateModels.cs"), outputModel.ToString());

            modelGenerator = new ClassGenerator(OutputMode.Details);
            outputModel = new StringBuilder();
            modelGenerator.CreateClasses(model, outputModel);

            modelOutputFolder = Path.Combine(outputFolder, model.ViewModelNamespace);
            Directory.CreateDirectory(modelOutputFolder);

            File.WriteAllText(Path.Combine(modelOutputFolder, "DetailModels.cs"), outputModel.ToString());

            modelGenerator = new ClassGenerator(OutputMode.Summary);
            outputModel = new StringBuilder();
            modelGenerator.CreateClasses(model, outputModel);

            modelOutputFolder = Path.Combine(outputFolder, model.ViewModelNamespace);
            Directory.CreateDirectory(modelOutputFolder);

            File.WriteAllText(Path.Combine(modelOutputFolder, "SummaryModels.cs"), outputModel.ToString());

            modelGenerator = new ClassGenerator(OutputMode.Update);
            outputModel = new StringBuilder();
            modelGenerator.CreateClasses(model, outputModel);

            modelOutputFolder = Path.Combine(outputFolder, model.ViewModelNamespace);
            Directory.CreateDirectory(modelOutputFolder);

            File.WriteAllText(Path.Combine(modelOutputFolder, "UpdateModels.cs"), outputModel.ToString());
        }
    }
}

using ModelGenerator.Generator;
using ModelGenerator.Model;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ModelGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid Arguments");
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }

            var sourcePath = args[0];
            var outputFolder = args[1].Trim('\\');

            var serializer = new XmlSerializer(typeof(Classes));
            var model = serializer.Deserialize(new StreamReader(sourcePath)) as Classes;

            var modelGenerator = new ClassGenerator(OutputMode.Model);
            var outputModel = new StringBuilder();
            modelGenerator.CreateClasses(model, outputModel);

            var modelOutputFolder = Path.Combine(outputFolder, model.ModelNamespace);
            Directory.CreateDirectory(modelOutputFolder);

            File.WriteAllText(Path.Combine(modelOutputFolder, "Models.cs"), outputModel.ToString());
        }
    }
}
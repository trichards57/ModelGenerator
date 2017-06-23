using System.Linq;
using System.Text;
using ModelGenerator.Model;

namespace ModelGenerator.Generator.TypescriptComponents
{
    internal class ClassGenerator : Typescript.ClassGenerator
    {
        private readonly Typescript.PropertiesGenerator _propGenerator;

        public ClassGenerator(OutputMode mode) : base(mode)
        {
            _propGenerator = new Typescript.PropertiesGenerator(mode);
        }

        public override void CreateClass(Class model, StringBuilder output)
        {
            base.CreateClass(model, output);
            output.AppendLine();

            var name = "I" + HelperClasses.GetName(model.Name, Mode);

            output.AppendLine($"// Model Generator {GeneratorVersion}");
            output.AppendLine($"export interface {name}Change {{");

            _propGenerator.CreateChangeProperties(model.Properties.OrderBy(p => p.Name), output);

            output.AppendLine("}");

            var pickName = $"pick{HelperClasses.GetName(model.Name, Mode)}";

            output.AppendLine();
            output.AppendLine($"export function {pickName}(source : {name}) : {name} {{");
            output.AppendLine("    return {");
            _propGenerator.CreateCopyProperties(model.Properties.OrderBy(p => p.Name), output, "source");
            output.AppendLine("    };");
            output.AppendLine("}");
        }

        public override void CreateClasses(Classes model, StringBuilder output)
        {
            foreach (var c in model.Items)
                _propGenerator.ClassMapping.Add(c.Name, "I" + HelperClasses.GetName(c.Name, Mode));

            var groupedModel = model.Items.SelectMany(m =>
            {
                var components = m.Properties.Where(p => !string.IsNullOrWhiteSpace(p.Component)).Select(p => p.Component).Distinct();

                return components.Select(s => new Class
                {
                    Name = m.Name + s,
                    Properties = m.Properties.Where(p => p.Component == s).ToList(),
                    GenerateDetailModel = true
                });
            }).ToList();

            var newModel = new Classes
            {
                Items = groupedModel.ToList(),
                TypescriptFolder = model.TypescriptFolder,
            };

            base.CreateClasses(newModel, output);
        }
    }
}

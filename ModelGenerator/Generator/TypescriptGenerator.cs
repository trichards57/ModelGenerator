using ModelGenerator.Model;
using System.Linq;
using System.Text;

namespace ModelGenerator.Generator
{
    internal class TypescriptGenerator : ClassGeneratorBase
    {
        private TypescriptPropertiesGenerator _propGenerator;

        public TypescriptGenerator(OutputMode mode) : base(mode)
        {
            _propGenerator = new TypescriptPropertiesGenerator(mode);
        }

        public override void CreateClass(Class model, StringBuilder output)
        {
            var name = "I" + HelperClasses.GetName(model.Name, Mode);

            output.AppendLine($"// Model Generator {GeneratorVersion}");
            output.AppendLine($"interface {name} {{");

            _propGenerator.CreateProperties(model.Properties.OrderBy(p => p.Name), output);

            output.AppendLine("}");
        }
    }
}

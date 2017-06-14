using System.Linq;
using System.Text;
using ModelGenerator.Model;

namespace ModelGenerator.Generator.Typescript
{
    internal class ClassGenerator : ClassGeneratorBase
    {
        private readonly PropertiesGenerator _propGenerator;

        public ClassGenerator(OutputMode mode) : base(mode)
        {
            _propGenerator = new PropertiesGenerator(mode);
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

﻿using System.Linq;
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
            output.AppendLine($"export interface {name} {{");

            _propGenerator.CreateProperties(model.Properties.OrderBy(p => p.Name), output);

            output.AppendLine("}");
        }

        public override void CreateClasses(Classes model, StringBuilder output)
        {
            foreach (var c in model.Items)
            {
                _propGenerator.ClassMapping.Add(c.Name, "I" + HelperClasses.GetName(c.Name, Mode));
            }

            base.CreateClasses(model, output);
        }
    }
}

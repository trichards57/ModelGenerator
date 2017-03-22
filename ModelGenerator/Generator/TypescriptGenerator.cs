using ModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModelGenerator.Generator
{
    internal class TypescriptGenerator
    {
        private string _generatorVersion;
        private OutputMode _mode;
        private TypescriptPropertiesGenerator _propGenerator;

        public TypescriptGenerator(OutputMode mode)
        {
            _mode = mode;
            _generatorVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            _propGenerator = new TypescriptPropertiesGenerator(mode);
        }

        public void CreateClass(Class model, StringBuilder output)
        {
            var name = GetName(model);

            output.AppendLine($"// Model Generator {_generatorVersion}");
            output.AppendLine($"interface {name} {{");

            _propGenerator.CreateProperties(model.Properties.OrderBy(p => p.Name), output);

            output.AppendLine("}");
        }

        public void CreateClasses(Classes model, StringBuilder output)
        {
            var cls = model.Items.AsEnumerable();

            switch (_mode)
            {
                case OutputMode.Create:
                    cls = cls.Where(c => c.GenerateCreateModel);
                    break;

                case OutputMode.Details:
                    cls = cls.Where(c => c.GenerateDetailModel);
                    break;

                case OutputMode.Summary:
                    cls = cls.Where(c => c.GenerateSummaryModel);
                    break;

                case OutputMode.Update:
                    cls = cls.Where(c => c.GenerateUpdateModel);
                    break;
            }

            foreach (var cl in cls)
            {
                CreateClass(cl, output);
                output.AppendLine();
            }
        }

        private string GetName(Class model)
        {
            var name = "I" + model.Name;

            switch (_mode)
            {
                case OutputMode.Create:
                    name += "Create";
                    break;

                case OutputMode.Details:
                    name += "Details";
                    break;

                case OutputMode.Summary:
                    name += "Summary";
                    break;

                case OutputMode.Update:
                    name += "Update";
                    break;
            }
            return name;
        }
    }
}

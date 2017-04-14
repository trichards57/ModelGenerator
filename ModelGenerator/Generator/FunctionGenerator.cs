using ModelGenerator.Model;
using System;
using System.Linq;
using System.Text;

namespace ModelGenerator.Generator
{
    public class FunctionGenerator
    {
        private readonly OutputMode _mode;

        public FunctionGenerator(OutputMode mode)
        {
            _mode = mode;
        }

        public void CreateCloneMethod(Class model, StringBuilder output)
        {
            output.AppendLine("\t\tpublic object Clone()");
            CreateCopyFunction(model, output);
        }

        public void CreateConstructor(Class model, StringBuilder output)
        {
            if (_mode == OutputMode.Create || _mode == OutputMode.Model)
                return;

            var name = HelperClasses.GetName(model.Name, _mode);

            if (_mode == OutputMode.Update)
            {
                output.AppendLine($"\t\tpublic {name}() {{ }}");
            }

            output.AppendLine($"\t\tpublic {name}({model.Name} item)");
            output.AppendLine("\t\t{");

            var lines = HelperClasses.FilterProperties(model.Properties, _mode)
                .OrderBy(p => p.Name).Select(prop =>
            {
                if (prop.GenerateAsList)
                    return $"\t\t\t{prop.Name} = item.{prop.Name}.Select(i => new {HelperClasses.GetName(prop.Type, _mode)}(i));";
                return $"\t\t\t{prop.Name} = item.{prop.Name};";
            });

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine("\t\t}");
        }

        public void CreateToItemMethod(Class model, StringBuilder output)
        {
            if (_mode != OutputMode.Create)
                return;

            output.AppendLine($"\t\tpublic {model.Name} ToItem()");
            CreateCopyFunction(model, output);
        }

        private void CreateCopyFunction(Class model, StringBuilder output)
        {
            output.AppendLine("\t\t{");
            output.AppendLine($"\t\t\tvar item = new {model.Name}();");

            foreach (var prop in HelperClasses.FilterProperties(model.Properties, _mode)
                .Where(p => string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Where(p => !p.GenerateAsList)
                .OrderBy(p => p.Name))
            {
                output.AppendLine($"\t\t\titem.{ prop.Name} = { prop.Name};");
            }

            output.AppendLine();
            output.AppendLine("\t\t\treturn item;");
            output.AppendLine("\t\t}");
        }
    }
}

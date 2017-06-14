using System;
using System.Linq;
using System.Text;
using ModelGenerator.Model;

namespace ModelGenerator.Generator.CSharp
{
    internal class FunctionGenerator
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
                .OrderBy(p => p.Name)
                .Select(prop => prop.GenerateAsList ? $"\t\t\t{prop.Name} = item.{prop.Name}.Select(i => new {HelperClasses.GetName(prop.Type, _mode)}(i));" : $"\t\t\t{prop.Name} = item.{prop.Name};");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine("\t\t}");
        }

        public void CreateEqualsMethods(Class model, StringBuilder output)
        {
            var name = HelperClasses.GetName(model.Name, _mode);

            output.AppendLine();
            output.AppendLine("\t\tpublic override bool Equals(object other)");
            output.AppendLine("\t\t{");
            output.AppendLine($"\t\t\treturn Equals(other as {name});");
            output.AppendLine("\t\t}");
            output.AppendLine();
            output.AppendLine($"\t\tpublic bool Equals({name} other)");
            output.AppendLine("\t\t{");
            output.AppendLine("\t\t\tif (other == null)");
            output.AppendLine("\t\t\t\treturn false;");
            output.AppendLine();
            output.AppendLine("\t\t\tvar res = true;");
            output.AppendLine();

            var lines = HelperClasses.FilterProperties(model.Properties, _mode).Where(p => string.IsNullOrWhiteSpace(p.NavigationPropertyId)).Where(p => !p.GenerateAsList).OrderBy(p => p.Name)
                .Select(prop => prop.Type == "DateTime" || prop.Type == "DateTimeOffset"
                    ? $"\t\t\tres &= ({prop.Name} - other.{prop.Name}).TotalSeconds < 30;"
                    : $"\t\t\tres &= {prop.Name}.Equals(other.{prop.Name});");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("\t\t\treturn res;");
            output.AppendLine("\t\t}");
        }

        public void CreateHashCodeMethod(Class model, StringBuilder output)
        {
            output.AppendLine();
            output.AppendLine("\t\tpublic override int GetHashCode()");
            output.AppendLine("\t\t{");
            output.AppendLine("\t\t\tint hash = 17;");
            output.AppendLine();

            var lines = HelperClasses.FilterProperties(model.Properties, _mode)
                .Where(p => string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Where(p => !p.GenerateAsList)
                .OrderBy(p => p.Name)
                .Select(prop => $"\t\t\thash = hash * 31 + {prop.Name}.GetHashCode();");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("\t\t\treturn hash;");
            output.AppendLine("\t\t}");
        }

        public void CreateToItemMethod(Class model, StringBuilder output)
        {
            if (_mode != OutputMode.Create)
                return;

            output.AppendLine($"\t\tpublic {model.Name} ToItem()");
            CreateCopyFunction(model, output);
        }

        public void CreateToViewModelMethod(Class model, OutputMode mode, StringBuilder output)
        {
            if (_mode != OutputMode.Model)
                return;

            var name = HelperClasses.GetName("To", mode);
            var returnType = HelperClasses.GetName(model.Name, mode);

            output.AppendLine($"\t\tpublic {returnType} {name}()");
            output.AppendLine("\t\t{");
            output.AppendLine($"\t\treturn new {returnType}(this);");
            output.AppendLine("\t\t}");
        }

        public void CreateUpdateItemMethod(Class model, StringBuilder output)
        {
            if (_mode != OutputMode.Update)
                return;

            output.AppendLine($"\t\tpublic void UpdateItem({model.Name} item)");
            output.AppendLine("\t\t{");

            var lines = HelperClasses.FilterProperties(model.Properties, _mode)
                .Where(p => !p.GenerateAsList)
                .OrderBy(p => p.Name)
                .Select(prop => $"\t\t\titem.{ prop.Name } = { prop.Name };");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine("\t\t}");
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
                output.AppendLine($"\t\t\titem.{ prop.Name } = { prop.Name };");
            }

            output.AppendLine();
            output.AppendLine("\t\t\treturn item;");
            output.AppendLine("\t\t}");
        }
    }
}

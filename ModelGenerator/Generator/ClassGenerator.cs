using ModelGenerator.Model;
using System.Text;
using System;
using System.Reflection;
using System.Linq;

namespace ModelGenerator.Generator
{
    public class ClassGenerator
    {
        private string _generatorVersion;
        private OutputMode _mode;
        private PropertiesGenerator _propGenerator;

        public ClassGenerator(OutputMode mode)
        {
            _mode = mode;
            _generatorVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            _propGenerator = new PropertiesGenerator(mode);
        }

        public void CreateClass(Class model, StringBuilder output)
        {
            output.AppendLine($"    [GeneratedCode(\"Model Generator\", \"{_generatorVersion}\"), ExcludeFromCodeCoverage]");
            output.Append($"    public partial class {model.Name} : IIdentifiable, ICloneable, IEquatable<{model.Name}>");

            if (_mode == OutputMode.Model && model.GenerateDetailModel)
                output.Append($", IDetailable<{model.Name}Details>");

            output.AppendLine();

            output.AppendLine("    {");

            _propGenerator.CreateProperties(model.Properties.OrderBy(p => p.Name), output);

            if (_mode == OutputMode.Model)
            {
                output.AppendLine();
                CreateCloneMethod(model, output);
            }

            CreateEqualsMethods(model, output);
            CreateHashCodeMethod(model, output);
            if (_mode == OutputMode.Model && model.GenerateDetailModel)
                CreateToDetailMethod(model, output);

            output.AppendLine("    }");
        }

        public void CreateClasses(Classes model, StringBuilder output)
        {
            CreateUsings(model, output);
            StartNamespace(model, output);
            CreateInterfaces(output);
            foreach (var cl in model.Items)
            {
                CreateClass(cl, output);
                output.AppendLine();
            }
            EndNamespace(output);
        }

        private void CreateCloneMethod(Class model, StringBuilder output)
        {
            output.AppendLine("        public object Clone()");
            output.AppendLine("        {");
            output.AppendLine($"            var item = new {model.Name}");
            output.AppendLine("            {");

            var lines = model.Properties.OrderBy(p => p.Name).Select(prop => $"                { prop.Name} = { prop.Name}");

            output.Append(string.Join("," + Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine("            };");
            output.AppendLine();
            output.AppendLine("            return item;");
            output.AppendLine("        }");
        }

        private void CreateEqualsMethods(Class model, StringBuilder output)
        {
            output.AppendLine();
            output.AppendLine($"        public override bool Equals(object other)");
            output.AppendLine("        {");
            output.AppendLine($"            return Equals(other as {model.Name});");
            output.AppendLine("        }");
            output.AppendLine();
            output.AppendLine($"        public bool Equals({model.Name} other)");
            output.AppendLine("        {");
            output.AppendLine("            if (other == null)");
            output.AppendLine("                return false;");
            output.AppendLine();
            output.AppendLine($"            var res = true;");
            output.AppendLine();

            var lines = model.Properties.OrderBy(p => p.Name).Select(prop => $"            res &= {prop.Name}.Equals(other.{prop.Name});");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("            return res;");
            output.AppendLine("        }");
        }

        private void CreateHashCodeMethod(Class model, StringBuilder output)
        {
            output.AppendLine();
            output.AppendLine($"        public override int GetHashCode()");
            output.AppendLine("        {");
            output.AppendLine("            int hash = 17;");
            output.AppendLine();

            var lines = model.Properties.OrderBy(p => p.Name).Select(prop => $"            hash = hash * 31 + {prop.Name}.GetHashCode();");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("            return hash;");
            output.AppendLine("        }");
        }

        private void CreateInterfaces(StringBuilder output)
        {
            output.AppendLine("    public interface IDetailable<TDetail>");
            output.AppendLine("    {");
            output.AppendLine("        TDetail ToDetail();");
            output.AppendLine("    }");
            output.AppendLine();
            output.AppendLine("    public interface IIdentifiable");
            output.AppendLine("    {");
            output.AppendLine("        int Id { get; set; }");
            output.AppendLine("    }");
            output.AppendLine();
        }

        private void CreateToDetailMethod(Class model, StringBuilder output)
        {
            output.AppendLine($"        public {model.Name}Details ToDetail()");
            output.AppendLine("        {");
            output.AppendLine($"            return new {model.Name}Details(this);");
            output.AppendLine("        }");
        }

        private void CreateUsings(Classes model, StringBuilder output)
        {
            output.AppendLine($"using {model.RootNamespace}.{model.ViewModelNamespace};");
            output.AppendLine("using System;");
            output.AppendLine("using System.CodeDom.Compiler;");
            output.AppendLine("using System.Collections.Generic;");
            output.AppendLine("using System.ComponentModel.DataAnnotations;");
            output.AppendLine("using System.Diagnostics.CodeAnalysis;");
            output.AppendLine();
        }

        private void EndNamespace(StringBuilder output)
        {
            output.AppendLine("}");
            output.AppendLine();
            output.AppendLine("#pragma warning restore 1591");
        }

        private void StartNamespace(Classes model, StringBuilder output)
        {
            output.AppendLine("#pragma warning disable 1591");
            output.AppendLine();

            if (_mode == OutputMode.Model)
            {
                output.AppendLine($"namespace {model.RootNamespace}.{model.ModelNamespace}");
                output.AppendLine("{");
            }
        }
    }
}
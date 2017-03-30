using ModelGenerator.Model;
using System.Text;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

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
            var name = GetName(model);

            output.AppendLine($"\t[GeneratedCode(\"Model Generator\", \"{_generatorVersion}\"), ExcludeFromCodeCoverage]");
            output.Append($"\tpublic partial class {name}");

            if (_mode == OutputMode.Model)
            {
                output.Append($": IIdentifiable, ICloneable, IEquatable<{model.Name}>");

                if (model.GenerateDetailModel)
                    output.Append($", IDetailable<{model.Name}Details>");
            }

            if (_mode == OutputMode.Create)
                output.Append($": ICreateViewModel<{model.Name}>");

            if (_mode == OutputMode.Update)
                output.Append($": IUpdateViewModel<{model.Name}>");

            output.AppendLine();

            output.AppendLine("\t{");

            if (_mode == OutputMode.Details || _mode == OutputMode.Summary || _mode == OutputMode.Update)
                CreateConstructor(model, output);

            _propGenerator.CreateProperties(model.Properties.OrderBy(p => p.Name), output);

            if (_mode == OutputMode.Model)
                CreateCloneMethod(model, output);

            if (_mode == OutputMode.Model || _mode == OutputMode.Details || _mode == OutputMode.Summary)
            {
                CreateEqualsMethods(model, output);
                CreateHashCodeMethod(model, output);
            }

            if (_mode == OutputMode.Model && model.GenerateDetailModel)
                CreateToDetailMethod(model, output);

            if (_mode == OutputMode.Create)
                CreateToItemMethod(model, output);

            if (_mode == OutputMode.Update)
                CreateUpdateItemMethod(model, output);

            output.AppendLine("\t}");
        }

        public void CreateClasses(Classes model, StringBuilder output)
        {
            CreateUsings(model, output);
            StartNamespace(model, output);

            if (_mode == OutputMode.Model)
                CreateModelInterfaces(output);
            if (_mode == OutputMode.Create)
                CreateCreateInterfaces(output);
            if (_mode == OutputMode.Update)
                CreateUpdateInterfaces(output);

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
            EndNamespace(output);
        }

        private void CreateCloneMethod(Class model, StringBuilder output)
        {
            output.AppendLine("\t\tpublic object Clone()");
            CreateCopyFunction(model, output);
        }

        private void CreateConstructor(Class model, StringBuilder output)
        {
            var name = GetName(model);

            if (_mode == OutputMode.Update)
            {
                output.Append($"\t\tpublic {name}() {{ }}");
                output.AppendLine("\t\t");
            }

            output.AppendLine($"\t\tpublic {name}({model.Name} item)");
            output.AppendLine("\t\t{");

            var lines = FilterProperties(model.Properties).OrderBy(p => p.Name).Select(prop =>
            {
                if (prop.GenerateAsList)
                    return $"\t\t\t{prop.Name} = item.{prop.Name}.Select(i=> new {prop.Type}Details(i));";
                return $"\t\t\t{prop.Name} = item.{prop.Name};";
            });

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine("\t\t}");
        }

        private void CreateCopyFunction(Class model, StringBuilder output)
        {
            output.AppendLine("\t\t{");
            output.AppendLine($"\t\t\tvar item = new {model.Name}");
            output.AppendLine("\t\t\t{");

            var lines = FilterProperties(model.Properties).OrderBy(p => p.Name).Select(prop => $"\t\t\t\t{ prop.Name} = { prop.Name}");

            output.Append(string.Join("," + Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine("\t\t\t};");
            output.AppendLine();
            output.AppendLine("\t\t\treturn item;");
            output.AppendLine("\t\t}");
        }

        private void CreateCreateInterfaces(StringBuilder output)
        {
            output.AppendLine("\tpublic interface ICreateViewModel<TModel>");
            output.AppendLine("\t{");
            output.AppendLine("\t\tTModel ToItem();");
            output.AppendLine("\t}");
            output.AppendLine();
        }

        private void CreateEqualsMethods(Class model, StringBuilder output)
        {
            var name = GetName(model);

            output.AppendLine();
            output.AppendLine($"\t\tpublic override bool Equals(object other)");
            output.AppendLine("\t\t{");
            output.AppendLine($"\t\t\treturn Equals(other as {name});");
            output.AppendLine("\t\t}");
            output.AppendLine();
            output.AppendLine($"\t\tpublic bool Equals({name} other)");
            output.AppendLine("\t\t{");
            output.AppendLine("\t\t\tif (other == null)");
            output.AppendLine("\t\t\t\treturn false;");
            output.AppendLine();
            output.AppendLine($"\t\t\tvar res = true;");
            output.AppendLine();

            var lines = FilterProperties(model.Properties).Where(p => !p.GenerateAsList).OrderBy(p => p.Name)
                .Select(prop =>
                {
                    if (prop.Type == "DateTime" || prop.Type == "DateTimeOffset")
                        return $"\t\t\tres &= ({prop.Name} - other.{prop.Name}).TotalSeconds < 30;";
                    else
                        return $"\t\t\tres &= {prop.Name}.Equals(other.{prop.Name});";
                });

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("\t\t\treturn res;");
            output.AppendLine("\t\t}");
        }

        private void CreateHashCodeMethod(Class model, StringBuilder output)
        {
            output.AppendLine();
            output.AppendLine($"\t\tpublic override int GetHashCode()");
            output.AppendLine("\t\t{");
            output.AppendLine("\t\t\tint hash = 17;");
            output.AppendLine();

            var lines = FilterProperties(model.Properties).OrderBy(p => p.Name).Select(prop => $"\t\t\thash = hash * 31 + {prop.Name}.GetHashCode();");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine();
            output.AppendLine("\t\t\treturn hash;");
            output.AppendLine("\t\t}");
        }

        private void CreateModelInterfaces(StringBuilder output)
        {
            output.AppendLine("\tpublic interface IDetailable<TDetail>");
            output.AppendLine("\t{");
            output.AppendLine("\t    TDetail ToDetail();");
            output.AppendLine("\t}");
            output.AppendLine();
            output.AppendLine("\tpublic interface IIdentifiable");
            output.AppendLine("\t{");
            output.AppendLine("\t    int Id { get; set; }");
            output.AppendLine("\t}");
            output.AppendLine();
        }

        private void CreateToDetailMethod(Class model, StringBuilder output)
        {
            output.AppendLine($"\t\tpublic {model.Name}Details ToDetail()");
            output.AppendLine("\t\t{");
            output.AppendLine($"\t\t\treturn new {model.Name}Details(this);");
            output.AppendLine("\t\t}");
        }

        private void CreateToItemMethod(Class model, StringBuilder output)
        {
            output.AppendLine($"\t\tpublic {model.Name} ToItem()");
            CreateCopyFunction(model, output);
        }

        private void CreateUpdateInterfaces(StringBuilder output)
        {
            output.AppendLine("\tpublic interface IUpdateViewModel<TModel>");
            output.AppendLine("\t{");
            output.AppendLine("\t    void UpdateItem(TModel item);");
            output.AppendLine("\t    int Id { get; set; }");
            output.AppendLine("\t}");
            output.AppendLine();
        }

        private void CreateUpdateItemMethod(Class model, StringBuilder output)
        {
            output.AppendLine($"\t\tpublic void UpdateItem({model.Name} item)");
            output.AppendLine("\t\t{");

            var lines = FilterProperties(model.Properties).OrderBy(p => p.Name).Select(prop => $"\t\t\titem.{ prop.Name} = { prop.Name};");

            output.Append(string.Join(Environment.NewLine, lines));
            output.AppendLine();
            output.AppendLine("\t\t}");
        }

        private void CreateUsings(Classes model, StringBuilder output)
        {
            if (_mode == OutputMode.Model)
                output.AppendLine($"using {model.RootNamespace}.{model.ViewModelNamespace};");
            if (_mode == OutputMode.Create || _mode == OutputMode.Details || _mode == OutputMode.Summary || _mode == OutputMode.Update)
                output.AppendLine($"using {model.RootNamespace}.{model.ModelNamespace};");

            output.AppendLine("using System;");
            output.AppendLine("using System.CodeDom.Compiler;");

            if (_mode == OutputMode.Model || _mode == OutputMode.Details)
                output.AppendLine("using System.Collections.Generic;");

            output.AppendLine("using System.ComponentModel.DataAnnotations;");
            output.AppendLine("using System.Diagnostics.CodeAnalysis;");
            if (_mode == OutputMode.Details)
                output.AppendLine($"using System.Linq;");

            output.AppendLine();
        }

        private void EndNamespace(StringBuilder output)
        {
            output.AppendLine("}");
            output.AppendLine();
            output.AppendLine("#pragma warning restore 1591");
        }

        private IEnumerable<Property> FilterProperties(IEnumerable<Property> properties)
        {
            switch (_mode)
            {
                case OutputMode.Create:
                    return properties.Where(c => c.IncludeInCreate);

                case OutputMode.Details:
                    return properties.Where(c => c.IncludeInDetail);

                case OutputMode.Summary:
                    return properties.Where(c => c.IncludeInSummary);

                case OutputMode.Update:
                    return properties.Where(c => c.IncludeInUpdate);
            }
            return properties;
        }

        private string GetName(Class model)
        {
            var name = model.Name;

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

        private void StartNamespace(Classes model, StringBuilder output)
        {
            output.AppendLine("#pragma warning disable 1591");
            output.AppendLine();

            if (_mode == OutputMode.Model)
            {
                output.AppendLine($"namespace {model.RootNamespace}.{model.ModelNamespace}");
                output.AppendLine("{");
            }
            else
            {
                output.AppendLine($"namespace {model.RootNamespace}.{model.ViewModelNamespace}");
                output.AppendLine("{");
            }
        }
    }
}
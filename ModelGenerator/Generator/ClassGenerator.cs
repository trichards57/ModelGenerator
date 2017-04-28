using ModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModelGenerator.Generator
{
    internal class ClassGenerator
    {
        private readonly FunctionGenerator _functionGenerator;
        private readonly string _generatorVersion;
        private readonly OutputMode _mode;
        private readonly PropertiesGenerator _propGenerator;

        public ClassGenerator(OutputMode mode)
        {
            _mode = mode;
            _generatorVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            _propGenerator = new PropertiesGenerator(mode);
            _functionGenerator = new FunctionGenerator(mode);
        }

        public void CreateClass(Class model, StringBuilder output)
        {
            var name = HelperClasses.GetName(model.Name, _mode);

            output.AppendLine($"\t[GeneratedCode(\"Model Generator\", \"{_generatorVersion}\"), ExcludeFromCodeCoverage]");
            output.Append($"\tpublic partial class {name}");

            if (_mode == OutputMode.Model)
            {
                output.Append($" : IIdentifiable, ICloneable, IEquatable<{model.Name}>");

                if (model.GenerateDetailModel)
                    output.Append($", IDetailable<{model.Name}Details>");
                if (model.GenerateSummaryModel)
                    output.Append($", ISummarisable<{model.Name}Summary>");
            }

            if (_mode == OutputMode.Create)
                output.Append($" : ICreateViewModel<{model.Name}>");

            if (_mode == OutputMode.Update)
                output.Append($" : IUpdateViewModel<{model.Name}>");

            output.AppendLine();

            output.AppendLine("\t{");

            _functionGenerator.CreateConstructor(model, output);
            _propGenerator.CreateProperties(model.Properties.OrderBy(p => p.Name), output);
            _functionGenerator.CreateCloneMethod(model, output);
            _functionGenerator.CreateEqualsMethods(model, output);
            _functionGenerator.CreateHashCodeMethod(model, output);

            if (model.GenerateDetailModel)
                _functionGenerator.CreateToViewModelMethod(model, OutputMode.Details, output);
            if (model.GenerateSummaryModel)
                _functionGenerator.CreateToViewModelMethod(model, OutputMode.Summary, output);

            _functionGenerator.CreateToItemMethod(model, output);

            _functionGenerator.CreateUpdateItemMethod(model, output);

            output.AppendLine("\t}");
        }

        public void CreateClasses(Classes model, StringBuilder output)
        {
            CreateUsings(model, output);
            StartNamespace(model, output);

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

        internal void CreateUsings(Classes model, StringBuilder output)
        {
            var outputs = new List<string>
            {
                "using System.CodeDom.Compiler;",
                "using System.ComponentModel.DataAnnotations;",
                "using System.Diagnostics.CodeAnalysis;",
                "using System;",
                "using WebsiteHelpers.Interfaces;"
            };
            if (_mode == OutputMode.Model)
            {
                outputs.Add("using System.Collections.Generic;");
                outputs.Add("using System.ComponentModel.DataAnnotations.Schema;");
                outputs.Add($"using {model.RootNamespace}.{model.ViewModelNamespace};");
            }
            else if (_mode == OutputMode.Details)
            {
                outputs.Add("using System.Collections.Generic;");
                outputs.Add("using System.Linq;");
                outputs.Add($"using {model.RootNamespace}.{model.ModelNamespace};");
            }
            else
            {
                outputs.Add($"using {model.RootNamespace}.{model.ModelNamespace};");
            }

            outputs.Sort();

            output.AppendLine(string.Join(Environment.NewLine, outputs));
            output.AppendLine();
        }

        internal void EndNamespace(StringBuilder output)
        {
            output.AppendLine("}");
        }

        internal void StartNamespace(Classes model, StringBuilder output)
        {
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

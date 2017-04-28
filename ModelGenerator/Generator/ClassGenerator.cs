using ModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGenerator.Generator
{
    internal class ClassGenerator : ClassGeneratorBase
    {
        private readonly FunctionGenerator _functionGenerator;
        private readonly PropertiesGenerator _propGenerator;

        public ClassGenerator(OutputMode mode) : base(mode)
        {
            _propGenerator = new PropertiesGenerator(mode);
            _functionGenerator = new FunctionGenerator(mode);
        }

        public override void CreateClass(Class model, StringBuilder output)
        {
            var name = HelperClasses.GetName(model.Name, Mode);

            output.AppendLine($"\t[GeneratedCode(\"Model Generator\", \"{GeneratorVersion}\"), ExcludeFromCodeCoverage]");
            output.Append($"\tpublic partial class {name}");

            if (Mode == OutputMode.Model)
            {
                output.Append($" : IIdentifiable, ICloneable, IEquatable<{model.Name}>");

                if (model.GenerateDetailModel)
                    output.Append($", IDetailable<{model.Name}Details>");
                if (model.GenerateSummaryModel)
                    output.Append($", ISummarisable<{model.Name}Summary>");
            }

            if (Mode == OutputMode.Create)
                output.Append($" : ICreateViewModel<{model.Name}>");

            if (Mode == OutputMode.Update)
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

        public override void CreateClasses(Classes model, StringBuilder output)
        {
            CreateUsings(model, output);
            StartNamespace(model, output);

            base.CreateClasses(model, output);

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
            if (Mode == OutputMode.Model)
            {
                outputs.Add("using System.Collections.Generic;");
                outputs.Add("using System.ComponentModel.DataAnnotations.Schema;");
                outputs.Add($"using {model.RootNamespace}.{model.ViewModelNamespace};");
            }
            else if (Mode == OutputMode.Details)
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
            if (Mode == OutputMode.Model)
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

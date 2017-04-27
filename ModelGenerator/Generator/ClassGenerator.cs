using ModelGenerator.Model;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModelGenerator.Generator
{
    public class ClassGenerator
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
            var name = GetName(model);

            output.AppendLine($"\t[GeneratedCode(\"Model Generator\", \"{_generatorVersion}\"), ExcludeFromCodeCoverage]");
            output.Append($"\tpublic partial class {name}");

            if (_mode == OutputMode.Model)
            {
                output.Append($": IIdentifiable, ICloneable, IEquatable<{model.Name}>");

                if (model.GenerateDetailModel)
                    output.Append($", IDetailable<{model.Name}Details>");
                if (model.GenerateSummaryModel)
                    output.Append($", ISummarisable<{model.Name}Summary>");
            }

            if (_mode == OutputMode.Create)
                output.Append($": ICreateViewModel<{model.Name}>");

            if (_mode == OutputMode.Update)
                output.Append($": IUpdateViewModel<{model.Name}>");

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

            if (_mode == OutputMode.Model)
                output.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");

            output.AppendLine("using System.ComponentModel.DataAnnotations;");
            output.AppendLine("using System.Diagnostics.CodeAnalysis;");
            if (_mode == OutputMode.Details)
                output.AppendLine($"using System.Linq;");
            output.AppendLine($"using WebsiteHelpers.Interfaces;");

            output.AppendLine();
        }

        private void EndNamespace(StringBuilder output)
        {
            output.AppendLine("}");
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

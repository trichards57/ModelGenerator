using ModelGenerator.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGenerator.Generator
{
    internal class PropertiesGenerator
    {
        private OutputMode _mode;

        public PropertiesGenerator(OutputMode mode)
        {
            _mode = mode;
        }

        private bool IsClientSide => _mode != OutputMode.Model;
        private bool IsReadOnlyMode => !(_mode == OutputMode.Model || _mode == OutputMode.Create || _mode == OutputMode.Update);

        public void CreateProperties(IEnumerable<Property> properties, StringBuilder output)
        {
            foreach (var p in HelperClasses.FilterProperties(properties, _mode))
                CreateProperty(p, output);
        }

        public void CreateProperty(Property property, StringBuilder output)
        {
            CreateValidationAttributes(property, output);

            if (!string.IsNullOrWhiteSpace(property.DisplayName) && IsClientSide)
                output.AppendLine($"\t\t[Display(Name=\"{property.DisplayName}\")]");

            if (!string.IsNullOrWhiteSpace(property.NavigationPropertyId) && !IsClientSide)
                output.AppendLine($"\t\t[ForeignKey(\"{property.NavigationPropertyId}\")]");

            var type = HelperClasses.GetName(property.Type, _mode);

            if (property.GenerateAsList)
            {
                if (_mode == OutputMode.Model)
                    output.AppendLine($"\t\tpublic ICollection<{type}> {property.Name} {{ get; set; }} = new HashSet<{type}>();");
                else
                    output.AppendLine($"\t\tpublic IEnumerable<{type}> {property.Name} {{ get; set; }}");
            }
            else
            {
                if (_mode == OutputMode.Model && property.Type == "string")
                    output.AppendLine($"\t\tpublic {property.Type} {property.Name} {{ get; set; }} = string.Empty;");
                else
                    output.AppendLine($"\t\tpublic {property.Type} {property.Name} {{ get; set; }}");
            }
        }

        private void CreateValidationAttributes(Property property, StringBuilder output)
        {
            if (property.PropertyRequired && !IsReadOnlyMode)
                output.AppendLine("\t\t[Required]");

            if (property.ValidateAsEmail && !IsReadOnlyMode && IsClientSide)
                output.AppendLine("\t\t[EmailAddress]");

            if (!string.IsNullOrWhiteSpace(property.RegularExpression) && IsClientSide && !IsReadOnlyMode)
                output.AppendLine($"\t\t[RegularExpression(@\"{property.RegularExpression}\")]");
        }
    }
}

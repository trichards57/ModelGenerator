using ModelGenerator.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGenerator.Generator
{
    public class PropertiesGenerator
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
            var selectedProperties = Enumerable.Empty<Property>();

            switch (_mode)
            {
                case OutputMode.Details:

                    selectedProperties = properties.Where(p => p.IncludeInDetail);
                    break;

                case OutputMode.Summary:
                    selectedProperties = properties.Where(p => p.IncludeInSummary);
                    break;

                case OutputMode.Create:
                    selectedProperties = properties.Where(p => p.IncludeInCreate);
                    break;

                case OutputMode.Update:
                    selectedProperties = properties.Where(p => p.IncludeInUpdate);
                    break;

                default:
                    selectedProperties = properties;
                    break;
            }

            foreach (var p in selectedProperties)
                CreateProperty(p, output);
        }

        public void CreateProperty(Property property, StringBuilder output)
        {
            if (property.PropertyRequired && !IsReadOnlyMode)
                output.AppendLine("\t\t[Required]");

            if (property.ValidateAsEmail && !IsReadOnlyMode && IsClientSide)
                output.AppendLine("\t\t[EmailAddress]");

            if (!string.IsNullOrWhiteSpace(property.DisplayName) && IsClientSide)
                output.AppendLine($"\t\t[Display(Name=\"{property.DisplayName}\")]");

            if (!string.IsNullOrWhiteSpace(property.RegularExpression) && !IsReadOnlyMode)
                output.AppendLine($"\t\t[RegularExpression(@\"{property.RegularExpression}\")]");

            if (!string.IsNullOrWhiteSpace(property.NavigationPropertyId))
                output.AppendLine($"\t\t[ForeignKey({property.NavigationPropertyId})]");

            var type = property.Type;

            if (_mode == OutputMode.Details)
                type += "Details";

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
    }
}

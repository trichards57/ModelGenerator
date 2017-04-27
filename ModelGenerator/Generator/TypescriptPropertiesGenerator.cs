using ModelGenerator.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelGenerator.Generator
{
    internal class TypescriptPropertiesGenerator
    {
        private OutputMode _mode;

        public TypescriptPropertiesGenerator(OutputMode mode)
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
            var type = property.Type;
            var name = char.ToLower(property.Name.First()).ToString() + new string(property.Name.Skip(1).ToArray());

            switch (type)
            {
                case "bool":
                    type = "boolean";
                    break;

                case "int":
                    type = "number";
                    break;

                case "DateTime":
                case "DateTimeOffset":
                case "TimeSpan":
                    type = "string";
                    break;
            }

            if (type.EndsWith("?"))
            {
                name += "?";
                type = type.TrimEnd('?');
            }

            if (property.GenerateAsList)
            {
                if (_mode == OutputMode.Details)
                    type += "Details";

                output.AppendLine($"    {name}: I{type}[];");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(property.NavigationPropertyId))
                    type = "I" + type;

                if (_mode == OutputMode.Details)
                    type += "Details";

                output.AppendLine($"    {name}: {type};");
            }
        }
    }
}

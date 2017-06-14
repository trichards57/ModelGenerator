using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelGenerator.Model;

namespace ModelGenerator.Generator.Typescript
{
    internal class PropertiesGenerator
    {
        private readonly OutputMode _mode;

        public PropertiesGenerator(OutputMode mode)
        {
            _mode = mode;
            ClassMapping.Add("bool", "boolean");
            ClassMapping.Add("float", "number");
            ClassMapping.Add("int", "number");
            ClassMapping.Add("DateTime", "string");
            ClassMapping.Add("DateTimeOffset", "string");
            ClassMapping.Add("TimeSpan", "string");
        }

        public Dictionary<string, string> ClassMapping { get; set; } = new Dictionary<string, string>();

        public void CreateProperties(IEnumerable<Property> properties, StringBuilder output)
        {
            foreach (var p in HelperClasses.FilterProperties(properties, _mode))
                CreateProperty(p, output);
        }

        public void CreateProperty(Property property, StringBuilder output)
        {
            var type = property.Type;
            var name = char.ToLower(property.Name.First()) + new string(property.Name.Skip(1).ToArray());

            if (ClassMapping.ContainsKey(type))
                type = ClassMapping[type];

            if (type.EndsWith("?"))
            {
                name += "?";
                type = type.TrimEnd('?');
            }

            if (property.GenerateAsList)
            {
                output.AppendLine($"    {name}: I{HelperClasses.GetName(type, _mode)}[];");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(property.NavigationPropertyId))
                {
                    type = "I" + HelperClasses.GetName(type, _mode);
                }

                output.AppendLine($"    {name}: {type};");
            }
        }
    }
}

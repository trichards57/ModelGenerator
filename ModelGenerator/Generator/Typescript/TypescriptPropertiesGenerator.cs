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

        public Dictionary<string, string> ClassMapping { get; } = new Dictionary<string, string>();

        public void CreateChangeProperties(IEnumerable<Property> properties, StringBuilder output)
        {
            foreach (var p in HelperClasses.FilterProperties(properties, _mode))
                CreateChangeProperty(p, output);
        }

        public void CreateCopyProperties(IEnumerable<Property> properties, StringBuilder output, string source)
        {
            foreach (var p in HelperClasses.FilterProperties(properties, _mode))
                CreateCopyProperty(p, output, source);
        }

        public void CreateProperties(IEnumerable<Property> properties, StringBuilder output)
        {
            foreach (var p in HelperClasses.FilterProperties(properties, _mode))
                CreateProperty(p, output);
        }

        public void CreateProperty(Property property, StringBuilder output)
        {
            var type = property.Type.TrimEnd('?');
            var name = char.ToLower(property.Name.First()) + new string(property.Name.Skip(1).ToArray());

            if (ClassMapping.ContainsKey(type))
                type = ClassMapping[type];

            if (property.Type.EndsWith("?") || (!string.IsNullOrEmpty(property.NavigationPropertyId) && !property.PropertyRequired))
            {
                name += "?";
            }

            output.AppendLine(property.GenerateAsList ? $"  {name}: {type}[];" : $"  {name}: {type};");
        }

        private void CreateChangeProperty(Property property, StringBuilder output)
        {
            var type = property.Type.TrimEnd('?');
            var name = "onChange" + char.ToUpper(property.Name.First()) + new string(property.Name.Skip(1).ToArray());

            if (ClassMapping.ContainsKey(type))
                type = ClassMapping[type];

            type = $"(value : {type}) => void";

            if (property.Type.EndsWith("?"))
            {
                name += "?";
            }

            output.AppendLine(property.GenerateAsList ? $"  {name}: {type}[];" : $"  {name}: {type};");
        }

        private void CreateCopyProperty(Property property, StringBuilder output, string source)
        {
            var name = char.ToLower(property.Name.First()) + new string(property.Name.Skip(1).ToArray());

            output.AppendLine($"        {name}: {source}.{name},");
        }
    }
}
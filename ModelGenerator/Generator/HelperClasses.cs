using ModelGenerator.Model;
using System.Collections.Generic;
using System.Linq;

namespace ModelGenerator.Generator
{
    internal static class HelperClasses
    {
        public static IEnumerable<Property> FilterProperties(IEnumerable<Property> properties, OutputMode mode)
        {
            switch (mode)
            {
                case OutputMode.Create:
                    return properties.Where(c => c.IncludeInCreate);

                case OutputMode.Details:
                    return properties.Where(c => c.IncludeInDetail);

                case OutputMode.Summary:
                    return properties.Where(c => c.IncludeInSummary);

                case OutputMode.Update:
                    return properties.Where(c => c.IncludeInUpdate);

                case OutputMode.Model:
                    return properties.Where(c => c.IncludeInDatabaseModel);
            }
            return properties;
        }

        public static string GetName(string name, OutputMode mode)
        {
            if (mode == OutputMode.Model)
                return name;

            return name + mode;
        }
    }
}
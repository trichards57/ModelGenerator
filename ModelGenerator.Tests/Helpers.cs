using ModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelGenerator.Tests
{
    public static class Helpers
    {
        public static bool FilterMode(Property prop, OutputMode mode)
        {
            switch (mode)
            {
                case OutputMode.Create:
                    return prop.IncludeInCreate;

                case OutputMode.Details:
                    return prop.IncludeInDetail;

                case OutputMode.Summary:
                    return prop.IncludeInSummary;

                case OutputMode.Update:
                    return prop.IncludeInUpdate;

                default:
                    return true;
            }
        }
    }
}

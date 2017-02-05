﻿using System;
using System.Text;
using ModelGenerator.Model;

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

        public virtual void CreateProperty(Property testProperty, StringBuilder output)
        {
            if (testProperty.PropertyRequired && !IsReadOnlyMode)
                output.AppendLine("\t\t[Required]");

            if (testProperty.ValidateAsEmail && !IsReadOnlyMode && IsClientSide)
                output.AppendLine("\t\t[EmailAddress]");

            if (!string.IsNullOrWhiteSpace(testProperty.DisplayName) && IsClientSide)
                output.AppendLine($"\t\t[DisplayName(\"{testProperty.DisplayName}\")]");

            if (testProperty.GenerateAsList)
            {
                if (_mode == OutputMode.Model)
                    output.AppendLine($"\t\tpublic ICollection<{testProperty.Type}> {testProperty.Name} {{ get; set; }} = new HashSet<{testProperty.Type}>()");
                else
                    output.AppendLine($"\t\tpublic IEnumerable<{testProperty.Type}> {testProperty.Name} {{ get; set; }}");
            }
            else
                output.AppendLine($"\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}");
        }
    }
}
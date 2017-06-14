using FluentAssertions;
using ModelGenerator.Generator;
using ModelGenerator.Model;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ModelGenerator.Tests
{
    public class TypescriptClassGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void ModelShouldCreateClass(OutputMode mode)
        {
            var versionNumber = typeof(Generator.Typescript.ClassGenerator).Assembly.GetName().Version.ToString(3);
            var generator = new Generator.Typescript.ClassGenerator(mode);

            var testModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>()
            };

            var output = new StringBuilder();

            generator.CreateClass(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"interface I{HelperClasses.GetName(testModel.Name, mode)} {{");
            result.Should().Contain($"// Model Generator v{versionNumber}");
        }
    }
}

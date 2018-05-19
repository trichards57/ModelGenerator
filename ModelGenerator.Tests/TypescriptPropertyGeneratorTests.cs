using FluentAssertions;
using ModelGenerator.Generator;
using ModelGenerator.Model;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace ModelGenerator.Tests
{
    public class TypescriptPropertyGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void BasicPropertyLeadsToPropertyOutput(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>()
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: {testProperty.Type};";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void BooleanTypeLeadsToBooleanProperty(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "bool"
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: boolean;";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void DateTimeOffsetTypeLeadsToStringProperty(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "DateTimeOffset"
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: string;";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void DateTimeTypeLeadsToStringProperty(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "DateTime"
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: string;";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void FloatTypeLeadsToNumberProperty(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "float"
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: number;";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void GeneratePropertiesOnlyOutputsItemsInCorrectMode(OutputMode mode)
        {
            var testProperties = new[] {
                // Model Only
                new Property
                {
                    Name = _fixture.Create("model"),
                    Type = _fixture.Create<string>()
                },
                // Create + Model
                new Property
                {
                    Name = _fixture.Create("create"),
                    Type = _fixture.Create<string>(),
                    IncludeInCreate = true
                },
                // Detail + Model
                new Property
                {
                    Name = _fixture.Create("detail"),
                    Type = _fixture.Create<string>(),
                    IncludeInDetail = true
                },
                // Update + Model
                new Property
                {
                    Name = _fixture.Create("update"),
                    Type = _fixture.Create<string>(),
                    IncludeInUpdate = true
                },
                // Summary + Model
                new Property
                {
                    Name = _fixture.Create("summary"),
                    Type = _fixture.Create<string>(),
                    IncludeInSummary = true
                },
                // Detail + Model
                new Property
                {
                    Name = _fixture.Create("detail2"),
                    Type = _fixture.Create<string>(),
                    IncludeInDetail = true,
                }
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperties(testProperties, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProps = testProperties.Where(p => Helpers.FilterMode(p, mode))
                .Select(p => $"{p.Name}: {p.Type};");

            result.Should().Contain(expectedProps);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void IntTypeLeadsToNumberProperty(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "int"
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: number;";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void ListPropertyLeadsToListOutput(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                GenerateAsList = true,
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            generator.ClassMapping.Add(testProperty.Type, "I" + HelperClasses.GetName(testProperty.Type, mode));

            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: I{HelperClasses.GetName(testProperty.Type, mode)}[];";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void NavigationPropertyLeadsToRenamedOutput(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                NavigationPropertyId = _fixture.Create<string>()
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            generator.ClassMapping.Add(testProperty.Type, "I" + HelperClasses.GetName(testProperty.Type, mode));

            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}?: I{HelperClasses.GetName(testProperty.Type, mode)};";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void NullableTypeLeadsToOptionalProperty(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "Test?"
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}?: Test;";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void PropertyShouldMakeFirstLetterLowerCase(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = "Test",
                Type = _fixture.Create<string>()
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"test: {testProperty.Type};";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
        public void TimeSpanTypeLeadsToStringProperty(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "TimeSpan"
            };

            var generator = new Generator.Typescript.PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"{testProperty.Name}: string;";

            result.Should().Be(expected);
        }
    }
}
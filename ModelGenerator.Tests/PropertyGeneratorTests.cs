using FluentAssertions;
using ModelGenerator.Model;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Text;
using ModelGenerator.Generator.CSharp;
using Xunit;

namespace ModelGenerator.Tests
{
    public class PropertyGeneratorTests
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

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"public {testProperty.Type} {testProperty.Name} {{ get; set; }}";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void DisplayNamePropertyAddsDisplayNameAttributeToViewModels(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                DisplayName = _fixture.Create<string>()
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).First();

            var expected = $"[Display(Name = \"{testProperty.DisplayName}\")]";

            result.Should().Be(expected);
        }

        [Fact]
        public void DisplayNamePropertyDoesNotAddDisplayNameAttributeToModel()
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                DisplayName = _fixture.Create<string>()
            };

            var generator = new PropertiesGenerator(OutputMode.Model);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().NotContain(s => s.Contains("Display"));
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void GeneratePropertiesOnlyOutputsItemsInCorrectMode(OutputMode mode)
        {
            var testProperties = new[] {
                // Model Only
                new Property
                {
                    Name = _fixture.Create("Model"),
                    Type = _fixture.Create<string>()
                },
                // Create + Model
                new Property
                {
                    Name = _fixture.Create("Create"),
                    Type = _fixture.Create<string>(),
                    IncludeInCreate = true
                },
                // Detail + Model
                new Property
                {
                    Name = _fixture.Create("Detail"),
                    Type = _fixture.Create<string>(),
                    IncludeInDetail = true
                },
                // Update + Model
                new Property
                {
                    Name = _fixture.Create("Update"),
                    Type = _fixture.Create<string>(),
                    IncludeInUpdate = true
                },
                // Summary + Model
                new Property
                {
                    Name = _fixture.Create("Summary"),
                    Type = _fixture.Create<string>(),
                    IncludeInSummary = true
                },
                // Detail + Model
                new Property
                {
                    Name = _fixture.Create("Detail2"),
                    Type = _fixture.Create<string>(),
                    IncludeInDetail = true,
                }
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperties(testProperties, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProps = testProperties.Where(p => Helpers.FilterMode(p, mode))
                .Select(p => $"public {p.Type} {p.Name} {{ get; set; }}");

            result.Should().Contain(expectedProps);
        }

        [Fact]
        public void ModelListPropertyLeadsToHashSetOutput()
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                GenerateAsList = true,
            };

            var generator = new PropertiesGenerator(OutputMode.Model);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"public ICollection<{testProperty.Type}> {testProperty.Name} {{ get; set; }} = new HashSet<{testProperty.Type}>();";

            result.Should().Be(expected);
        }

        [Fact]
        public void NavigationPropertyIdAddsForeignKeyAttributeInModel()
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                NavigationPropertyId = _fixture.Create<string>()
            };

            var generator = new PropertiesGenerator(OutputMode.Model);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).First();

            var expected = $"[ForeignKey(\"{testProperty.NavigationPropertyId}\")]";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Create), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void NavigationPropertyIdDoesNotAddForeignKeyAttributeInViewModel(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                NavigationPropertyId = _fixture.Create<string>()
            };

            var generator = new PropertiesGenerator(OutputMode.Details);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().NotContain(s => s.StartsWith("[ForeignKey"));
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Summary)]
        public void PropertyRequiredPropertyDoesNotAddRequiredAttributeInReadOnlyMode(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                IncludeInDetail = true,
                IncludeInSummary = true,
                PropertyRequired = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = $"\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Update)]
        public void RegularExpressionAddsRegularExpressionAttributeInWriteMode(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                RegularExpression = _fixture.Create<string>()
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).First();

            var expected = $"[RegularExpression(@\"{testProperty.RegularExpression}\")]";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary)]
        public void RegularExpressionDoesNotAddRegularExpressionAttributeInReadOnlyModeOrInModel(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                RegularExpression = _fixture.Create<string>()
            };

            var generator = new PropertiesGenerator(OutputMode.Details);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().NotContain(s => s.StartsWith("[RegularExpression"));
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Model), InlineData(OutputMode.Update)]
        public void RequiredPropertyAddsRequiredAttributeInWriteableModelsAndModel(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                PropertyRequired = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).First();

            var expected = "[Required]";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Summary)]
        public void RequiredPropertyDoesNotAddRequiredAttributeInReadOnlyModels(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                PropertyRequired = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().NotContain(s => s.StartsWith("[Required"));
        }

        [Fact]
        public void StringPropertyInitialisedInModel()
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "string"
            };

            var generator = new PropertiesGenerator(OutputMode.Model);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"public string {testProperty.Name} {{ get; set; }} = string.Empty;";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void StringPropertyUninitialisedInViewModels(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = "string"
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"public string {testProperty.Name} {{ get; set; }}";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Update)]
        public void ValidateAsEmailPropertyAddsEmailAddressAttributeInWriteMode(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                ValidateAsEmail = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).First();

            const string expected = "[EmailAddress]";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary)]
        public void ValidateAsEmailPropertyDoesNotAddEmailAddressAttributeInReadOnlyModeOrInModel(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                ValidateAsEmail = true
            };

            var generator = new PropertiesGenerator(OutputMode.Details);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().NotContain(s => s.StartsWith("[EmailAddress"));
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void ViewModeListPropertyLeadsToIEnumerableOutput(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                GenerateAsList = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString().Trim();

            var expected = $"public IEnumerable<{testProperty.Type}{mode.ToString()}> {testProperty.Name} {{ get; set; }}";
            result.Should().Be(expected);
        }
    }
}

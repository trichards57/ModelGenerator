using FluentAssertions;
using ModelGenerator.Generator;
using ModelGenerator.Model;
using Ploeh.AutoFixture;
using System;
using System.Text;
using Xunit;

namespace ModelGenerator.Tests
{
    public class PropertiesGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Create), InlineData(OutputMode.Update), InlineData(OutputMode.Model)]
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

            var result = output.ToString();

            string expected;

            switch (mode)
            {
                case OutputMode.Details:
                    expected = $"\t\tpublic {testProperties[2].Type} {testProperties[2].Name} {{ get; set; }}{Environment.NewLine}";
                    expected += $"\t\tpublic {testProperties[5].Type} {testProperties[5].Name} {{ get; set; }}{Environment.NewLine}";
                    break;

                case OutputMode.Summary:
                    expected = $"\t\tpublic {testProperties[4].Type} {testProperties[4].Name} {{ get; set; }}{Environment.NewLine}";
                    break;

                case OutputMode.Create:
                    expected = $"\t\tpublic {testProperties[1].Type} {testProperties[1].Name} {{ get; set; }}{Environment.NewLine}";
                    break;

                case OutputMode.Update:
                    expected = $"\t\tpublic {testProperties[3].Type} {testProperties[3].Name} {{ get; set; }}{Environment.NewLine}";
                    break;

                case OutputMode.Model:
                    expected = $"\t\tpublic {testProperties[0].Type} {testProperties[0].Name} {{ get; set; }}{Environment.NewLine}";
                    expected += $"\t\tpublic {testProperties[1].Type} {testProperties[1].Name} {{ get; set; }}{Environment.NewLine}";
                    expected += $"\t\tpublic {testProperties[2].Type} {testProperties[2].Name} {{ get; set; }}{Environment.NewLine}";
                    expected += $"\t\tpublic {testProperties[3].Type} {testProperties[3].Name} {{ get; set; }}{Environment.NewLine}";
                    expected += $"\t\tpublic {testProperties[4].Type} {testProperties[4].Name} {{ get; set; }}{Environment.NewLine}";
                    expected += $"\t\tpublic {testProperties[5].Type} {testProperties[5].Name} {{ get; set; }}{Environment.NewLine}";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void ModelModeBasicPropertyLeadsToPropertyOutput(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                IncludeInCreate = true,
                IncludeInDetail = true,
                IncludeInSummary = true,
                IncludeInUpdate = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = $"\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

            result.Should().Be(expected);
        }

        [Fact]
        public void ModelModeListPropertyLeadsToHashSetOutput()
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                GenerateAsList = true,
                IncludeInCreate = true,
                IncludeInDetail = true,
                IncludeInSummary = true,
                IncludeInUpdate = true
            };

            var generator = new PropertiesGenerator(OutputMode.Model);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = $"\t\tpublic ICollection<{testProperty.Type}> {testProperty.Name} {{ get; set; }} = new HashSet<{testProperty.Type}>();{Environment.NewLine}";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void NonEmptyDisplayNamePropertyAddsDisplayNameAttributeToClientModels(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                IncludeInCreate = true,
                IncludeInDetail = true,
                IncludeInSummary = true,
                IncludeInUpdate = true,
                DisplayName = _fixture.Create<string>()
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = $"\t\t[Display(Name=\"{testProperty.DisplayName}\")]{Environment.NewLine}\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void OtherModeListPropertyLeadsToICollectionOutput(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                IncludeInCreate = true,
                IncludeInDetail = true,
                IncludeInSummary = true,
                IncludeInUpdate = true,
                GenerateAsList = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = string.Empty;

            if (mode == OutputMode.Details)
                expected = $"\t\tpublic IEnumerable<{testProperty.Type}Details> {testProperty.Name} {{ get; set; }}{Environment.NewLine}";
            else
                expected = $"\t\tpublic IEnumerable<{testProperty.Type}> {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

            result.Should().Be(expected);
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
        [InlineData(OutputMode.Create), InlineData(OutputMode.Model), InlineData(OutputMode.Update)]
        public void RequiredPropertyAddsRequiredAttributeInWriteMode(OutputMode mode)
        {
            var testProperty = new Property
            {
                Name = _fixture.Create<string>(),
                Type = _fixture.Create<string>(),
                IncludeInCreate = true,
                IncludeInUpdate = true,
                PropertyRequired = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = $"\t\t[Required]{Environment.NewLine}\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

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
                IncludeInCreate = true,
                IncludeInUpdate = true,
                ValidateAsEmail = true
            };

            var generator = new PropertiesGenerator(mode);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = $"\t\t[EmailAddress]{Environment.NewLine}\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

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
                IncludeInDetail = true,
                IncludeInSummary = true,
                ValidateAsEmail = true
            };

            var generator = new PropertiesGenerator(OutputMode.Details);
            var output = new StringBuilder();
            generator.CreateProperty(testProperty, output);

            var result = output.ToString();

            var expected = $"\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

            result.Should().Be(expected);
        }
    }
}
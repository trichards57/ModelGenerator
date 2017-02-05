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

            var expected = $"\t\tpublic ICollection<{testProperty.Type}> {testProperty.Name} {{ get; set; }} = new HashSet<{testProperty.Type}>(){Environment.NewLine}";

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

            var expected = $"\t\t[DisplayName(\"{testProperty.DisplayName}\")]{Environment.NewLine}\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

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

            var expected = $"\t\tpublic IEnumerable<{testProperty.Type}> {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

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
                Name = "TestProperty",
                Type = "string",
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

        //// TODO : Once converted to xUnit, make this data driven to test Summary, Model and Detail modes as well
        //[TestMethod]
        //public void ValidateAsEmailPropertyAddsEmailAddressAttributeInReadOnlyMode()
        //{
        //    var testProperty = new Property
        //    {
        //        Name = "TestProperty",
        //        Type = "string",
        //        ValidateAsEmail = true
        //    };

        //    var generator = new PropertiesGenerator(OutputMode.Details);
        //    var output = new StringBuilder();
        //    generator.CreateProperty(testProperty, output);

        //    var result = output.ToString();

        //    var expected = $"\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

        //    Assert.AreEqual(expected, result);
        //}

        //// TODO : Once converted to xUnit, make this data driven to test Update and Create modes as well
        //[TestMethod]
        //public void ValidateAsEmailPropertyAddsEmailAddressAttributeInWriteMode()
        //{
        //    var testProperty = new Property
        //    {
        //        Name = "TestProperty",
        //        Type = "string",
        //        ValidateAsEmail = true
        //    };

        //    var generator = new PropertiesGenerator(OutputMode.Create);
        //    var output = new StringBuilder();
        //    generator.CreateProperty(testProperty, output);

        //    var result = output.ToString();

        //    var expected = $"\t\t[EmailAddress]{Environment.NewLine}\t\tpublic {testProperty.Type} {testProperty.Name} {{ get; set; }}{Environment.NewLine}";

        //    Assert.AreEqual(expected, result);
        //}
    }
}
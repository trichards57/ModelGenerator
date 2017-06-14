using FluentAssertions;
using ModelGenerator.Generator;
using ModelGenerator.Model;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelGenerator.Generator.CSharp;
using Xunit;

namespace ModelGenerator.Tests
{
    public class FunctionGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        private readonly Class TestModel;

        public FunctionGeneratorTests()
        {
            TestModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>
                {
                    new Property {
                        Name = "N1",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = true,
                        IncludeInDetail = false,
                        IncludeInSummary = false,
                        IncludeInCreate = false,
                        GenerateAsList = true
                    },
                    new Property {
                        Name = "N2",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = true,
                        IncludeInDetail = false,
                        IncludeInSummary = false,
                        IncludeInCreate = false,
                        GenerateAsList = false
                    },
                    new Property {
                        Name = "N3",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = true,
                        IncludeInSummary = false,
                        IncludeInCreate = false,
                        GenerateAsList = true
                    },
                    new Property {
                        Name = "N4",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = true,
                        IncludeInSummary = false,
                        IncludeInCreate = false,
                        GenerateAsList = false
                    },
                    new Property {
                        Name = "N5",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = false,
                        IncludeInSummary = true,
                        IncludeInCreate = false,
                        GenerateAsList = true
                    },
                    new Property {
                        Name = "N6",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = false,
                        IncludeInSummary = true,
                        IncludeInCreate = false,
                        GenerateAsList = false
                    },
                    new Property {
                        Name = "N7",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = false,
                        IncludeInSummary = false,
                        IncludeInCreate = true,
                        GenerateAsList = true
                    },
                    new Property {
                        Name = "N8",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = false,
                        IncludeInSummary = false,
                        IncludeInCreate = true,
                        GenerateAsList = false
                    },
                    new Property {
                        Name = "N9",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = true,
                        IncludeInDetail = false,
                        IncludeInSummary = false,
                        IncludeInCreate = false,
                        GenerateAsList = false,
                        NavigationPropertyId = _fixture.Create<string>()
                    },
                    new Property {
                        Name = "N10",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = true,
                        IncludeInSummary = false,
                        IncludeInCreate = false,
                        GenerateAsList = false,
                        NavigationPropertyId = _fixture.Create<string>()
                    },
                    new Property {
                        Name = "N11",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = false,
                        IncludeInSummary = true,
                        IncludeInCreate = false,
                        GenerateAsList = false,
                        NavigationPropertyId = _fixture.Create<string>()
                    },
                    new Property {
                        Name = "N12",
                        Type = _fixture.Create<string>(),
                        IncludeInUpdate = false,
                        IncludeInDetail = false,
                        IncludeInSummary = false,
                        IncludeInCreate = true,
                        GenerateAsList = false,
                        NavigationPropertyId = _fixture.Create<string>()
                    }
                }
            };
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details),
            InlineData(OutputMode.Model), InlineData(OutputMode.Summary),
            InlineData(OutputMode.Update)]
        public void CloneMethodShouldOnlyIncludeReleventNonListProperties(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateCloneMethod(TestModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProperties = TestModel.Properties.Where(p => Helpers.FilterMode(p, mode) && !p.GenerateAsList && string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Select(s => $"item.{s.Name} = {s.Name};");
            var unexpectedProperties = TestModel.Properties.Where(p => !Helpers.FilterMode(p, mode) || p.GenerateAsList || !string.IsNullOrWhiteSpace(p.NavigationPropertyId))
               .Select(s => $"item.{s.Name} = {s.Name};");

            result.First().Should().Be("public object Clone()");
            result.Should().Contain(expectedProperties);
            result.Should().NotContain(unexpectedProperties);
        }

        [Theory]
        [InlineData(OutputMode.Update), InlineData(OutputMode.Details), InlineData(OutputMode.Summary)]
        public void ConstructorShouldOutputCopyAndDefaultConstructors(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateConstructor(TestModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProperties = TestModel.Properties.Where(p => Helpers.FilterMode(p, mode))
                .Select(s => s.GenerateAsList ? $"{s.Name} = item.{s.Name}.Select(i => new {s.Type}{mode.ToString()}(i));" : $"{s.Name} = item.{s.Name};");
            var unexpectedProperties = TestModel.Properties.Where(p => !Helpers.FilterMode(p, mode))
                .Select(s => s.GenerateAsList ? $"{s.Name} = item.{s.Name}.Select(i => new {s.Type}{mode.ToString()}(i));" : $"{s.Name} = item.{s.Name};");

            if (mode == OutputMode.Update)
                result.Should().Contain($"public {TestModel.Name}{mode.ToString()}() {{ }}");

            result.Should().Contain($"public {TestModel.Name}{mode.ToString()}({TestModel.Name} item)");
            result.Should().Contain(expectedProperties);
            result.Should().NotContain(unexpectedProperties);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Model)]
        public void CreateAndModelConstructorShouldOutputNothing(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateConstructor(TestModel, output);

            var result = output.ToString();

            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void CreateEqualsShouldCompareDateTimesWithTolerance(OutputMode mode)
        {
            var model = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>
                {
                    new Property {
                        Name = "N1",
                        Type = "DateTime",
                        IncludeInUpdate = true,
                        IncludeInDetail = true,
                        IncludeInSummary = true,
                        IncludeInCreate = true,
                    },
                    new Property {
                        Name = "N2",
                        Type = "DateTimeOffset",
                        IncludeInUpdate = true,
                        IncludeInDetail = true,
                        IncludeInSummary = true,
                        IncludeInCreate = true,
                    }
                }
            };

            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateEqualsMethods(model, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProperties = model.Properties.Select(s => $"res &= ({s.Name} - other.{s.Name}).TotalSeconds < 30;");

            result.Should().Contain(expectedProperties);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void CreateEqualsShouldCompareOnlyNonListNonValidationProperties(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateEqualsMethods(TestModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProperties = TestModel.Properties.Where(p => Helpers.FilterMode(p, mode) && !p.GenerateAsList && string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Select(s => $"res &= {s.Name}.Equals(other.{s.Name});");
            var unexpectedProperties = TestModel.Properties.Where(p => !Helpers.FilterMode(p, mode) || p.GenerateAsList || !string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Select(s => $"res &= {s.Name}.Equals(other.{s.Name});");

            result.Should().Contain("public override bool Equals(object other)");
            result.Should().Contain($"public bool Equals({TestModel.Name}{(mode != OutputMode.Model ? mode.ToString() : "")} other)");
            result.Should().Contain(expectedProperties);
            result.Should().NotContain(unexpectedProperties);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void CreateHashCodeShouldHashOnlyNonListNonValidationProperties(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateHashCodeMethod(TestModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProperties = TestModel.Properties.Where(p => Helpers.FilterMode(p, mode) && !p.GenerateAsList && string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Select(s => $"hash = hash * 31 + {s.Name}.GetHashCode();");
            var unexpectedProperties = TestModel.Properties.Where(p => !Helpers.FilterMode(p, mode) || p.GenerateAsList || !string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Select(s => $"hash = hash * 31 + {s.Name}.GetHashCode();");

            result.Should().Contain("public override int GetHashCode()");
            result.Should().Contain(expectedProperties);
            result.Should().NotContain(unexpectedProperties);
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void CreateToViewModelFunctionShouldCallConstructorInModelMode(OutputMode mode)
        {
            var generator = new FunctionGenerator(OutputMode.Model);

            var output = new StringBuilder();
            generator.CreateToViewModelMethod(TestModel, mode, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var name = HelperClasses.GetName("To", mode);
            var returnType = HelperClasses.GetName(TestModel.Name, mode);

            result.Should().Contain($"public {returnType} {name}()");
            result.Should().Contain($"return new {returnType}(this);");
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Details), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void CreateToViewModelFunctionShouldHaveNoOutputInOtherModes(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();
            generator.CreateToViewModelMethod(TestModel, OutputMode.Create, output);
            generator.CreateToViewModelMethod(TestModel, OutputMode.Details, output);
            generator.CreateToViewModelMethod(TestModel, OutputMode.Summary, output);
            generator.CreateToViewModelMethod(TestModel, OutputMode.Update, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().BeEmpty();
        }

        [Fact]
        public void CreateViewModelToItemMethodShouldOnlyIncludeReleventNonListProperties()
        {
            var generator = new FunctionGenerator(OutputMode.Create);

            var output = new StringBuilder();

            generator.CreateToItemMethod(TestModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProperties = TestModel.Properties.Where(p => Helpers.FilterMode(p, OutputMode.Create) && !p.GenerateAsList && string.IsNullOrWhiteSpace(p.NavigationPropertyId))
                .Select(s => $"item.{s.Name} = {s.Name};");
            var unexpectedProperties = TestModel.Properties.Where(p => !Helpers.FilterMode(p, OutputMode.Create) || p.GenerateAsList || !string.IsNullOrWhiteSpace(p.NavigationPropertyId))
               .Select(s => $"item.{s.Name} = {s.Name};");

            result.First().Should().Be($"public {TestModel.Name} ToItem()");
            result.Should().Contain(expectedProperties);
            result.Should().NotContain(unexpectedProperties);
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Update)]
        public void NonCreateViewModelToItemMethodShouldOutputNothing(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateToItemMethod(TestModel, output);

            var result = output.ToString();

            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(OutputMode.Details), InlineData(OutputMode.Model), InlineData(OutputMode.Summary), InlineData(OutputMode.Create)]
        public void NonUpdateViewModelUpdateItemMethodShouldOutputNothing(OutputMode mode)
        {
            var generator = new FunctionGenerator(mode);

            var output = new StringBuilder();

            generator.CreateUpdateItemMethod(TestModel, output);

            var result = output.ToString();

            result.Should().BeEmpty();
        }

        [Fact]
        public void UpdateViewModelUpdateItemMethodShouldOnlyIncludeReleventNonListProperties()
        {
            var generator = new FunctionGenerator(OutputMode.Update);

            var output = new StringBuilder();

            generator.CreateUpdateItemMethod(TestModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            var expectedProperties = TestModel.Properties.Where(p => Helpers.FilterMode(p, OutputMode.Update) && !p.GenerateAsList)
                .Select(s => $"item.{s.Name} = {s.Name};");
            var unexpectedProperties = TestModel.Properties.Where(p => !Helpers.FilterMode(p, OutputMode.Update) || p.GenerateAsList)
               .Select(s => $"item.{s.Name} = {s.Name};");

            result.First().Should().Be($"public void UpdateItem({TestModel.Name} item)");
            result.Should().Contain(expectedProperties);
            result.Should().NotContain(unexpectedProperties);
        }
    }
}

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
    public class ClassGeneratorTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void CreateModelClassShouldCreateClass()
        {
            var versionNumber = typeof(ClassGenerator).Assembly.GetName().Version.ToString(3);
            var generator = new ClassGenerator(OutputMode.Model);

            var testModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>()
            };

            var output = new StringBuilder();

            generator.CreateClass(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"public partial class {testModel.Name} : IIdentifiable, ICloneable, IEquatable<{testModel.Name}>");
            result.Should().Contain($"[GeneratedCode(\"Model Generator\", \"v{versionNumber}\"), ExcludeFromCodeCoverage]");
        }

        [Fact]
        public void CreateModelWithDetailClassShouldCreateDetailableClass()
        {
            var versionNumber = typeof(ClassGenerator).Assembly.GetName().Version.ToString(3);
            var generator = new ClassGenerator(OutputMode.Model);

            var testModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>(),
                GenerateDetailModel = true
            };

            var output = new StringBuilder();

            generator.CreateClass(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"public partial class {testModel.Name} : IIdentifiable, ICloneable, IEquatable<{testModel.Name}>, IDetailable<{HelperClasses.GetName(testModel.Name, OutputMode.Details)}>");
            result.Should().Contain($"[GeneratedCode(\"Model Generator\", \"v{versionNumber}\"), ExcludeFromCodeCoverage]");
        }

        [Fact]
        public void CreateModelWithSummaryClassShouldCreateSummarisableClass()
        {
            var versionNumber = typeof(ClassGenerator).Assembly.GetName().Version.ToString(3);
            var generator = new ClassGenerator(OutputMode.Model);

            var testModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>(),
                GenerateSummaryModel = true
            };

            var output = new StringBuilder();

            generator.CreateClass(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"public partial class {testModel.Name} : IIdentifiable, ICloneable, IEquatable<{testModel.Name}>, ISummarisable<{HelperClasses.GetName(testModel.Name, OutputMode.Summary)}>");
            result.Should().Contain($"[GeneratedCode(\"Model Generator\", \"v{versionNumber}\"), ExcludeFromCodeCoverage]");
        }

        [Fact]
        public void CreateViewModelModelCreateClass()
        {
            var versionNumber = typeof(ClassGenerator).Assembly.GetName().Version.ToString(3);
            var generator = new ClassGenerator(OutputMode.Create);

            var testModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>(),
            };

            var output = new StringBuilder();

            generator.CreateClass(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"public partial class {HelperClasses.GetName(testModel.Name, OutputMode.Create)} : ICreateViewModel<{testModel.Name}>");
            result.Should().Contain($"[GeneratedCode(\"Model Generator\", \"v{versionNumber}\"), ExcludeFromCodeCoverage]");
        }

        [Fact]
        public void DetailsModeUsingsShouldIncludeSpecifiedItems()
        {
            var generator = new ClassGenerator(OutputMode.Details);

            var output = new StringBuilder();
            var testModel = new Classes
            {
                RootNamespace = _fixture.Create<string>(),
                ViewModelNamespace = _fixture.Create<string>()
            };

            generator.CreateUsings(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"using System.CodeDom.Compiler;");
            result.Should().Contain($"using System.ComponentModel.DataAnnotations;");
            result.Should().Contain($"using System.Diagnostics.CodeAnalysis;");
            result.Should().Contain($"using System;");
            result.Should().Contain($"using WebsiteHelpers.Interfaces;");

            result.Should().Contain($"using {testModel.RootNamespace}.{testModel.ModelNamespace};");
            result.Should().Contain($"using System.Collections.Generic;");
            result.Should().Contain($"using System.Linq;");

            result.Should().OnlyHaveUniqueItems().And.BeInAscendingOrder();
        }

        [Fact]
        public void EndNamespaceShouldJustCloseNamespace()
        {
            var generator = new ClassGenerator(OutputMode.Model);

            var output = new StringBuilder();
            generator.EndNamespace(output);

            output.ToString().Trim().Should().Be("}");
        }

        [Fact]
        public void ModelModeUsingsShouldIncludeSpecifiedItems()
        {
            var generator = new ClassGenerator(OutputMode.Model);

            var output = new StringBuilder();
            var testModel = new Classes
            {
                RootNamespace = _fixture.Create<string>(),
                ViewModelNamespace = _fixture.Create<string>()
            };

            generator.CreateUsings(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"using System.CodeDom.Compiler;");
            result.Should().Contain($"using System.ComponentModel.DataAnnotations;");
            result.Should().Contain($"using System.Diagnostics.CodeAnalysis;");
            result.Should().Contain($"using System;");
            result.Should().Contain($"using WebsiteHelpers.Interfaces;");

            result.Should().Contain($"using {testModel.RootNamespace}.{testModel.ViewModelNamespace};");
            result.Should().Contain($"using System.Collections.Generic;");
            result.Should().Contain($"using System.ComponentModel.DataAnnotations.Schema;");

            result.Should().OnlyHaveUniqueItems().And.BeInAscendingOrder();
        }

        [Fact]
        public void ModelStartNamespaceShouldUseModelNamespace()
        {
            var generator = new ClassGenerator(OutputMode.Model);

            var output = new StringBuilder();
            var testModel = new Classes
            {
                RootNamespace = _fixture.Create<string>(),
                ModelNamespace = _fixture.Create<string>()
            };

            generator.StartNamespace(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"namespace {testModel.RootNamespace}.{testModel.ModelNamespace}");
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Update), InlineData(OutputMode.Summary)]
        public void OtherModeUsingsShouldIncludeSpecifiedItems(OutputMode mode)
        {
            var generator = new ClassGenerator(mode);

            var output = new StringBuilder();
            var testModel = new Classes
            {
                RootNamespace = _fixture.Create<string>(),
                ModelNamespace = _fixture.Create<string>()
            };

            generator.CreateUsings(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"using System.CodeDom.Compiler;");
            result.Should().Contain($"using System.ComponentModel.DataAnnotations;");
            result.Should().Contain($"using System.Diagnostics.CodeAnalysis;");
            result.Should().Contain($"using System;");
            result.Should().Contain($"using WebsiteHelpers.Interfaces;");

            result.Should().Contain($"using {testModel.RootNamespace}.{testModel.ModelNamespace};");

            result.Should().OnlyHaveUniqueItems().And.BeInAscendingOrder();
        }

        [Theory]
        [InlineData(OutputMode.Summary), InlineData(OutputMode.Details)]
        public void OtherViewModelModelCreateClass(OutputMode mode)
        {
            var versionNumber = typeof(ClassGenerator).Assembly.GetName().Version.ToString(3);
            var generator = new ClassGenerator(mode);

            var testModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>(),
            };

            var output = new StringBuilder();

            generator.CreateClass(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"public partial class {HelperClasses.GetName(testModel.Name, mode)}");
            result.Should().Contain($"[GeneratedCode(\"Model Generator\", \"v{versionNumber}\"), ExcludeFromCodeCoverage]");
        }

        [Fact]
        public void UpdateViewModelModelCreateClass()
        {
            var versionNumber = typeof(ClassGenerator).Assembly.GetName().Version.ToString(3);
            var generator = new ClassGenerator(OutputMode.Update);

            var testModel = new Class
            {
                Name = _fixture.Create<string>(),
                Properties = new List<Property>(),
            };

            var output = new StringBuilder();

            generator.CreateClass(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"public partial class {HelperClasses.GetName(testModel.Name, OutputMode.Update)} : IUpdateViewModel<{testModel.Name}>");
            result.Should().Contain($"[GeneratedCode(\"Model Generator\", \"v{versionNumber}\"), ExcludeFromCodeCoverage]");
        }

        [Theory]
        [InlineData(OutputMode.Create), InlineData(OutputMode.Update), InlineData(OutputMode.Summary), InlineData(OutputMode.Details)]
        public void ViewModelStartNamespaceShouldUseViewModelNamespace(OutputMode mode)
        {
            var generator = new ClassGenerator(mode);

            var output = new StringBuilder();
            var testModel = new Classes
            {
                RootNamespace = _fixture.Create<string>(),
                ViewModelNamespace = _fixture.Create<string>()
            };

            generator.StartNamespace(testModel, output);

            var result = output.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());

            result.Should().Contain($"namespace {testModel.RootNamespace}.{testModel.ViewModelNamespace}");
        }
    }
}

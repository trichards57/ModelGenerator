using FluentAssertions;
using ModelGenerator.Generator;
using ModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace ModelGenerator.Test
{
    [Binding]
    public class PropertyGeneratorSteps
    {
        private PropertiesGenerator _generator;
        private string _output;
        private IEnumerable<Property> _testProperties;
        private Property _testProperty;

        [Given(@"the generator is in (.*) mode")]
        public void GivenTheGeneratorIsInMode(OutputMode mode)
        {
            _generator = new PropertiesGenerator(mode);
        }

        [Given(@"there are a group of properties")]
        public void GivenThereAreAGroupOfProperties(Table table)
        {
            _testProperties = table.CreateSet<Property>();
        }

        [Given(@"there is a single property")]
        public void GivenThereIsASingleProperty(Table table)
        {
            _testProperty = table.CreateInstance<Property>();
        }

        [Then(@"the output properties should be (.*)")]
        public void ThenTheOutputPropertiesShouldBe(string propertyList)
        {
            var names = propertyList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());

            var output = _testProperties.Where(p => names.Contains(p.Name))
                .OrderBy(p => p.Name)
                .Select(p => $"\t\tpublic {p.Type} {p.Name} {{ get; set; }}{Environment.NewLine}");

            var expectedOutput = string.Concat(output);

            _output.Should().Be(expectedOutput);
        }

        [Then(@"the output should be a basic property")]
        public void ThenTheOutputShouldBeABasicProperty()
        {
            var expected = $"\t\tpublic {_testProperty.Type} {_testProperty.Name} {{ get; set; }}";

            _output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .SingleOrDefault(s => s.StartsWith(expected))
                .Should().NotBeNull();
        }

        [Then(@"the output should be a collection property")]
        public void ThenTheOutputShouldBeACollectionProperty()
        {
            var expected = $"\t\tpublic ICollection<{_testProperty.Type}> {_testProperty.Name} {{ get; set; }}";

            _output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .SingleOrDefault(s => s.StartsWith(expected))
                .Should().NotBeNull();
        }

        [Then(@"the output should be an enumerable (.*) property")]
        public void ThenTheOutputShouldBeAEnumerableProperty(OutputMode mode)
        {
            var expected = $"\t\tpublic IEnumerable<{_testProperty.Type + mode.ToString()}> {_testProperty.Name} {{ get; set; }}";

            _output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .SingleOrDefault(s => s.StartsWith(expected))
                .Should().NotBeNull();
        }

        [Then(@"the output should have a (.*) attribute")]
        public void ThenTheOutputShouldHaveAttribute(string attrib)
        {
            var expected = $"\t\t[{attrib}]";

            _output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .SingleOrDefault(s => s.StartsWith(expected))
                .Should().NotBeNull();
        }

        [Then(@"the output should not have a (.*) attribute")]
        public void ThenTheOutputShouldNotHaveAttribute(string attrib)
        {
            var expected = $"\t\t[{attrib}";

            _output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .SingleOrDefault(s => s.StartsWith(expected))
                .Should().BeNull();
        }

        [Then(@"the property should be initialised with a hashset")]
        public void ThenThePropertyShouldBeInitialisedWithAHashset()
        {
            var expected = $"}} = new HashSet<{_testProperty.Type}>();{Environment.NewLine}";

            _output.Should().EndWith(expected);
        }

        [Then(@"the property should be initialised with an empty string")]
        public void ThenThePropertyShouldBeInitialisedWithAnEmptyString()
        {
            var expected = $"}} = string.Empty;{Environment.NewLine}";

            _output.Should().EndWith(expected);
        }

        [Then(@"the property should not be initialised")]
        public void ThenThePropertyShouldNotBeInitialised()
        {
            var expected = $"set; }}";

            _output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .SingleOrDefault(s => s.StartsWith("\t\tpublic") && s.EndsWith(expected))
                .Should().NotBeNull();
        }

        [When(@"you create a set of properties")]
        public void WhenYouCreateASetOfProperties()
        {
            var outBuilder = new StringBuilder();

            _generator.CreateProperties(_testProperties, outBuilder);

            _output = outBuilder.ToString();
        }

        [When(@"you create a single property")]
        public void WhenYouCreateASingleProperty()
        {
            var outBuilder = new StringBuilder();
            _generator.CreateProperty(_testProperty, outBuilder);
            _output = outBuilder.ToString();
        }
    }
}

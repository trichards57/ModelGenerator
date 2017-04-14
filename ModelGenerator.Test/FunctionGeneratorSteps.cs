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
    public class FunctionGeneratorSteps
    {
        private FunctionGenerator _generator;
        private OutputMode _mode;
        private IEnumerable<string> _output;
        private Class _testClass;

        [Given(@"it has the properties")]
        public void GivenHasProperties(Table table)
        {
            _testClass.Properties = new List<Property>(table.CreateSet<Property>());
        }

        [Given(@"I have the test class")]
        public void GivenIHaveTheTestClass(Table table)
        {
            _testClass = table.CreateInstance<Class>();
        }

        [Given(@"the function generator is in (.*) mode")]
        public void GivenTheFunctionGeneratorIsInModelMode(OutputMode mode)
        {
            _generator = new FunctionGenerator(mode);
            _mode = mode;
        }

        [Then(@"it should assign properties ([a-zA-Z0-9,]*)")]
        public void ThenItShouldAssignProperties(string propertyNames)
        {
            var names = propertyNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var properties = _testClass.Properties.Where(p => names.Contains(p.Name))
                .Select(p =>
             {
                 if (p.GenerateAsList)
                     return $"{p.Name} = item.{p.Name}.Select(i => new {p.Type + _mode}(i));";
                 else
                     return $"{p.Name} = item.{p.Name};";
             });

            foreach (var p in properties)
            {
                _output.Should().Contain(p);
            }
        }

        [Then(@"it should assign properties ([a-zA-Z0-9,]*) to ([a-zA-Z0-9,]*)")]
        public void ThenItShouldAssignPropertiesToItem(string propertyNames, string item)
        {
            var names = propertyNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var properties = _testClass.Properties.Where(p => names.Contains(p.Name))
                .Select(p =>
                {
                    if (p.GenerateAsList)
                        return $"{item}.{p.Name} = {p.Name}.Select(i => new {p.Type + _mode}(i));";
                    else
                        return $"{item}.{p.Name} = {p.Name};";
                });

            foreach (var p in properties)
            {
                _output.Should().Contain(p);
            }
        }

        [Then(@"it should not assign properties ([a-zA-Z0-9,]*)")]
        public void ThenItShouldNotAssignProperties(string propertyNames)
        {
            var names = propertyNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var properties = _testClass.Properties.Where(p => names.Contains(p.Name))
                .Select(p =>
                {
                    if (p.GenerateAsList)
                        return $"{p.Name} = item.{p.Name}.Select(i => new {p.Type + _mode}(i));";
                    else
                        return $"{p.Name} = item.{p.Name};";
                });

            foreach (var p in properties)
            {
                _output.Should().NotContain(p);
            }
        }

        [Then(@"it should not assign properties ([a-zA-Z0-9,]*) to ([a-zA-Z0-9,]*)")]
        public void ThenItShouldNotAssignPropertiesToItem(string propertyNames, string item)
        {
            var names = propertyNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var properties = _testClass.Properties.Where(p => names.Contains(p.Name))
                .Select(p =>
                {
                    if (p.GenerateAsList)
                        return $"{item}.{p.Name} =";
                    else
                        return $"{item}.{p.Name} =";
                });

            foreach (var p in properties)
            {
                _output.Should().NotContain(s => s.StartsWith(p));
            }
        }

        [Then(@"there should be a (.*) function (.*)")]
        public void ThenThereShouldBeAFunction(string accessor, string name)
        {
            var expected = $"{accessor} {name}";

            _output.Should().Contain(expected);
        }

        [Then(@"there should be an empty (.*) function (.*)")]
        public void ThenThereShouldBeAnEmptyFunction(string accessor, string name)
        {
            var expected = $"{accessor} {name} {{ }}";

            _output.Should().Contain(expected);
        }

        [Then(@"there should be no output")]
        public void ThenThereShouldBeNoOutput()
        {
            _output.Should().BeEmpty();
        }

        [Then(@"there should not be a (.*) function (.*)")]
        public void ThenThereShouldNotBeAFunction(string accessor, string name)
        {
            var expected = $"{accessor} {name}";

            _output.Should().NotContain(expected);
        }

        [Then(@"there should not be an empty (.*) function (.*)")]
        public void ThenThereShouldNotBeAnEmptyFunction(string accessor, string name)
        {
            var expected = $"{accessor} {name} {{ }}";

            _output.Should().NotContain(expected);
        }

        [When(@"I generate a clone method")]
        public void WhenIGenerateACloneMethod()
        {
            var outBuilder = new StringBuilder();

            _generator.CreateCloneMethod(_testClass, outBuilder);

            _output = outBuilder.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        [When(@"I generate a constructor")]
        public void WhenIGenerateAConstructor()
        {
            var outBuilder = new StringBuilder();

            _generator.CreateConstructor(_testClass, outBuilder);

            _output = outBuilder.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        [When(@"I generate a ToItem method")]
        public void WhenIGenerateAToItemMethod()
        {
            var outBuilder = new StringBuilder();

            _generator.CreateToItemMethod(_testClass, outBuilder);

            _output = outBuilder.ToString().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }
    }
}

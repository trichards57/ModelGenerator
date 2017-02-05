using FluentAssertions;
using ModelGenerator.Model;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace ModelGenerator.Tests
{
    public class ModelTests
    {
        [Fact]
        public void TestDeserialize()
        {
            var serializer = new XmlSerializer(typeof(Models));
            var testData = "<models><model name='List' summary='true' create='true' update='true' details='true'><property name='Id' type='int' detail='true' update='true' summary='true' /><property name='Name' type='string' required='true' detail='true' create='true' summary='true' /><property name='Description' type='string' detail='true' create='true' summary='true' /><property name='UserId' type='string' /></model></models>";

            var testModel = serializer.Deserialize(new StringReader(testData)) as Models;

            testModel.Should().NotBeNull();
            testModel.Items.Should().HaveCount(1);

            testModel.Items[0].Name.Should().Be("List");
        }
    }
}
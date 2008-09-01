using Boo.BooLangService.Intellisense;
using Boo.BooLangService.StringParsing;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class LineEntityParserTests
    {
        [Fact]
        public void SingleVariableReturned()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("instance");

            Assert.Equal(1, entityNames.Length);
            Assert.Equal("instance", entityNames[0].Name);
        }

        [Fact]
        public void SingleMethodNameReturned()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("MethodName()");

            Assert.Equal(1, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
        }

        [Fact]
        public void MethodNameReturnedWithoutParenthesis()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("MethodName()");

            Assert.Equal(1, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
        }

        [Fact]
        public void VariableThenMethodReturned()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("instance.MethodName()");

            Assert.Equal(2, entityNames.Length);
            Assert.Equal("instance", entityNames[0].Name);
            Assert.Equal("MethodName", entityNames[1].Name);
        }

        [Fact]
        public void NestedMethodsReturnedInOrder()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("instance.MethodName().SecondMethod().ThirdMethod()");

            Assert.Equal(4, entityNames.Length);
            Assert.Equal("instance", entityNames[0].Name);
            Assert.Equal("MethodName", entityNames[1].Name);
            Assert.Equal("SecondMethod", entityNames[2].Name);
            Assert.Equal("ThirdMethod", entityNames[3].Name);
        }

        [Fact]
        public void MethodReturnedWithoutParameters()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("instance.MethodName(aParameter).SecondMethod(anotherParameter, secondParameter)");

            Assert.Equal(3, entityNames.Length);
            Assert.Equal("instance", entityNames[0].Name);
            Assert.Equal("MethodName", entityNames[1].Name);
            Assert.Equal("SecondMethod", entityNames[2].Name);
        }

        [Fact]
        public void MethodReturnedWithoutParametersWhenParameterIsStringWithParentheses()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("MethodName(\"(hello)\").Second()");

            Assert.Equal(2, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
            Assert.Equal("Second", entityNames[1].Name);
        }

        [Fact]
        public void MethodReturnedWithoutParametersWhenParameterIsStringWithFullStop()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("MethodName(\"hello.there\").Second()");

            Assert.Equal(2, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
            Assert.Equal("Second", entityNames[1].Name);
        }

        [Fact]
        public void MethodReturnedWithoutParametersWhenParameterIsStringWithEscapedQuote()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("MethodName(\"hello\\\"there\").Second()");

            Assert.Equal(2, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
            Assert.Equal("Second", entityNames[1].Name);

        }

        [Fact]
        public void PropertiesReturned()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("Property.SecondProperty.ThirdProperty");

            Assert.Equal(3, entityNames.Length);
            Assert.Equal("Property", entityNames[0].Name);
            Assert.Equal("SecondProperty", entityNames[1].Name);
            Assert.Equal("ThirdProperty", entityNames[2].Name);
        }

        [Fact]
        public void MixedMethodsAndPropertiesReturned()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("MethodName().Property.Second()");

            Assert.Equal(3, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
            Assert.Equal("Property", entityNames[1].Name);
            Assert.Equal("Second", entityNames[2].Name);
        }

        [Fact]
        public void ReturnAtStartOfLineAreIgnored()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("return MethodName().Property.Second()");

            Assert.Equal(3, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
            Assert.Equal("Property", entityNames[1].Name);
        }

        [Fact]
        public void ImportAtStartOfLineAreIgnored()
        {
            var lineParser = new LineEntityParser();
            var entityNames = lineParser.GetEntityNames("import MethodName().Property.Second()");

            Assert.Equal(3, entityNames.Length);
            Assert.Equal("MethodName", entityNames[0].Name);
            Assert.Equal("Property", entityNames[1].Name);
        }

    }
}
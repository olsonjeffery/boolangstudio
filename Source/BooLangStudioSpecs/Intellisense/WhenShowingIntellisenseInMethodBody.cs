using System.Reflection;
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseInMethodBody : BaseCompilerContext
    {
        [Fact]
        public void ShowLocalVariables()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("aNumber", "aString");
        }

        [Fact]
        public void ShowClassVariables()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("myClassString", "myClassInteger");
        }

        [Fact]
        public void ShowAllMethodsFromSameClass()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowCurrentMethod()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowNamespacesFromReferences()
        {
            CompiledFixtures
                .SetReferences(Assembly.LoadFrom(@"..\..\..\..\Dependencies\boo\build\Boo.Lang.dll"))
                .GetDeclarations()
                .AssertPresenceOf("Boo");
        }
    }
}
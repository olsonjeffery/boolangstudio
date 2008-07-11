
using System;
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseForMemberSuggestion : BaseCompilerContext
    {
        [Fact]
        public void ShowPublicMethodsForClass()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ExcludeConstructorFromList()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertNonPresenceOf(".ctor");
        }

        [Fact]
        public void ExcludePrivateMembersOfOtherClasses()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertNonPresenceOf("APrivateMethod", "AnotherPrivateMethod");
        }

        [Fact]
        public void LocalVariablesCanBeUsed()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("IndexOf", "Substring");
        }

        [Fact]
        public void FieldsCanBeUsed()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("IndexOf", "Substring");
        }

        [Fact]
        public void PropertiesCanBeUsed()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("IndexOf", "Substring");
        }

        [Fact]
        public void ReturnTypesOfMethodCallsCanBeUsed()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("TestMethod", "AnotherTestMethod");
        }

        [Fact]
        public void DirectInstantiationsCanBeUsed()
        {
            // not sure what to really call this, but it's:
            // c#:  new MyClass().
            // boo: MyClass().

            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("TestMethod", "AnotherTestMethod");
        }
    }
}
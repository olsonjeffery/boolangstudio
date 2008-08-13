
using System;
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseForMemberSuggestion : BaseCompilerContext
    {
        [Fact]
        public void IncludeOnlyStaticMethodsForClass()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("FirstStaticMethod", "SecondStaticMethod")
                .AssertNonPresenceOf("AnInstanceMethod");
        }

        [Fact]
        public void IncludePublicMethodsForInstanceOfClass()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("FirstMethod", "SecondMethod");
        }

        [Fact]
        public void IncludePublicMembersForInstanceOfImportedClass()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("Copy", "Join");
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
        public void ExcludeProtectedMembersOfOtherClasses()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertNonPresenceOf("AProtectedMethod", "AnotherProtectedMethod");
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
        public void ReturnTypesOfMethodCallsOnAnObjectCanBeUsed()
        {
            // object.method.
            // currently trys to find a reference point of the whole thing
            // (something literally declared as object.method) but it should
            // really split on "." then find each individually. Find object,
            // then find method on object.
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("IndexOf", "Substring");
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
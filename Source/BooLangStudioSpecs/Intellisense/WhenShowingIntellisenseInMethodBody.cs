using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseInMethodBody : BaseIntellisenseContext
    {
        [Fact]
        public void ShowLocalVariables()
        {
            var declarations = CompiledFixtures.GetDeclarations();

            ValidatePresenceOfDeclarations(declarations, "aNumber", "aString");
        }

        [Fact]
        public void ShowClassVariables()
        {
            var declarations = CompiledFixtures.GetDeclarations();

            ValidatePresenceOfDeclarations(declarations, "myClassString", "myClassInteger");
        }

        [Fact]
        public void ShowAllMethodsFromSameClass()
        {
            var declarations = CompiledFixtures.GetDeclarations();

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowCurrentMethod()
        {
            var declarations = CompiledFixtures.GetDeclarations();

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowNamespacesFromReferences()
        {
            var declarations = CompiledFixtures
                .SetReferences("System", "Boo")
                .GetDeclarations();

            ValidatePresenceOfDeclarations(declarations, "System", "Boo");
        }
    }
}
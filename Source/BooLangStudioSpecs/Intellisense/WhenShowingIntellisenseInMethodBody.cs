using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseInMethodBody : BaseIntellisenseContext
    {
        [Fact]
        public void ShowLocalVariables()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation);
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "aNumber", "aString");
        }

        [Fact]
        public void ShowClassVariables()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation);
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "myClassString", "myClassInteger");
        }

        [Fact]
        public void ShowAllMethodsFromSameClass()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation);
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowCurrentMethod()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation);
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowNamespacesFromReferences()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation,
                "System", "Boo");
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "System", "Boo");
        }
    }
}
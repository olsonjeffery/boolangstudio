
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseForMemberSuggestion : BaseIntellisenseContext
    {
        [Fact]
        public void ShowPublicMethodsForClass()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation);
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ExcludeConstructorFromList()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation);
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidateNonPresenceOfDeclarations(declarations, ".ctor");
        }
    }
}
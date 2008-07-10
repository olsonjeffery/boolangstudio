using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseAtImportStatement : BaseIntellisenseContext
    {
        [Fact]
        public void ShowAllReferencedNamespaces()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();

            var finder = CreateFinder(compilationOutput.Project, compilationOutput.CaretLocation.LineSource,
                "Boo", "BooLangStudio");
            var declarations = finder.Find(compilationOutput.CaretLocation, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "Boo", "BooLangStudio");
        }
    }
}
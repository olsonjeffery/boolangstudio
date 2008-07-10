using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseAtImportStatement : BaseIntellisenseContext
    {
        [Fact]
        public void ShowAllReferencedNamespaces()
        {
            var declarations = CompiledFixtures
                .SetReferences("Boo", "BooLangStudio")
                .GetDeclarations();

            ValidatePresenceOfDeclarations(declarations, "Boo", "BooLangStudio");
        }
    }
}
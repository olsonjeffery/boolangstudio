using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseAtImportStatement : BaseCompilerContext
    {
        [Fact]
        public void ShowAllReferencedNamespaces()
        {
            CompiledFixtures
                .SetReferences("Boo", "BooLangStudio")
                .GetDeclarations()
                .AssertPresenceOf("Boo", "BooLangStudio");
        }
    }
}
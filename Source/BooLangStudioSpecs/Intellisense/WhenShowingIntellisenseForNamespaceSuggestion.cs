using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseForNamespaceSuggestion : BaseCompilerContext
    {
        [Fact]
        public void IncludeSubNamespaces()
        {
            CompiledFixtures
                .GetDeclarations()
                .AssertPresenceOf("Collections", "IO");
        }
    }
}

using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseForMemberSuggestion : BaseIntellisenseContext
    {
        [Fact]
        public void ShowPublicMethodsForClass()
        {
            var declarations = CompiledFixtures.GetDeclarations();

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ExcludeConstructorFromList()
        {
            var declarations = CompiledFixtures.GetDeclarations();

            ValidateNonPresenceOfDeclarations(declarations, ".ctor");
        }
    }
}
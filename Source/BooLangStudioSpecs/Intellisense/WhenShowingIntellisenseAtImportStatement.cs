using System.Reflection;
using BooLangService;
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
                .SetReferences(
                    Assembly.LoadFrom(@"..\..\..\..\Dependencies\boo\bin\Boo.Lang.dll"))
                .GetDeclarations()
                .AssertPresenceOf("Boo");
        }
    }
}
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
                .SetReferences(Assembly.LoadFrom(@"..\..\..\..\Dependencies\boo\build\Boo.Lang.dll"))
                .GetDeclarations()
                .AssertPresenceOf("Boo");
        }

        [Fact]
        public void ShowSubNamespacesForPartialImport()
        {
            CompiledFixtures
                .SetReferences(Assembly.LoadFrom(@"..\..\..\..\Dependencies\boo\build\Boo.Lang.dll"))
                .GetDeclarations()
                .AssertPresenceOf("Lang");
        }

        [Fact]
        public void ExcludeExposedTypes()
        {
            CompiledFixtures
                .SetReferences(Assembly.LoadFrom(@"..\..\..\..\Dependencies\boo\build\Boo.Lang.dll"))
                .GetDeclarations()
                .AssertNonPresenceOf("ICallable", "IQuackFu");
        }
    }
}
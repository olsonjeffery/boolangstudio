using System.Reflection;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseForNamespaceSuggestion : BaseCompilerContext
    {
        [Fact]
        public void IncludeSubNamespaces()
        {
            CompiledFixtures
                .SetReferences(Assembly.LoadFrom(@"..\..\..\..\Dependencies\boo\build\Boo.Lang.dll"))
                .GetDeclarations()
                .AssertPresenceOf("Lang");
        }

        [Fact]
        public void IncludeExposedTypes()
        {
            CompiledFixtures
                .SetReferences(Assembly.LoadFrom(@"..\..\..\..\Dependencies\boo\build\Boo.Lang.dll"))
                .GetDeclarations()
                .AssertPresenceOf("ICallable", "IQuackFu");
        }
    }
}
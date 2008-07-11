using System.Collections.Generic;
using Boo.BooLangService;
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenParsingMultipleFilesForIntellisense : BaseCompilerContext
    {
        [Fact]
        public void InstancesAreScopedToFileWhenMethodsHaveSameName()
        {
            var declarations = CompiledFixtures.GetDeclarations();
        }
    }
}
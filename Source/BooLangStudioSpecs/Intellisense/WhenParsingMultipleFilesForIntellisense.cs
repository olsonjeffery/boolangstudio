using System.Collections.Generic;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenParsingMultipleFilesForIntellisense : BaseCompilerContext
    {
        [Fact]
        public void InstancesAreScopedToFileWhenMethodsHaveSameName()
        {
            var compilationOutput = Fixtures.CompileForCurrentMethod();
        }
    }
}
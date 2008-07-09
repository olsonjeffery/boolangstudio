using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseAtImportStatement : BaseDisplayIntellisenseContext
    {
        [Fact]
        public void ShowAllReferencedNamespaces()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
import ~
");

            var finder = CreateFinder(document, line,
                                      "Boo", "BooLangStudio");
            var declarations = finder.Find(lineNum, colNum, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "Boo", "BooLangStudio");
        }
    }
}
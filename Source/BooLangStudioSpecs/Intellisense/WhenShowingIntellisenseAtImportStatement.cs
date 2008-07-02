using MbUnit.Framework;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [TestFixture]
    public class WhenShowingIntellisenseAtImportStatement : BaseDisplayIntellisenseContext
    {
        [Test]
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
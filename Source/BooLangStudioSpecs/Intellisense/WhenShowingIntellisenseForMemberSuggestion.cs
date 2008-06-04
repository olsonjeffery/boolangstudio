using MbUnit.Framework;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [TestFixture]
    public class WhenShowingIntellisenseForMemberSuggestion : BaseDisplayIntellisenseContext
    {
        [Test]
        public void ShowPublicMethodsForClass()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
class TheClassToReference:
  def FirstMethod():
    pass
  
  def SecondMethod():
    pass

class MyClass:
  def MyMethod():
    instance = TheClassToReference()
    instance.~
");

            var finder = CreateFinder(document, line);
            var declarations = finder.Find(lineNum, colNum);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }
    }
}
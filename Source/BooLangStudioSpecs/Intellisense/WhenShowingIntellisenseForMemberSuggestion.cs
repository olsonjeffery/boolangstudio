using MbUnit.Framework;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [TestFixture]
    public class WhenShowingIntellisenseForMemberSuggestion : BaseDisplayIntellisenseContext
    {
        [Test]
        public void ShowPublicMethodsForClass()
        {
            var declarations = GetDeclarations(@"
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

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Test]
        public void ExcludeConstructorFromList()
        {
            var declarations = GetDeclarations(@"
class MyClass:
  def MyMethod():
    instance = ""a string""
    instance.~
");

            ValidateNonPresenceOfDeclarations(declarations, ".ctor");
        }
    }
}
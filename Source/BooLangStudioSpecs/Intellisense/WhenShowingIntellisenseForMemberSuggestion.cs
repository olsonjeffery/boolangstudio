
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseForMemberSuggestion : BaseDisplayIntellisenseContext
    {
        [Fact]
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

        [Fact]
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
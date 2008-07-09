using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenShowingIntellisenseInMethodBody : BaseDisplayIntellisenseContext
    {
        [Fact]
        public void ShowLocalVariables()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
class MyClass:
  def MyMethod():
    aNumber = 10
    aString = ""testing""
    ~
    return aString
");

            var finder = CreateFinder(document, line);
            var declarations = finder.Find(lineNum, colNum, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "aNumber", "aString");
        }

        [Fact]
        public void ShowClassVariables()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
class MyClass:
  private myClassString = ""Testing...""
  public myClassInteger = 10

  def MyMethod():
    ~
    pass
");

            var finder = CreateFinder(document, line);
            var declarations = finder.Find(lineNum, colNum, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "myClassString", "myClassInteger");

        }

        [Fact]
        public void ShowAllMethodsFromSameClass()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
class MyClass:
  def FirstMethod():
    pass

  def SecondMethod() as string:
    return ""I'm the second method""

  def ThirdMethod():
    ~
    pass
");

            var finder = CreateFinder(document, line);
            var declarations = finder.Find(lineNum, colNum, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowCurrentMethod()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
class MyClass:
  def FirstMethod():
    pass

  def SecondMethod() as string:
    return ""I'm the second method""

  def ThirdMethod():
    ~
    pass
");

            var finder = CreateFinder(document, line);
            var declarations = finder.Find(lineNum, colNum, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Fact]
        public void ShowNamespacesFromReferences()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
class MyClass:
  def MyMethod():
    ~
    pass
");

            var finder = CreateFinder(document, line,
                                      "System", "Boo");
            var declarations = finder.Find(lineNum, colNum, ParseReason.None);

            ValidatePresenceOfDeclarations(declarations, "System", "Boo");
        }
    }
}
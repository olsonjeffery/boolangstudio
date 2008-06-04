using Context = MbUnit.Framework.TestFixtureAttribute;
using Spec = MbUnit.Framework.TestAttribute;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [Context]
    public class WhenShowingIntellisenseInMethodBody : BaseDisplayIntellisenseContext
    {
        [Spec]
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
            var declarations = finder.Find(lineNum, colNum);

            ValidatePresenceOfDeclarations(declarations, "aNumber", "aString");
        }

        [Spec]
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
            var declarations = finder.Find(lineNum, colNum);

            ValidatePresenceOfDeclarations(declarations, "myClassString", "myClassInteger");

        }

        [Spec]
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
            var declarations = finder.Find(lineNum, colNum);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Spec]
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
            var declarations = finder.Find(lineNum, colNum);

            ValidatePresenceOfDeclarations(declarations, "FirstMethod", "SecondMethod");
        }

        [Spec]
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
            var declarations = finder.Find(lineNum, colNum);

            ValidatePresenceOfDeclarations(declarations, "System", "Boo");
        }
    }
}
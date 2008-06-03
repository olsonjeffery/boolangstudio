using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using BooLangService;
using MbUnit.Framework;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Context = MbUnit.Framework.TestFixtureAttribute;
using Spec = MbUnit.Framework.TestAttribute;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [Context]
    public class WhenShowingIntellisenseAtImportStatement : BaseDisplayIntellisenseContext
    {
        [Spec]
        public void ShowAllReferencedNamespaces()
        {
            string line;
            int lineNum, colNum;
            var document = Compile(out line, out lineNum, out colNum, @"
import ~
");

            var finder = CreateFinder(document, line,
                "Boo", "BooLangStudio");
            var declarations = finder.Find(lineNum, colNum);

            ValidatePresenceOfDeclarations(declarations, "Boo", "BooLangStudio");
        }
    }
    
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

    [Context]
    public class WhenParsingCodeForIntellisense : BaseCompilerContext
    {
        [Spec]
        public void CompiledDocumentShouldAlwaysContainADocumentTreeNode()
        {
            CompiledDocument document = Compile(@"
pass
            ");

            Assert.IsTrue(document.ParseTree is DocumentTreeNode,
                          "Parsed document should always start with a DocumentTreeNode.");
        }

        [Spec]
        public void ClassesShouldBeParsed()
        {
            CompiledDocument document = Compile(@"
class MyClass:
  pass
");
            var children = document.ParseTree.Children;

            Assert.IsTrue(children.Count == 1,
                "Should only have one child, the Class, but found: " + children.Count);

            var classNode = children[0];

            Assert.IsTrue(classNode is ClassTreeNode,
                "First child should be the class.");
            Assert.IsTrue(classNode.Name == "MyClass",
                "Name should match the name in the source.");
        }

        [Spec]
        public void InterfacesShouldBeParsed()
        {
            CompiledDocument document = Compile(@"
interface MyInterface:
  pass
");
            var children = document.ParseTree.Children;

            Assert.IsTrue(children.Count == 1,
                "Should only have one child, the Interface, but found: " + children.Count);

            var interfaceNode = children[0];

            Assert.IsTrue(interfaceNode is InterfaceTreeNode,
                "First child should be the interface.");
            Assert.IsTrue(interfaceNode.Name == "MyInterface",
                "Name should match the name in the source.");
        }
    }
}
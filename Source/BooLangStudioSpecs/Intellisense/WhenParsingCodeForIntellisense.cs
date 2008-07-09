using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using MbUnit.Framework;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [TestFixture]
    public class WhenParsingCodeForIntellisense : BaseCompilerContext
    {
        [Test]
        public void CompiledDocumentShouldAlwaysContainADocumentTreeNode()
        {
            CompiledProject document = Compile(@"
pass
");

            Assert.IsTrue(document.ParseTree is DocumentTreeNode,
                          "Parsed document should always start with a DocumentTreeNode.");
        }

        [Test]
        public void ClassesShouldBeParsed()
        {
            CompiledProject document = Compile(@"
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

        [Test]
        public void InterfacesShouldBeParsed()
        {
            CompiledProject document = Compile(@"
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

        [Test]
        public void MethodParametersShouldBeAddedToMethod()
        {
            CompiledProject document = Compile(@"
class MyClass:
  def MyMethod(param1 as string, param2):
    pass
");
            
            //                                       .root     .class      .method
            var methodNode = (MethodTreeNode)document.ParseTree.Children[0].Children[0];

            Assert.IsTrue(methodNode.Parameters.Count == 2, "Should be two parameters.");
            Assert.IsTrue(methodNode.Parameters[0].Name == "param1", "First parameter should be named param1.");
            Assert.IsTrue(methodNode.Parameters[0].Type == "string", "First parameter should be a string.");
            Assert.IsTrue(methodNode.Parameters[1].Name == "param2", "First parameter should be named param2.");
        }
    }
}
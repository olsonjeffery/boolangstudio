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
            CompiledDocument document = Compile(@"
pass
");

            Assert.IsTrue(document.ParseTree is DocumentTreeNode,
                          "Parsed document should always start with a DocumentTreeNode.");
        }

        [Test]
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

        [Test]
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
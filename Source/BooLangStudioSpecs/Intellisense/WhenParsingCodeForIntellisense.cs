using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenParsingCodeForIntellisense : BaseCompilerContext
    {
        [Fact]
        public void CompiledDocumentShouldAlwaysContainADocumentTreeNode()
        {
            CompiledProject document = Compile(@"
pass
");

            Assert.True(document.ParseTree is DocumentTreeNode,
                          "Parsed document should always start with a DocumentTreeNode.");
        }

        [Fact]
        public void ClassesShouldBeParsed()
        {
            CompiledProject document = Compile(@"
class MyClass:
  pass
");
            var children = document.ParseTree.Children;

            Assert.True(children.Count == 1,
                          "Should only have one child, the Class, but found: " + children.Count);

            var classNode = children[0];

            Assert.True(classNode is ClassTreeNode,
                          "First child should be the class.");
            Assert.True(classNode.Name == "MyClass",
                          "Name should match the name in the source.");
        }

        [Fact]
        public void InterfacesShouldBeParsed()
        {
            CompiledProject document = Compile(@"
interface MyInterface:
  pass
");
            var children = document.ParseTree.Children;

            Assert.True(children.Count == 1,
                          "Should only have one child, the Interface, but found: " + children.Count);

            var interfaceNode = children[0];

            Assert.True(interfaceNode is InterfaceTreeNode,
                          "First child should be the interface.");
            Assert.True(interfaceNode.Name == "MyInterface",
                          "Name should match the name in the source.");
        }

        [Fact]
        public void MethodParametersShouldBeAddedToMethod()
        {
            CompiledProject document = Compile(@"
class MyClass:
  def MyMethod(param1 as string, param2):
    pass
");
            
            //                                       .root     .class      .method
            var methodNode = (MethodTreeNode)document.ParseTree.Children[0].Children[0];

            Assert.True(methodNode.Parameters.Count == 2, "Should be two parameters.");
            Assert.True(methodNode.Parameters[0].Name == "param1", "First parameter should be named param1.");
            Assert.True(methodNode.Parameters[0].Type == "string", "First parameter should be a string.");
            Assert.True(methodNode.Parameters[1].Name == "param2", "First parameter should be named param2.");
        }
    }
}
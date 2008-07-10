using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenParsingCodeForIntellisense : BaseCompilerContext
    {
        [Fact]
        public void FirstReturnedNodeIsProjectTreeNode()
        {
            var project = Compile(@"
pass
");
            Assert.IsType<ProjectTreeNode>(project.ParseTree);
        }

        [Fact]
        public void FirstChildIsDocumentTreeNode()
        {
            var project = Compile(@"
pass
");
            Assert.IsType<DocumentTreeNode>(project.ParseTree.Children[0]);
        }

        [Fact]
        public void ClassesShouldBeParsed()
        {
            var project = Compile(@"
class MyClass:
  pass
");
            var document = project.ParseTree.Children[0];
            var classNode = document.Children[0];

            Assert.IsType<ClassTreeNode>(classNode);
            Assert.Equal("MyClass", classNode.Name);
        }

        [Fact]
        public void InterfacesShouldBeParsed()
        {
            var project = Compile(@"
interface MyInterface:
  pass
");
            var document = project.ParseTree.Children[0];
            var interfaceNode = document.Children[0];

            Assert.IsType<InterfaceTreeNode>(interfaceNode);
            Assert.Equal("MyInterface", interfaceNode.Name);
        }

        [Fact]
        public void MethodParametersShouldBeAddedToMethod()
        {
            var project = Compile(@"
class MyClass:
  def MyMethod(param1 as string, param2):
    pass
");
            
            //                                       .project .document   .class      .method
            var methodNode = (MethodTreeNode)project.ParseTree.Children[0].Children[0].Children[0];
            var numberOfParameters = methodNode.Parameters.Count;
            var firstParameter = methodNode.Parameters[0];
            var secondParameter = methodNode.Parameters[1];

            Assert.Equal(2, numberOfParameters);
            Assert.Equal("param1", firstParameter.Name);
            Assert.Equal("string", firstParameter.Type);
            Assert.Equal("param2", secondParameter.Name);
        }
    }
}
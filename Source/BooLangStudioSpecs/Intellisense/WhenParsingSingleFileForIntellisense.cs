using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenParsingSingleFileForIntellisense : BaseCompilerContext
    {
        [Fact]
        public void FirstReturnedNodeIsProjectTreeNode()
        {
            Assert.IsType<ProjectTreeNode>(CompiledFixtures.Project.ParseTree);
        }

        [Fact]
        public void FirstChildIsDocumentTreeNode()
        {
            var project = CompiledFixtures.Project;

            Assert.IsType<DocumentTreeNode>(project.ParseTree.Children[0]);
        }

        [Fact]
        public void ClassesShouldBeParsed()
        {
            var project = CompiledFixtures.Project;

            var document = project.ParseTree.Children[0];
            var classNode = document.Children[0];

            Assert.IsType<ClassTreeNode>(classNode);
            Assert.Equal("MyClass", classNode.Name);
        }

        [Fact]
        public void InterfacesShouldBeParsed()
        {
            var project = CompiledFixtures.Project;

            var document = project.ParseTree.Children[0];
            var interfaceNode = document.Children[0];

            Assert.IsType<InterfaceTreeNode>(interfaceNode);
            Assert.Equal("MyInterface", interfaceNode.Name);
        }

        [Fact]
        public void MethodParametersShouldBeAddedToMethod()
        {
            var project = CompiledFixtures.Project;
            
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
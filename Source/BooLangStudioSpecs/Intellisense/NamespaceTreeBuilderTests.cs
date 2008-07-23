using Boo.BooLangService.Intellisense;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class NamespaceTreeBuilderTests
    {
        [Fact]
        public void DoesntAddAnythingForANullNamespace()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build(new string[] { null });

            Assert.Empty(namespaceTree);
        }

        [Fact]
        public void OneSingleLevelNamespaceProducesOneLeaf()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("Boo");

            Assert.Equal(1, namespaceTree.Count);
            Assert.Equal("Boo", namespaceTree[0].Name);
        }

        [Fact]
        public void TwoSingleLevelNamespacesProducesTwoLeafs()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("Boo", "System");

            Assert.Equal(2, namespaceTree.Count);
            Assert.Equal("Boo", namespaceTree[0].Name);
            Assert.Equal("System", namespaceTree[1].Name);
        }

        [Fact]
        public void OneDoubleLevelNamespaceProducesOneTopLevelLeaf()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("Boo.Lang");

            Assert.Equal(1, namespaceTree.Count);
            Assert.Equal("Boo", namespaceTree[0].Name);
        }

        [Fact]
        public void TwoDoubleLevelNamespacesProducesTwoTopLevelLeafs()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("Boo.Lang", "System.Collections");

            Assert.Equal(2, namespaceTree.Count);
            Assert.Equal("Boo", namespaceTree[0].Name);
            Assert.Equal("System", namespaceTree[1].Name);
        }

        [Fact]
        public void OneDoubleLevelNamespaceProducesOneChildLeaf()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("Boo.Lang");
            var booLeaf = namespaceTree[0];

            Assert.Equal(1, booLeaf.Children.Count);
            Assert.Equal("Lang", booLeaf.Children[0].Name);
        }

        [Fact]
        public void TwoDoubleLevelNamespacesThatHaveTheSameRootProducesOneTopLevelLeaf()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("Boo.Lang", "Boo.Parser");

            Assert.Equal(1, namespaceTree.Count);
            Assert.Equal("Boo", namespaceTree[0].Name);
        }

        [Fact]
        public void TwoDoubleLevelNamespacesThatHaveTheSameRootProducesTwoChildLeafs()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("Boo.Lang", "Boo.Parser");
            var booLeaf = namespaceTree[0];

            Assert.Equal(2, booLeaf.Children.Count);
            Assert.Equal("Lang", booLeaf.Children[0].Name);
            Assert.Equal("Parser", booLeaf.Children[1].Name);
        }

        [Fact]
        public void ReallyDeepNamespaceProducesCorrectTree()
        {
            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build("My.Namespace.Is.Really.Long");

            Assert.Equal("My", namespaceTree[0].Name);
            Assert.Equal("Namespace", namespaceTree[0].Children[0].Name);
            Assert.Equal("Is", namespaceTree[0].Children[0].Children[0].Name);
            Assert.Equal("Really", namespaceTree[0].Children[0].Children[0].Children[0].Name);
            Assert.Equal("Long", namespaceTree[0].Children[0].Children[0].Children[0].Children[0].Name);
        }
    }
}
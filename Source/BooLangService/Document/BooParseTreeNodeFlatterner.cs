using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace BooLangService
{
    public class BooParseTreeNodeFlatterner
    {
        public BooParseTreeNodeList FlattenFrom(IBooParseTreeNode node)
        {
            BooParseTreeNodeList flattened = new BooParseTreeNodeList();

            // add anything "below" the scope (e.g. locals in a method, methods in a class)
            flattened.AddRange(FlattenDown(node.Children));

            // add anything "above" the scope (e.g. classes in a method)
            // also walks sideways (e.g. other methods in same class if scope is a method)
            flattened.AddRange(FlattenUp(node));

            return flattened;
        }

        private IList<IBooParseTreeNode> FlattenUp(IBooParseTreeNode node)
        {
            List<IBooParseTreeNode> flattened = new List<IBooParseTreeNode>();
            IBooParseTreeNode parent = node;

            while ((parent = parent.Parent) != null)
            {
                foreach (IBooParseTreeNode sibling in parent.Children)
                {
                    flattened.Add(sibling);
                }

                if (!(parent is RootTreeNode))
                    flattened.Add(parent); // don't add the root node, because it's only there as a container
            }

            return flattened;
        }

        private IList<IBooParseTreeNode> FlattenDown(IList<IBooParseTreeNode> nodes)
        {
            List<IBooParseTreeNode> flattened = new List<IBooParseTreeNode>();

            foreach (IBooParseTreeNode node in nodes)
            {
                flattened.Add(node);
                flattened.AddRange(FlattenDown(node.Children));
            }

            return flattened;
        }
    }
}
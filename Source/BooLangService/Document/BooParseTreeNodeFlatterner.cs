using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// 
    /// </summary>
    public class BooParseTreeNodeFlatterner
    {
        /// <summary>
        /// Flattens a tree into a list.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public BooParseTreeNodeList FlattenFrom(IBooParseTreeNode node)
        {
            BooParseTreeNodeList flattened = new IntellisenseNodeList();

            // add anything "below" the scope (e.g. locals in a method, methods in a class)
            flattened.AddRange(FlattenDown(node.Children));

            // add anything "above" the scope (e.g. classes in a method)
            // also walks sideways (e.g. other methods in same class if scope is a method)
            flattened.AddRange(FlattenUp(node));

            return flattened;
        }

        private IList<IBooParseTreeNode> FlattenUp(IBooParseTreeNode node)
        {
            IList<IBooParseTreeNode> flattened = new IntellisenseNodeList();
            IBooParseTreeNode parent = node;

            while ((parent = parent.Parent) != null)
            {
                foreach (IBooParseTreeNode sibling in parent.Children)
                {
                    flattened.Add(sibling);
                }

                flattened.Add(parent);
            }

            return flattened;
        }

        private IList<IBooParseTreeNode> FlattenDown(IList<IBooParseTreeNode> nodes)
        {
            BooParseTreeNodeList flattened = new IntellisenseNodeList();

            foreach (IBooParseTreeNode node in nodes)
            {
                flattened.Add(node);
                flattened.AddRange(FlattenDown(node.Children));
            }

            return flattened;
        }
    }
}
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
            FlattenDown(node.Children, ref flattened);

            // add anything "above" the scope (e.g. classes in a method)
            // also walks sideways (e.g. other methods in same class if scope is a method)
            FlattenUp(node, ref flattened);

            return flattened;
        }

        private void FlattenUp(IBooParseTreeNode node, ref BooParseTreeNodeList flattened)
        {
            IBooParseTreeNode parent = node;

            while ((parent = parent.Parent) != null)
            {
                foreach (IBooParseTreeNode sibling in parent.Children)
                {
                    flattened.Add(sibling);

                    // hack to get cross-file intellisense working
                    // todo: make this better
                    if (sibling is DocumentTreeNode)
                    {
                        foreach (var documentChild in sibling.Children)
                        {
                            flattened.Add(documentChild);
                        }
                    }
                }

                flattened.Add(parent);
            }
        }

        private void FlattenDown(IList<IBooParseTreeNode> nodes, ref BooParseTreeNodeList flattened)
        {
            foreach (IBooParseTreeNode node in nodes)
            {
                flattened.Add(node);
                
                FlattenDown(node.Children, ref flattened);
            }
        }
    }
}
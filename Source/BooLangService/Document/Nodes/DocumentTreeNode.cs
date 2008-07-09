using System.Collections.Generic;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class DocumentTreeNode : AbstractTreeNode
    {
        private readonly IDictionary<string, IList<IBooParseTreeNode>> imports = new Dictionary<string, IList<IBooParseTreeNode>>();

        public IDictionary<string, IList<IBooParseTreeNode>> Imports
        {
            get { return imports; }
        }
    }
}
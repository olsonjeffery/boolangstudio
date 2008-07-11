using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class DocumentTreeNode : AbstractTreeNode
    {
        private readonly IDictionary<string, IList<IBooParseTreeNode>> imports = new Dictionary<string, IList<IBooParseTreeNode>>();

        public DocumentTreeNode(IEntity entity) : base(entity)
        {}

        public IDictionary<string, IList<IBooParseTreeNode>> Imports
        {
            get { return imports; }
        }
    }
}
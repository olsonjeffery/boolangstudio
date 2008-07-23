using System.Collections.Generic;
using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class DocumentTreeNode : AbstractTreeNode
    {
        private readonly IDictionary<string, IList<IBooParseTreeNode>> imports = new Dictionary<string, IList<IBooParseTreeNode>>();

        public DocumentTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public IDictionary<string, IList<IBooParseTreeNode>> Imports
        {
            get { return imports; }
        }
    }
}
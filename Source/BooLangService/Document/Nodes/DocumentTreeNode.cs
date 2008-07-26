using System.Collections.Generic;
using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class DocumentTreeNode : AbstractTreeNode
    {
        private readonly IDictionary<string, ImportedNamespaceTreeNode> imports = new Dictionary<string, ImportedNamespaceTreeNode>();

        public DocumentTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public IDictionary<string, ImportedNamespaceTreeNode> Imports
        {
            get { return imports; }
        }
    }
}
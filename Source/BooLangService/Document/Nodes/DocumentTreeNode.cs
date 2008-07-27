using System.Collections.Generic;
using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class DocumentTreeNode : AbstractTreeNode
    {
        private readonly IDictionary<string, ImportedNamespaceTreeNode> imports = new Dictionary<string, ImportedNamespaceTreeNode>();
        private readonly string fileName;

        public DocumentTreeNode(ISourceOrigin sourceOrigin, string fileName) : base(sourceOrigin)
        {
            this.fileName = fileName;
        }

        public override string Name
        {
            get { return fileName; }
        }

        public IDictionary<string, ImportedNamespaceTreeNode> Imports
        {
            get { return imports; }
        }
    }
}
using System.Collections.Generic;
using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public class ProjectTreeNode : AbstractTreeNode
    {
        private readonly IDictionary<string, ReferencedNamespaceTreeNode> referencedNamespaces = new Dictionary<string, ReferencedNamespaceTreeNode>();

        public ProjectTreeNode() : base(new NullSourceOrigin())
        {}

        public IDictionary<string, ReferencedNamespaceTreeNode> ReferencedNamespaces
        {
            get { return referencedNamespaces; }
        }
    }
}
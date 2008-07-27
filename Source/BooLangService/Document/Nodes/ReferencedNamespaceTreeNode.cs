using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    public class ReferencedNamespaceTreeNode : NamespaceTreeNode
    {
        public ReferencedNamespaceTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public override bool IntellisenseVisible
        {
            get { return true; }
        }
    }
}
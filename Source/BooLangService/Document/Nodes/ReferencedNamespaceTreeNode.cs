using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class ReferencedNamespaceTreeNode : NamespaceTreeNode
    {
        public ReferencedNamespaceTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public override string Name
        {
            get { return SourceOrigin.Name; }
            set { base.Name = value; }
        }
    }
}
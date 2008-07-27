using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class NamespaceDeclarationTreeNode : NamespaceTreeNode
    {
        public NamespaceDeclarationTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public override bool IntellisenseVisible
        {
            get { return true; }
        }
    }
}
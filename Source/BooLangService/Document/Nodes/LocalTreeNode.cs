using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public class LocalTreeNode : InstanceDeclarationTreeNode
    {
        public LocalTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public override bool IntellisenseVisible
        {
            get { return true; }
        }
    }
}
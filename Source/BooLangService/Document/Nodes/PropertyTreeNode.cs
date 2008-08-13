using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public class PropertyTreeNode : InstanceDeclarationTreeNode
    {
        public PropertyTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public override bool IntellisenseVisible
        {
            get { return true; }
        }
    }
}
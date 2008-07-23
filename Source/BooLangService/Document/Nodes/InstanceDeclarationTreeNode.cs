using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class InstanceDeclarationTreeNode : AbstractTreeNode, IReturnableNode
    {
        protected InstanceDeclarationTreeNode(ISourceOrigin sourceOrigin)
            : base(sourceOrigin)
        {}

        public string ReturnType { get; set; }

        public override string GetIntellisenseDescription()
        {
            return ReturnType + " " + Name;
        }
    }
}
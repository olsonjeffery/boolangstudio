using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class InstanceDeclarationTreeNode : AbstractTreeNode, IReturnableNode
    {
        protected InstanceDeclarationTreeNode(IEntity entity) : base(entity)
        {}

        public string ReturnType { get; set; }

        public override string GetIntellisenseDescription()
        {
            return ReturnType + " " + Name;
        }
    }
}
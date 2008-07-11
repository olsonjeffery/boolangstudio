using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class TypeDeclarationTreeNode : AbstractTreeNode
    {
        private readonly string fullName;

        protected TypeDeclarationTreeNode(IEntity entity, string fullName) : base(entity)
        {
            this.fullName = fullName;
        }

        public string FullName
        {
            get { return fullName; }
        }
    }
}
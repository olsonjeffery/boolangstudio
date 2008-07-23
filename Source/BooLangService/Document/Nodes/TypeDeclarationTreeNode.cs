using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class TypeDeclarationTreeNode : AbstractTreeNode
    {
        private readonly string fullName;

        protected TypeDeclarationTreeNode(ISourceOrigin sourceOrigin, string fullName)
            : base(sourceOrigin)
        {
            this.fullName = fullName;
        }

        public string FullName
        {
            get { return fullName; }
        }
    }
}
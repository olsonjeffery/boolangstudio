namespace Boo.BooLangService.Document.Nodes
{
    public abstract class TypeDeclarationTreeNode : AbstractTreeNode
    {
        private readonly string fullName;

        protected TypeDeclarationTreeNode(string fullName)
        {
            this.fullName = fullName;
        }

        public string FullName
        {
            get { return fullName; }
        }
    }
}
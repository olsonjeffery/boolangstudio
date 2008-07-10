using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable, IntellisenseVisible]
    public class ClassTreeNode : TypeDeclarationTreeNode
    {
        public ClassTreeNode(string fullName) : base(fullName)
        {}

        public override string GetIntellisenseDescription()
        {
            return "Class " + FullName;
        }
    }
}
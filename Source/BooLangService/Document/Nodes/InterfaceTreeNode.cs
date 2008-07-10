using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable, IntellisenseVisible]
    public class InterfaceTreeNode : TypeDeclarationTreeNode
    {
        public InterfaceTreeNode(string fullName) : base(fullName)
        {}

        public override string GetIntellisenseDescription()
        {
            return "Interface " + FullName;
        }
    }
}
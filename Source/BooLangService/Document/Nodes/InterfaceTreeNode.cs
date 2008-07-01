using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class InterfaceTreeNode : AbstractTreeNode
    {
        public string FullName { get; set; }

        public override string GetIntellisenseDescription()
        {
            return "Interface " + FullName;
        }
    }
}
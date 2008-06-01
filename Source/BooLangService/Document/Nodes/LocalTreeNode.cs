using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class LocalTreeNode : AbstractTreeNode, IReturnableNode
    {
        public string ReturnType { get; set; }
    }
}
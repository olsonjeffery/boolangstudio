using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable, IntellisenseVisible]
    public class ClassTreeNode : AbstractTreeNode
    {
        public string FullName { get; set; }
    }
}
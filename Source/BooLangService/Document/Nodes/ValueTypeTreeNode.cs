using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable, IntellisenseVisible]
    public class ValueTypeTreeNode : TypeDeclarationTreeNode
    {
        public ValueTypeTreeNode(ISourceOrigin sourceOrigin, string fullName) : base(sourceOrigin, fullName)
        {}
    }
}
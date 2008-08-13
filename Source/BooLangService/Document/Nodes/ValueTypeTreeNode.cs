using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class ValueTypeTreeNode : TypeDeclarationTreeNode
    {
        public ValueTypeTreeNode(ISourceOrigin sourceOrigin, string fullName) : base(sourceOrigin, fullName)
        {}

        public override bool IntellisenseVisible
        {
            get { return true; }
        }
    }
}
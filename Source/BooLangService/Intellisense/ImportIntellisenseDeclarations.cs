using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Intellisense
{
    public class ImportIntellisenseDeclarations : IntellisenseDeclarations
    {
        public override void Add(IBooParseTreeNode member)
        {
            // allow only other namespaces to be displayed
            if (member is NamespaceTreeNode)
                base.Add(member);
        }
    }
}
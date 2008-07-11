using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class ImportedNamespaceTreeNode : AbstractTreeNode
    {
        public ImportedNamespaceTreeNode() : base(null)
        {}
    }
}
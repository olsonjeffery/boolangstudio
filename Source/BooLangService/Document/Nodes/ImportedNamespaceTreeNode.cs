using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class ImportedNamespaceTreeNode : AbstractTreeNode
    {
        public ImportedNamespaceTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}

        public override string Name
        {
            get { return SourceOrigin.Name; }
            set { base.Name = value; }
        }
    }
}
using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [IntellisenseVisible]
    public class FieldTreeNode : InstanceDeclarationTreeNode
    {
        public FieldTreeNode(ISourceOrigin sourceOrigin) : base(sourceOrigin)
        {}
    }
}
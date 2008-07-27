using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class InterfaceTreeNode : TypeDeclarationTreeNode
    {
        public InterfaceTreeNode(ISourceOrigin sourceOrigin, string fullName) : base(sourceOrigin, fullName)
        {}

        public override string GetIntellisenseDescription()
        {
            return "Interface " + FullName;
        }

        public override bool IntellisenseVisible
        {
            get { return true; }
        }
    }
}
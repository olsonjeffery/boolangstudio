using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable, IntellisenseVisible]
    public class InterfaceTreeNode : TypeDeclarationTreeNode
    {
        public InterfaceTreeNode(IEntity entity, string fullName) : base(entity, fullName)
        {}

        public override string GetIntellisenseDescription()
        {
            return "Interface " + FullName;
        }
    }
}
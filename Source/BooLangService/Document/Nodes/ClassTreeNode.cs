using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable, IntellisenseVisible]
    public class ClassTreeNode : TypeDeclarationTreeNode
    {
        public ClassTreeNode(IEntity entity, string fullName) : base(entity, fullName)
        {}

        public override string GetIntellisenseDescription()
        {
            return "Class " + FullName;
        }
    }
}
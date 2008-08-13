using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class ClassTreeNode : TypeDeclarationTreeNode
    {
        public ClassTreeNode(ISourceOrigin sourceOrigin, string fullName) : base(sourceOrigin, fullName)
        {}

        public override string GetIntellisenseDescription()
        {
            return "Class " + FullName;
        }

        public override bool IntellisenseVisible
        {
            get { return true;}
        }
    }
}
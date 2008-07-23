using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public class ProjectTreeNode : AbstractTreeNode
    {
        public ProjectTreeNode() : base(new NullSourceOrigin())
        {}
    }
}
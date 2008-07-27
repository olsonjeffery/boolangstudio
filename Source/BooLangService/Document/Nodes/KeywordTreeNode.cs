using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public class KeywordTreeNode : AbstractTreeNode
    {
        private readonly string name;

        public KeywordTreeNode(string name) : base(new NullSourceOrigin())
        {
            this.name = name;
        }

        public override string Name
        {
            get { return name; }
        }

        public override bool IntellisenseVisible
        {
            get { return true; }
        }
    }
}
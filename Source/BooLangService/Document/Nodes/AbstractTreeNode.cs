using System.Collections.Generic;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class AbstractTreeNode : IBooParseTreeNode
    {
        private IBooParseTreeNode parent;
        private readonly IList<IBooParseTreeNode> children;
        private string name;
        private int startLine;
        private int endLine;

        public AbstractTreeNode()
        {
            children = new ParseTreeNodeSet(this);
        }

        public IBooParseTreeNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public IList<IBooParseTreeNode> Children
        {
            get { return children; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int StartLine
        {
            get { return startLine; }
            set { startLine = value; }
        }

        public int EndLine
        {
            get { return endLine; }
            set { endLine = value; }
        }
    }
}
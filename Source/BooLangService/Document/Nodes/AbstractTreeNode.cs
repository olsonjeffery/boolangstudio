using System.Collections.Generic;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class AbstractTreeNode : IBooParseTreeNode
    {
        private IBooParseTreeNode parent;
        private readonly IList<IBooParseTreeNode> children;
        private string name;
        private int startLine;
        private int endLine = -1; // forces generated endline if not set

        public AbstractTreeNode()
        {
            children = new ParseTreeNodeSet(this);
        }

        public bool ContainsLine(int line)
        {
            return StartLine <= line && EndLine >= line;
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
            get { return endLine > -1 ? endLine : GetHighestChildEndLine(); }
            set { endLine = value; }
        }

        private int GetHighestChildEndLine()
        {
            int end = 0;

            foreach (IBooParseTreeNode child in Children)
            {
                if (child.EndLine > end)
                    end = child.EndLine;
            }

            return end;
        }
    }
}
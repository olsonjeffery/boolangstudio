using System.Collections.Generic;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class AbstractTreeNode : IBooParseTreeNode
    {
        private readonly IList<IBooParseTreeNode> children;
        private int endLine = -1; // forces generated endline if not set

        public AbstractTreeNode()
        {
            children = new ParseTreeNodeSet(this);
        }

        public bool ContainsLine(int line)
        {
            return StartLine <= line && EndLine >= line;
        }

        public virtual string GetIntellisenseDescription()
        {
            return Name;
        }

        public IList<IBooParseTreeNode> Children
        {
            get { return children; }
        }

        public IBooParseTreeNode Parent { get; set; }
        public string Name { get; set; }
        public int StartLine { get; set; }

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
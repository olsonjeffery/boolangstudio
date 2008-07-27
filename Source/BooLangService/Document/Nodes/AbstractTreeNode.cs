using System.Collections.Generic;
using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    public abstract class AbstractTreeNode : IBooParseTreeNode
    {
        private readonly ISourceOrigin sourceOrigin;
        private readonly IList<IBooParseTreeNode> children;
        private int endLine = -1; // forces generated endline if not set
        private readonly string name;

        protected AbstractTreeNode(ISourceOrigin sourceOrigin)
        {
            this.sourceOrigin = sourceOrigin;
            this.name = sourceOrigin.Name;
            children = new ParseTreeNodeSet(this);
        }

        public virtual bool IntellisenseVisible
        {
            get { return false; }
        }

        public virtual string Name
        {
            get { return name; }
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
        public int StartLine { get; set; }

        public int EndLine
        {
            get { return endLine > -1 ? endLine : GetHighestChildEndLine(); }
            set { endLine = value; }
        }

        public ISourceOrigin SourceOrigin
        {
            get { return sourceOrigin; }
        }

        private int GetHighestChildEndLine()
        {
            int end = 0;

            foreach (var child in Children)
            {
                if (child.EndLine > end)
                    end = child.EndLine;
            }

            return end;
        }
    }
}
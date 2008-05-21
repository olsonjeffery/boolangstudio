using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boo.BooLangService;
using Microsoft.VisualStudio.Package;

namespace BooLangService
{
    public class BooScope : AuthoringScope
    {
        private readonly IBooParseTreeNode compiledDocument;

        public BooScope(IBooParseTreeNode compiledDocument)
        {
            this.compiledDocument = compiledDocument;
        }

        public override string GetDataTipText(int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            throw new NotImplementedException();
        }

        private IBooParseTreeNode GetContainingNode(IBooParseTreeNode node, int line)
        {
            foreach (IBooParseTreeNode child in node.Children)
            {
                IBooParseTreeNode foundNode = GetContainingNode(child, line);

                if (foundNode != null)
                    return foundNode;
            }

            if (node.StartLine <= line && node.EndLine >= line)
                return node;

            return null;
        }

        public override Declarations GetDeclarations(Microsoft.VisualStudio.TextManager.Interop.IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
        {
            // returns the declarations pre-prepared by the BooLangService.ParseSource method
            // I really think the parsing should be done in ParseSource, but the searching should
            // be done in here.
            IBooParseTreeNode scope = GetContainingNode(compiledDocument, line);

            List<IBooParseTreeNode> displayableInScope = new List<IBooParseTreeNode>();

            displayableInScope.AddRange(FlattenDown(scope.Children));
            displayableInScope.AddRange(FlattenUp(scope));

            displayableInScope.Sort(delegate(IBooParseTreeNode x, IBooParseTreeNode y) {
                return x.Name.CompareTo(y.Name);
            });

            return new BooDeclarations(displayableInScope);
        }

        private IList<IBooParseTreeNode> FlattenUp(IBooParseTreeNode node)
        {
            List<IBooParseTreeNode> flattened = new List<IBooParseTreeNode>();
            IBooParseTreeNode parent = node;

            while ((parent = parent.Parent) != null)
            {
                foreach (IBooParseTreeNode sibling in parent.Children)
                {
                    flattened.Add(sibling);
                }

                if (!(parent is RootTreeNode))
                    flattened.Add(parent); // don't add the root node, because it's only there as a container
            }

            return flattened;
        }

        private IList<IBooParseTreeNode> FlattenDown(IList<IBooParseTreeNode> nodes)
        {
            List<IBooParseTreeNode> flattened = new List<IBooParseTreeNode>();

            foreach (IBooParseTreeNode node in nodes)
            {
                flattened.Add(node);
                flattened.AddRange(FlattenDown(node.Children));
            }

            return flattened;
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            throw new NotImplementedException();
        }

        public override string Goto(Microsoft.VisualStudio.VSConstants.VSStd97CmdID cmd, Microsoft.VisualStudio.TextManager.Interop.IVsTextView textView, int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            throw new NotImplementedException();
        }
    }
}

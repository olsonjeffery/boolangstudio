using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Document
{
    public class CompiledDocument
    {
        private readonly IBooParseTreeNode root;

        public CompiledDocument(IBooParseTreeNode root)
        {
            this.root = root;
        }

        public IBooParseTreeNode GetScopeByLine(int line)
        {
            return GetScopeByLine(root, line);
        }

        private IBooParseTreeNode GetScopeByLine(IBooParseTreeNode node, int line)
        {
            foreach (IBooParseTreeNode child in node.Children)
            {
                IBooParseTreeNode foundNode = GetScopeByLine(child, line);

                if (foundNode != null)
                    return foundNode;
            }

            if (node.StartLine <= line && node.EndLine > line)
                return node;

            return null;
        }
    }
}
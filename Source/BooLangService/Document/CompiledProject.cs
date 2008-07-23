using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document
{
    public class CompiledProject
    {
        private readonly IBooParseTreeNode root;

        public CompiledProject(IBooParseTreeNode root, List<Assembly> references)
        {
            this.root = root;
        }

        public IBooParseTreeNode ParseTree
        {
            get { return root; }
        }

        public IBooParseTreeNode GetScope(string fileName, int line)
        {
            foreach (var document in ((ProjectTreeNode)root).Children)
            {
                if (document.Name == fileName)
                    return GetScope(document, line);
            }

            return null;
        }

        private IBooParseTreeNode GetScope(IBooParseTreeNode node, int line)
        {
            foreach (IBooParseTreeNode child in node.Children)
            {
                IBooParseTreeNode foundNode = GetScope(child, line);

                if (foundNode != null) 
                    return foundNode;
            }

            bool isScopable = AttributeHelper.Has<ScopableAttribute>(node.GetType());

            if (isScopable && node.ContainsLine(line))
                return node;

            return null;
        }
    }
}
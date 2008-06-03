using System;
using System.Collections;
using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Document
{
    public class CompiledDocument
    {
        private readonly IBooParseTreeNode root;
        private readonly string content;
        private readonly IDictionary<string, IList<IBooParseTreeNode>> imports;

        public CompiledDocument(IBooParseTreeNode root, IDictionary<string, IList<IBooParseTreeNode>> importedNamespaces, string content)
        {
            this.root = root;
            this.content = content;
            this.imports = importedNamespaces;
        }

        public string Content
        {
            get { return content; }
        }

        public IDictionary<string, IList<IBooParseTreeNode>> Imports
        {
            get { return imports; }
        }

        public IBooParseTreeNode ParseTree
        {
            get { return root; }
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

            bool isScopable = AttributeHelper.Has<ScopableAttribute>(node.GetType());

            if (isScopable && node.ContainsLine(line))
                return node;

            return null;
        }
    }
}
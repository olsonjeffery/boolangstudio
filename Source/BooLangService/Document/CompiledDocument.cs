using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document
{
    public class CompiledDocument
    {
        private readonly IBooParseTreeNode root;
        private readonly string content;
        private readonly IDictionary<string, IList<IBooParseTreeNode>> imports;
        private readonly IList<ReferencePoint> referencePoints;

        public CompiledDocument(IBooParseTreeNode root, IDictionary<string, IList<IBooParseTreeNode>> importedNamespaces, IList<ReferencePoint> referencePoints, string content)
        {
            this.root = root;
            this.content = content;
            this.imports = importedNamespaces;
            this.referencePoints = referencePoints;
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

        /// <summary>
        /// Gets an entity, if one exists, at a specified line and column.
        /// </summary>
        /// <param name="line">Line</param>
        /// <param name="column">Column</param>
        /// <returns>Entity </returns>
        public IEntity GetEntityAt(int line, int column)
        {
            foreach (var referencePoint in referencePoints)
            {
                if (referencePoint.WithinBounds(line, column))
                    return referencePoint.Entity;
            }

            return null;
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
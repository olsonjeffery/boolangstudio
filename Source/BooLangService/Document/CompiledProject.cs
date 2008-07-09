using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document
{
    public class CompiledProject
    {
        private readonly IBooParseTreeNode root;
        private readonly IList<ReferencePoint> referencePoints;

        public CompiledProject(IBooParseTreeNode root, IList<ReferencePoint> referencePoints)
        {
            this.root = root;
            this.referencePoints = referencePoints;
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
        public IEntity GetEntityAt(string fileName, int line, int column)
        {
            foreach (var referencePoint in referencePoints)
            {
                if (referencePoint.FileName == fileName && referencePoint.WithinBounds(line, column))
                    return referencePoint.Entity;
            }

            return null;
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
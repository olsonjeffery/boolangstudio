using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document.Origins
{
    /// <summary>
    /// Represents a namespace located in a referenced assembly.
    /// </summary>
    internal class AssemblyNamespaceSourceOrigin : ISourceOrigin
    {
        private readonly NamespaceLeaf namespaceLeaf;

        public AssemblyNamespaceSourceOrigin(NamespaceLeaf namespaceLeaf)
        {
            this.namespaceLeaf = namespaceLeaf;
        }

        public string Name
        {
            get { return namespaceLeaf.Name; }
        }

        public List<ISourceOrigin> GetMembers(bool isConstructor)
        {
            var members = new List<ISourceOrigin>();

            namespaceLeaf.Children.ForEach(c => members.Add(new AssemblyNamespaceSourceOrigin(c)));

            return members;
        }

        public IBooParseTreeNode ToTreeNode()
        {
            return new ImportedNamespaceTreeNode(this);
        }
    }
}
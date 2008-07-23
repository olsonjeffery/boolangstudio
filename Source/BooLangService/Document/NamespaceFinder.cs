using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangProject;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;
using BooLangService;
using EnvDTE;
using VSLangProj;

namespace Boo.BooLangService.Document
{
    public class NamespaceFinder
    {
        /// <summary>
        /// Gets namespaces from references that match a search query.
        /// </summary>
        /// <param name="references">Project references to include in the search</param>
        /// <param name="searchQuery">Start of the namespace to resolve from. If blank gets all namespaces.</param>
        /// <returns>List of available namespaces</returns>
        public BooParseTreeNodeList QueryNamespacesFromReferences(List<IReference> references, string searchQuery)
        {
            var namespaces = new BooParseTreeNodeList();

            foreach (var reference in references)
            {
                namespaces.AddRange(QueryNamespacesFromAssembly(reference.GetAssembly(), searchQuery));
            }

            return namespaces;
        }

        private IList<IBooParseTreeNode> QueryNamespacesFromAssembly(Assembly assembly, string searchQuery)
        {
            var namespaces = new List<IBooParseTreeNode>();

            if (assembly == null) return namespaces; // just return empty list

            var exposedNamespaces = new List<string>();

            foreach (var type in assembly.GetExportedTypes())
            {
                if (!exposedNamespaces.Contains(type.Namespace))
                    exposedNamespaces.Add(type.Namespace);
            }

            var treeBuilder = new NamespaceTreeBuilder();
            var namespaceTree = treeBuilder.Build(exposedNamespaces.ToArray());

            foreach (var ns in namespaceTree)
            {
                namespaces.Add(new ImportedNamespaceTreeNode(new AssemblyNamespaceSourceOrigin(ns)));
            }

            return namespaces;
        }
    }
}
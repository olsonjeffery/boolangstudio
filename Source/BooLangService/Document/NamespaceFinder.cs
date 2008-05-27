using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.VSInterop;
using BooLangService;
using Microsoft.VisualStudio.Package;

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
        public BooParseTreeNodeList QueryNamespacesFromReferences(IList<ProjectReference> references, string searchQuery)
        {
            BooParseTreeNodeList namespaces = new BooParseTreeNodeList();

            foreach (ProjectReference reference in references)
            {
                if (reference.IsAssembly)
                {
                    Assembly asm = Assembly.LoadFrom(reference.Target);

                    foreach (Type type in asm.GetExportedTypes())
                    {
                        string ns = type.Namespace;

                        if (!ns.StartsWith(searchQuery)) continue;

                        ns = ns.Remove(0, searchQuery.Length);
                        ns = ns.Contains(".") ? ns.Substring(0, ns.IndexOf(".")) : ns;

                        IBooParseTreeNode treeNode = new ImportedNamespaceTreeNode();

                        treeNode.Name = ns;

                        namespaces.Add(treeNode);
                    }
                }
                else
                {
                    // project reference - not supported yet
                }
            }

            return namespaces;
        }
    }
}
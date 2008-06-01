using System;
using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.VSInterop;
using BooLangService;
using EnvDTE;
using Microsoft.VisualStudio.Package;
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
        public BooParseTreeNodeList QueryNamespacesFromReferences(References references, string searchQuery)
        {
            var namespaces = new BooParseTreeNodeList();

            foreach (Reference reference in references)
            {
                if (reference.SourceProject != null)
                    namespaces.AddRange(QueryNamespacesFromProjectReference(reference.SourceProject));
            }

            return namespaces;
        }

        private IList<IBooParseTreeNode> QueryNamespacesFromProjectReference(Project project)
        {
            var namespaces = new List<IBooParseTreeNode>();

            foreach (CodeElement element in project.CodeModel.CodeElements)
            {
                var codeNamespace = element as CodeNamespace;

                if (codeNamespace != null)
                {
                    bool isInternal = false;

                    foreach (CodeElement member in codeNamespace.Members)
                    {
                        if (member.InfoLocation == vsCMInfoLocation.vsCMInfoLocationProject)
                            isInternal = true;
                    }

                    if (isInternal)
                        namespaces.Add(new ImportedNamespaceTreeNode {Name = codeNamespace.FullName});
                }
            }

            return namespaces;
        }
    }
}
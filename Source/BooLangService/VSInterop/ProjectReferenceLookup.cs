using System.Collections.Generic;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.VSInterop;
using Microsoft.VisualStudio.Package;
using VSLangProj;

namespace Boo.BooLangService.VSInterop
{
    public class ProjectReferenceLookup : IProjectReferenceLookup
    {
        private readonly LanguageService service;

        public ProjectReferenceLookup(LanguageService service)
        {
            this.service = service;
        }

        public IList<IBooParseTreeNode> GetReferencedNamespacesInProjectContaining(string fileName, string namespaceContinuation)
        {
            var availableNamespaces = new NamespaceFinder();
            var projects = new ProjectHierarchy(service);
            VSProject project = projects.GetContainingProject(fileName);

            return availableNamespaces.QueryNamespacesFromReferences(project.References, namespaceContinuation);
        }
    }
}
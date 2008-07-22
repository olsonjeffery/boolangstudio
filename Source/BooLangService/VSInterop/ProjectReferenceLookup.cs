using System.Collections.Generic;
using Boo.BooLangProject;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.VSInterop;

namespace Boo.BooLangService.VSInterop
{
    public class ProjectReferenceLookup : IProjectReferenceLookup
    {
        public IList<IBooParseTreeNode> GetReferencedNamespacesInProjectContaining(string fileName, string namespaceContinuation)
        {
            var availableNamespaces = new NamespaceFinder();
            var project = BooProjectSources.Find(fileName);

            return availableNamespaces.QueryNamespacesFromReferences(project.References, namespaceContinuation);
        }
    }
}
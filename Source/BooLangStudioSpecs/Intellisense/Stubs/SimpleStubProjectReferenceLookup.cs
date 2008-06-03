using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.VSInterop;

namespace Boo.BooLangStudioSpecs.Intellisense.Stubs
{
    internal class SimpleStubProjectReferenceLookup : IProjectReferenceLookup
    {
        private readonly IList<IBooParseTreeNode> namespaces = new List<IBooParseTreeNode>();

        public IList<IBooParseTreeNode> GetReferencedsNamespacesInProjectContaining(string fileName, string namespaceContinuation)
        {
            return namespaces;
        }

        public void AddFakeNamespaces(string[] fakeNamespaces)
        {
            foreach (var ns in fakeNamespaces)
            {
                namespaces.Add(new ImportedNamespaceTreeNode { Name = ns });
            }
        }
    }
}
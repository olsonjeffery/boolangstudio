using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangProject;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.VSInterop;

namespace Boo.BooLangStudioSpecs.Intellisense.Stubs
{
    internal class SimpleStubProjectReferenceLookup : IProjectReferenceLookup
    {
        private readonly List<IBooParseTreeNode> namespaces = new List<IBooParseTreeNode>();

        public IList<IBooParseTreeNode> GetReferencedNamespacesInProjectContaining(string fileName, string namespaceContinuation)
        {
            return namespaces;
        }

        public void AddAssembliesForReferences(Assembly[] assemblies)
        {
            var namespaceFinder = new NamespaceFinder();
            var references = GetReferencesFromAssemblies(assemblies);

            namespaces.AddRange(namespaceFinder.QueryNamespacesFromReferences(references, ""));
        }

        private List<IReference> GetReferencesFromAssemblies(Assembly[] assemblies)
        {
            var references = new List<IReference>();

            foreach (var assembly in assemblies)
            {
                references.Add(new AssemblyReferenceFake(assembly));
            }

            return references;
        }
    }

    internal class AssemblyReferenceFake : IReference
    {
        private readonly Assembly assembly;

        public AssemblyReferenceFake(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public Assembly GetAssembly()
        {
            return assembly;
        }

        public string Path { get; set; }
    }
}
using System.Collections.Generic;
using System.Reflection;
using Boo.BooLangProject;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangStudioSpecs.Intellisense.Stubs
{
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
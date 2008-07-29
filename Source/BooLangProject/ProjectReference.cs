using System.Reflection;
using BooLangService;

namespace Boo.BooLangProject
{
    internal class ProjectReference : IReference
    {
        public Assembly GetAssembly()
        {
            return AssemblyHelper.FindInCurrentAppDomainOrLoad(Path);
        }

        public string Path { get; set; }
    }
}
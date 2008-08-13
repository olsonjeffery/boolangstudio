using System.Reflection;

namespace Boo.BooLangProject
{
    public interface IReference
    {
        Assembly GetAssembly();
        string Path { get; set; }
    }
}
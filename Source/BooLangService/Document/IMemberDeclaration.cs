using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document
{
    public interface IMemberDeclaration
    {
        string Name { get; }
        string FullName { get; }
        string Description { get; }
        IntellisenseIcon Icon { get; }
    }
}
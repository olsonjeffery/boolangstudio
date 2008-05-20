namespace Boo.BooLangService.Members
{
    public interface IMemberDeclaration
    {
        string Name { get; }
        string FullName { get; }
        string Description { get; }
        IntellisenseIcon Icon { get; }
    }
}
using System;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Document
{
    [Obsolete("Being phased out in favor of the tree parser.")]
    public interface IMemberDeclaration
    {
        string Name { get; }
        string FullName { get; }
        string Description { get; }
        IntellisenseIcon Icon { get; }
    }
}
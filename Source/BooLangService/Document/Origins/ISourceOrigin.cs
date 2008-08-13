using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Document.Origins
{
    public interface ISourceOrigin
    {
        string Name { get; }
        List<ISourceOrigin> GetMembers(bool isConstructor);
        IBooParseTreeNode ToTreeNode();
    }
}
using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.VSInterop
{
    public interface IProjectReferenceLookup
    {
        IList<IBooParseTreeNode> GetReferencedNamespacesInProjectContaining(string fileName, string namespaceContinuation);
    }
}
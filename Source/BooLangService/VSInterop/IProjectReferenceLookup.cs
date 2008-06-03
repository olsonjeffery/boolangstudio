using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.VSInterop
{
    public interface IProjectReferenceLookup
    {
        IList<IBooParseTreeNode> GetReferencedsNamespacesInProjectContaining(string fileName, string namespaceContinuation);
    }
}
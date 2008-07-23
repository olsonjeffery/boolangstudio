using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Document.Origins
{
    /// <summary>
    /// Used for tree nodes that don't have an origin in the source, the project is one example, keywords another.
    /// See: Null Object pattern.
    /// </summary>
    public class NullSourceOrigin : ISourceOrigin
    {
        public string Name
        {
            get { return ""; }
        }
        public List<ISourceOrigin> GetMembers(bool isConstructor)
        {
            return null;
        }

        public IBooParseTreeNode ToTreeNode()
        {
            return null;
        }
    }
}
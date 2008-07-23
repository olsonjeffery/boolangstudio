using System.Collections.Generic;

namespace Boo.BooLangService.Intellisense
{
    /// <summary>
    /// Builds a hierarchy from a collection of fully-qualifed namespaces.
    /// </summary>
    /// <example>
    /// Given:
    ///     Boo.Lang.Runtime
    /// 
    /// Result:
    ///     Boo
    ///     |- Lang
    ///           |- Runtime
    /// 
    /// 
    /// Given:
    ///     Boo.Lang.Parser
    ///     Boo.Lang.Runtime
    /// 
    /// Result:
    ///     Boo
    ///     |- Lang
    ///     |     |- Parser
    ///     |
    ///     |- Runtime
    /// </example>
    public class NamespaceTreeBuilder
    {
        public List<NamespaceLeaf> Build(params string[] namespaces)
        {
            var tree = new List<NamespaceLeaf>();

            foreach (var ns in namespaces)
            {
                if (ns == null) continue;

                BuildIndividualTree(ns, tree);
            }

            return tree;
        }

        private void BuildIndividualTree(string fullNamespace, List<NamespaceLeaf> parentBranch)
        {
            var indexOfFirstDot = fullNamespace.IndexOf('.');

            var firstPart = indexOfFirstDot > 0 ? fullNamespace.Substring(0, indexOfFirstDot) : fullNamespace;
            var rest = indexOfFirstDot > 0 ? fullNamespace.Substring(indexOfFirstDot + 1) : null;
            var leaf = parentBranch.Find(l => l.Name == firstPart);

            if (leaf == null)
            {
                leaf = new NamespaceLeaf(firstPart);
                parentBranch.Add(leaf);
            }

            if (rest != null)
                BuildIndividualTree(rest, leaf.Children);
        }
    }
}
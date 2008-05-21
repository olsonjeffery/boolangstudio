using System;
using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Intellisense
{
    public class IntellisenseIconResolver
    {
        private readonly IDictionary<Type, IntellisenseIcon> treeNodeIconMap = new Dictionary<Type, IntellisenseIcon>();

        public IntellisenseIconResolver()
        {
            treeNodeIconMap.Add(typeof(MethodTreeNode), IntellisenseIcon.Method);
            treeNodeIconMap.Add(typeof(LocalTreeNode), IntellisenseIcon.Variable);
        }

        public IntellisenseIcon Resolve(IBooParseTreeNode node)
        {
            Type nodeType = node.GetType();

            if (treeNodeIconMap.ContainsKey(nodeType))
                return treeNodeIconMap[nodeType];

            return 0;
        }
    }
}
using System;
using System.Collections.Generic;
using Boo.BooLangService.Members;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangService
{
    /// <summary>
    /// Contains a list of declarations to be contained within the intellisense list.
    /// </summary>
    /// <remarks>
    /// I'm not sure BooDeclarations is the best name for this, but because I can't just
    /// call it "Declarations", I'll stick with it.
    /// </remarks>
    public class BooDeclarations : Declarations
    {
        private readonly IntellisenseIconResolver icons = new IntellisenseIconResolver();
        private readonly IList<IBooParseTreeNode> members = new List<IBooParseTreeNode>();

        public BooDeclarations(IList<IBooParseTreeNode> members)
        {
            this.members = members;
        }

        public override int GetCount()
        {
            return members.Count;
        }

        public override string GetDescription(int index)
        {
            return GetDisplayText(index);
        }

        public override string GetDisplayText(int index)
        {
            return members[index].Name;
        }

        public override int GetGlyph(int index)
        {
            IBooParseTreeNode node = members[index];
            
            return (int)icons.Resolve(node);
        }

        public override string GetName(int index)
        {
            return GetDisplayText(index);
        }
    }

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
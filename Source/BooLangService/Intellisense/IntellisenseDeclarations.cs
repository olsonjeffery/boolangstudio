using System;
using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.TypeSystem;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangService
{
    /// <summary>
    /// Contains a list of declarations to be contained within the intellisense list.
    /// </summary>
    public class IntellisenseDeclarations : Declarations
    {
        private readonly IntellisenseIconResolver icons = new IntellisenseIconResolver();
        private readonly BooParseTreeNodeList members = new BooParseTreeNodeList();

        public override int GetCount()
        {
            return members.Count;
        }

        public override string GetDescription(int index)
        {
            IBooParseTreeNode node = members[index];

            return node.GetIntellisenseDescription();
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

        public void Add(IList<IBooParseTreeNode> list)
        {
            foreach (var node in list)
            {
                Add(node);
            }

            members.Sort();
        }

        public void Add(string[] keywords)
        {
            // still a bit hacky
            foreach (var keyword in keywords)
            {
                Add(new KeywordTreeNode { Name = keyword });
            }

            members.Sort();
        }

        public void Add(IBooParseTreeNode member)
        {
            members.Add(member);
        }

        public void Sort()
        {
            members.Sort();
        }
    }
}
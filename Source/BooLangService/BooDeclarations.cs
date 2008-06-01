using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;
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
        private readonly BooParseTreeNodeList members = new BooParseTreeNodeList();

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

        private void Add(IBooParseTreeNode node)
        {
            members.Add(node);
        }
    }
}
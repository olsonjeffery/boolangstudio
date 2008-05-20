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
        private readonly IList<IMemberDeclaration> members = new List<IMemberDeclaration>();

        public BooDeclarations(IList<IMemberDeclaration> members)
        {
            this.members = members;
        }

        public override int GetCount()
        {
            return members.Count;
        }

        public override string GetDescription(int index)
        {
            return members[index].Description;
        }

        public override string GetDisplayText(int index)
        {
            return members[index].Name;
        }

        public override int GetGlyph(int index)
        {
            return (int)members[index].Icon;
        }

        public override string GetName(int index)
        {
            return GetDisplayText(index);
        }
    }
}
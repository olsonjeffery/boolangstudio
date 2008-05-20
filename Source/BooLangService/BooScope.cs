using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boo.BooLangService;
using Microsoft.VisualStudio.Package;

namespace BooLangService
{
    public class BooScope : AuthoringScope
    {
        private readonly BooDeclarations declarations;

        public BooScope(BooDeclarations declarations)
        {
            this.declarations = declarations;
        }

        public override string GetDataTipText(int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            throw new NotImplementedException();
        }

        public override Declarations GetDeclarations(Microsoft.VisualStudio.TextManager.Interop.IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
        {
            // returns the declarations pre-prepared by the BooLangService.ParseSource method
            // I really think the parsing should be done in ParseSource, but the searching should
            // be done in here.
            return declarations;
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            throw new NotImplementedException();
        }

        public override string Goto(Microsoft.VisualStudio.VSConstants.VSStd97CmdID cmd, Microsoft.VisualStudio.TextManager.Interop.IVsTextView textView, int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;

namespace BooLangService
{
    class BooScope : AuthoringScope
    {
        public override string GetDataTipText(int line, int col, out Microsoft.VisualStudio.TextManager.Interop.TextSpan span)
        {
            throw new NotImplementedException();
        }

        public override Declarations GetDeclarations(Microsoft.VisualStudio.TextManager.Interop.IVsTextView view, int line, int col, TokenInfo info, ParseReason reason)
        {
            throw new NotImplementedException();
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

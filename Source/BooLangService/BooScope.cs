using System;
using Boo.BooLangService.Document;
using Boo.BooLangService.Intellisense;
using Boo.BooLangService.VSInterop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace BooLangService
{
    public class BooScope : AuthoringScope
    {
        private readonly DeclarationFinder declarations;

        public BooScope(LanguageService service, CompiledProject compiledProject, BooSource source, string fileName)
        {
            declarations = new DeclarationFinder(compiledProject, new ProjectReferenceLookup(), source, fileName);
        }

        public override string GetDataTipText(int line, int col, out TextSpan span)
        {
            throw new NotImplementedException();
        }

        public override Declarations GetDeclarations(IVsTextView view, int lineNum, int col, TokenInfo info, ParseReason reason)
        {
            return declarations.Find(lineNum, col, reason);
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            throw new NotImplementedException();
        }

        public override string Goto(VSConstants.VSStd97CmdID cmd, IVsTextView textView, int line, int col, out TextSpan span)
        {
            throw new NotImplementedException();
        }
    }
}

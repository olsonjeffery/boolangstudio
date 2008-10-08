using System;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Intellisense;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace BooLangService
{
    public class BooScope : AuthoringScope
    {
        private readonly DeclarationFinder declarations;
        private readonly Methods methods;
        private readonly ParseRequestProcessor parseRequestProcessor;
        private readonly BooSource source;
        private readonly ParseRequest parseRequest;

        public BooScope(CompiledProject compiledProject, BooSource source, ParseRequest parseRequest, Methods methods, ParseRequestProcessor parseRequestProcessor)
            : this(compiledProject, source, parseRequest, parseRequestProcessor)
        {
            this.methods = methods;
        }

        public BooScope(CompiledProject compiledProject, BooSource source, ParseRequest parseRequest, ParseRequestProcessor parseRequestProcessor)
        {
            this.source = source;
            declarations = new DeclarationFinder(compiledProject, source, parseRequest.FileName);
            this.parseRequest = parseRequest;
            this.parseRequestProcessor = parseRequestProcessor;
        }

        public override string GetDataTipText(int line, int col, out TextSpan span)
        {
            span = new TextSpan();

            var token = source.GetTokenInfo(line, col);

            span.iStartLine = line;
            span.iEndLine = line;
            span.iStartIndex = token.StartIndex;
            span.iEndIndex = token.EndIndex + 1;

            return parseRequestProcessor.GetQuickInfo(line, col, parseRequest);
        }

        public override Declarations GetDeclarations(IVsTextView view, int lineNum, int col, TokenInfo info, ParseReason reason)
        {
            return declarations.Find(lineNum, col, reason);
        }

        public override Methods GetMethods(int line, int col, string name)
        {
            return methods;
        }

        public override string Goto(VSConstants.VSStd97CmdID cmd, IVsTextView textView, int line, int col, out TextSpan span)
        {
            throw new NotImplementedException();
        }
    }
}

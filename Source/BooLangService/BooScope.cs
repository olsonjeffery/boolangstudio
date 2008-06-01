using System;
using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;
using Boo.BooLangService.VSInterop;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using VSLangProj;

namespace BooLangService
{
    public class BooScope : AuthoringScope
    {
        private readonly CompiledDocument compiledDocument;
        private readonly Source source;
        private readonly LanguageService service;
        private readonly string fileName;
        private const string ImportKeyword = "import";

        public BooScope(LanguageService service, CompiledDocument compiledDocument, Source source, string fileName)
        {
            this.service = service;
            this.compiledDocument = compiledDocument;
            this.source = source;
            this.fileName = fileName;
        }

        public override string GetDataTipText(int line, int col, out TextSpan span)
        {
            throw new NotImplementedException();
        }

        public override Declarations GetDeclarations(IVsTextView view, int lineNum, int col, TokenInfo info, ParseReason reason)
        {
            string line = source.GetLine(lineNum);

            if (line.StartsWith(ImportKeyword))
            {
                // handle this separately from normal intellisense, because:
                //  a) the open import statement will have broken the document
                //  b) we don't need the doc anyway, all imports would be external to the current file
                // only problem is, the top level namespaces (i.e. System, Boo, Microsoft) should be
                // usable from within code too, so we need some way of parsing and caching them and
                // making them available everywhere
                return GetImportIntellisenseDeclarations(line);
            }
            
            return GetScopedIntellisenseDeclarations(lineNum);
        }

        private Declarations GetImportIntellisenseDeclarations(string line)
        {
            NamespaceFinder availableNamespaces = new NamespaceFinder();

            // get any namespace already written (i.e. "Boo.Lang.")
            string namespaceContinuation = line.Trim();
            namespaceContinuation = namespaceContinuation.Remove(0, ImportKeyword.Length).Trim();

            // get project references for the project that the current file is in
            ProjectHierarchy projects = new ProjectHierarchy(service);
            VSProject project = projects.GetContainingProject(fileName);

            return new BooDeclarations(availableNamespaces.QueryNamespacesFromReferences(project.References, namespaceContinuation));
        }

        private Declarations GetScopedIntellisenseDeclarations(int lineNum)
        {
            // get the node that the caret is in
            Keywords keywords = new Keywords();
            BooParseTreeNodeFlatterner flattener = new BooParseTreeNodeFlatterner();

            IBooParseTreeNode scope = compiledDocument.GetScopeByLine(lineNum);

            keywords.InjectIntoScope(scope);

            return new BooDeclarations(flattener.FlattenFrom(scope));
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

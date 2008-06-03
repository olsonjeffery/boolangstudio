using System.Text.RegularExpressions;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;
using Boo.BooLangService.VSInterop;
using BooLangService;
using Boo.BooLangService.VSInterop;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangService.Intellisense
{
    public class DeclarationFinder
    {
        private readonly CompiledDocument compiledDocument;
        private const string ImportKeyword = "import";
        private readonly Regex IntellisenseTargetRegex = new Regex("[^ (]*$", RegexOptions.Compiled);
        private readonly ILineView lineView;
        private readonly string fileName;
        private readonly IProjectReferenceLookup projectReferences;

        public DeclarationFinder(CompiledDocument compiledDocument, IProjectReferenceLookup projectReferenceLookup, ILineView lineView, string fileName)
        {
            this.compiledDocument = compiledDocument;
            this.projectReferences = projectReferenceLookup;
            this.lineView = lineView;
            this.fileName = fileName;
        }

        public BooDeclarations Find(int lineNum, int colNum)
        {
            string line = lineView.GetTextUptoPosition(lineNum, colNum);

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

            if (line.EndsWith("."))
            {
                // Member Lookup: it's easier to check if the line ends in a . rather than checking
                // the parse reason, because the parse reason may not technically be correct.
                // for example "Syst[ctrl+space]" is a complete word, while "System[.]" is
                // a member lookup; however, "System.[ctrl+space]" is a member lookup but it gets
                // reported as a complete word because of the shortcut used.
                // So it's easier just to say any lines ending in "." are member lookups, and everything
                // else is complete word.
                return GetMemberLookupIntellisenseDeclarations(line);
            }

            // Everything else (complete word)
            return GetScopedIntellisenseDeclarations(lineNum);
        }

        private BooDeclarations GetImportIntellisenseDeclarations(string line)
        {
            // get any namespace already written (i.e. "Boo.Lang.")
            string intellisenseTarget = GetIntellisenseTarget(line);
            var declarations = new BooDeclarations();

            AddNamespacesFromReferences(declarations, intellisenseTarget);

            return declarations;
        }

        private BooDeclarations GetMemberLookupIntellisenseDeclarations(string line)
        {
            string intellisenseTarget = GetIntellisenseTarget(line);
            var declarations = new BooDeclarations();



            return declarations;
        }

        private BooDeclarations GetScopedIntellisenseDeclarations(int lineNum)
        {
            // get the node that the caret is in
            var scopedParseTree = compiledDocument.GetScopeByLine(lineNum);
            var declarations = new BooDeclarations();

            AddMembersFromScopeTree(declarations, scopedParseTree);
            AddKeywords(declarations, scopedParseTree);
            AddImports(declarations);
            AddReferences(declarations);

            return declarations;
        }

        private string GetIntellisenseTarget(string line)
        {
            return IntellisenseTargetRegex.Match(line).Groups[0].Value;
        }

        /// <summary>
        /// Adds members from the current scope (flattened, so all containing scopes are included) to
        /// the declarations.
        /// </summary>
        private void AddMembersFromScopeTree(BooDeclarations declarations, IBooParseTreeNode scopedParseTree)
        {
            var parseTreeFlattener = new BooParseTreeNodeFlatterner();

            declarations.Add(parseTreeFlattener.FlattenFrom(scopedParseTree));
        }

        /// <summary>
        /// Adds keywords based on the current scope to the declarations.
        /// </summary>
        private void AddKeywords(BooDeclarations declarations, IBooParseTreeNode scopedParseTree)
        {
            var keywords = new TypeKeywordResolver();

            declarations.Add(keywords.GetForScope(scopedParseTree));
        }

        /// <summary>
        /// Adds any types and namespaces, imported at the start of the document, to the declarations.
        /// </summary>
        private void AddImports(BooDeclarations declarations)
        {
            // add imports to declarations
            foreach (var importNamespace in compiledDocument.Imports.Keys)
            {
                var importedNodes = compiledDocument.Imports[importNamespace];

                declarations.Add(importedNodes);
            }
        }

        private void AddReferences(BooDeclarations declarations)
        {
            AddNamespacesFromReferences(declarations, "");
        }

        /// <summary>
        /// Adds namespaces from references to the declarations.
        /// </summary>
        private void AddNamespacesFromReferences(BooDeclarations declarations, string namespaceContinuation)
        {
            declarations.Add(projectReferences.GetReferencedsNamespacesInProjectContaining(fileName, namespaceContinuation));
        }
    }
}
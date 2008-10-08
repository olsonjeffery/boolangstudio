using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.BooLangService.StringParsing;
using Boo.BooLangStudioSpecs.Document;
using Boo.Lang.Compiler.TypeSystem;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangService.Intellisense
{
    public class DeclarationFinder
    {
        private const string ImportKeyword = "import";
        
        private readonly CompiledProject compiledProject;
        private readonly ILineView lineView;
        private readonly string fileName;

        public DeclarationFinder(CompiledProject compiledProject, ILineView lineView, string fileName)
        {
            this.compiledProject = compiledProject;
            this.lineView = lineView;
            this.fileName = fileName;
        }

        public IntellisenseDeclarations Find(CaretLocation caretLocation, ParseReason parseReason)
        {
            if (!caretLocation.IsValid)
                throw new ArgumentException("Caret location has not been provided, cannot continue.");

            return Find(caretLocation.Line.Value, caretLocation.Column.Value, parseReason);
        }

        /// <summary>
        /// Finds any intellisense declarations relative to the current caret position.
        /// </summary>
        /// <param name="lineNum">Caret line</param>
        /// <param name="colNum">Caret column</param>
        /// <param name="reason">Reason for parse</param>
        /// <returns>IntellisenseDeclarations</returns>
        public IntellisenseDeclarations Find(int lineNum, int colNum, ParseReason reason)
        {
            string line = lineView.GetTextUptoPosition(lineNum, colNum);
            bool isImportStatement = line.StartsWith(ImportKeyword);
            var declarations = (isImportStatement) ? new ImportIntellisenseDeclarations() : new IntellisenseDeclarations();

            if (line.EndsWith(".") || reason == ParseReason.MemberSelect)
            {
                if (isImportStatement) line = line.Remove(0, ImportKeyword.Length).Trim(); // remove import keyword

                // Member Select: it's easier to check if the line ends in a . rather than checking
                // the parse reason, because the parse reason may not technically be correct.
                // for example "Syst[ctrl+space]" is a complete word, while "System[.]" is
                // a member lookup; however, "System.[ctrl+space]" is a member lookup but it gets
                // reported as a complete word because of the shortcut used.
                // So it's easier just to say any lines ending in "." are member lookups, and everything
                // else is complete word.
                AddMemberLookupDeclarations(declarations, line, lineNum);

                return declarations;
            }

            // Everything else (complete word)
            return GetScopedIntellisenseDeclarations(lineNum);
        }

        private void AddMemberLookupDeclarations(IntellisenseDeclarations declarations, string lineSource, int line)
        {
            var members = GetMembersFromCurrentScope(line, lineSource);

            members.ForEach(e =>
            {
                IBooParseTreeNode node = e.ToTreeNode();

                if (node != null)
                    declarations.Add(node);
            });

            declarations.Sort();
        }

        private List<ISourceOrigin> GetMembersFromCurrentScope(int line, string lineSource)
        {
            var scopedDeclarations = GetScopedIntellisenseDeclarations(line);
            var invocationStack = GetInvocationStack(lineSource);

            IBooParseTreeNode firstNode = scopedDeclarations.Find(e => e.Name.Equals(invocationStack[0].Name, StringComparison.OrdinalIgnoreCase));
            ISourceOrigin entity = firstNode.SourceOrigin;
            var constructor = invocationStack[0].HadParentheses;

            for (int i = 1; i < invocationStack.Length; i++)
            {
                var invocation = invocationStack[i];
                var members = entity.GetMembers(constructor);
                var matchingMember = members.Find(e => e.Name == invocation.Name);

                constructor = false;

                if (matchingMember != null)
                    entity = matchingMember;
            }

            return entity.GetMembers(constructor);
        }

        private Invocation[] GetInvocationStack(string lineSource)
        {
            var lineParser = new LineEntityParser();

            if (lineSource.EndsWith("."))
                lineSource = lineSource.Substring(0, lineSource.Length - 1);

            return lineParser.GetEntityNames(lineSource);
        }

        private IntellisenseDeclarations GetScopedIntellisenseDeclarations(int lineNum)
        {
            // get the node that the caret is in
            var scopedParseTree = compiledProject.GetScope(fileName, lineNum);
            var declarations = new IntellisenseDeclarations();

            AddSpecialTypes(declarations);
            AddMembersFromScopeTree(declarations, scopedParseTree);
            AddKeywords(declarations, scopedParseTree);
            AddImports(declarations, GetDocument(scopedParseTree));
            AddReferences(declarations, (ProjectTreeNode)compiledProject.ParseTree);

            declarations.Sort();

            return declarations;
        }

        private void AddSpecialTypes(IntellisenseDeclarations declarations)
        {
            ISourceOrigin origin = new TypeSourceOrigin(typeof(string));

            declarations.Add(origin.ToTreeNode());
        }

        private DocumentTreeNode GetDocument(IBooParseTreeNode node)
        {
            var currentNode = node;

            while (!(currentNode is DocumentTreeNode))
            {
                currentNode = currentNode.Parent;
            }

            return currentNode as DocumentTreeNode;
        }

        /// <summary>
        /// Adds members from the current scope (flattened, so all containing scopes are included) to
        /// the declarations.
        /// </summary>
        private void AddMembersFromScopeTree(IntellisenseDeclarations declarations, IBooParseTreeNode scopedParseTree)
        {
            var parseTreeFlattener = new BooParseTreeNodeFlatterner();

            declarations.AddRange(parseTreeFlattener.FlattenFrom(scopedParseTree));
        }

        /// <summary>
        /// Adds keywords based on the current scope to the declarations.
        /// </summary>
        private void AddKeywords(IntellisenseDeclarations declarations, IBooParseTreeNode scopedParseTree)
        {
            var keywords = new TypeKeywordResolver();

            declarations.Add(keywords.GetForScope(scopedParseTree));
        }

        /// <summary>
        /// Adds any types and namespaces, imported at the start of the document, to the declarations.
        /// </summary>
        private void AddImports(IntellisenseDeclarations declarations, DocumentTreeNode document)
        {
            // add imports to declarations
            foreach (var importNamespace in document.Imports.Keys)
            {
                ImportedNamespaceTreeNode importedNodes = document.Imports[importNamespace];

                foreach (var member in importedNodes.SourceOrigin.GetMembers(false))
                {
                    declarations.Add(member.ToTreeNode());
                }

                declarations.Add(importedNodes);
            }
        }

        private void AddReferences(IntellisenseDeclarations declarations, ProjectTreeNode project)
        {
            var namespaces = new List<IBooParseTreeNode>();

            foreach (var ns in project.ReferencedNamespaces.Keys)
            {
                namespaces.Add(project.ReferencedNamespaces[ns]);
            }

            declarations.AddRange(namespaces);
        }
    }
}
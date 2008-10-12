using System;
using System.Collections.Generic;
using System.Diagnostics;
using Boo.BooLangProject;
using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;
using Boo.BooLangService.StringParsing;
using BooLangService;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangService
{
    public class ParseRequestProcessor
    {
        private readonly LanguageService languageService;

        public ParseRequestProcessor(LanguageService languageService)
        {
            this.languageService = languageService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Initially I thought it would be enough to just compile the documents and be done with it, but VS also wants a load of string
        /// manip doing.
        /// 
        /// I figure we need to do a two stage Parse, the first part compiles the docs to build up the code DOM (so we can easily find members
        /// etc), and then the second part works on the current view's string content, allowing brace matching and what not. We can't use the
        /// compiled source, because the boo compiler alters it durin compilation (adds implied constructors etc... which screws up the line
        /// numbers).
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuthoringScope GetAuthoringScopeForRequest(ParseRequest request)
        {
            var compiledProject = GetCompiledProject(request);
            var viewSource = request.Text;

            // Autos:
            // Parse the code block at the given location to obtain any expressions that might be of interest in the Autos debugging window
            // (an expression is the name of variable or parameter that can be evaluated to produce a value). 
            if (request.Reason == ParseReason.Autos) return GetAutos(request);

            // Check:
            // Parse the entire source file, checking for errors. This pass should also create lists of matching language pairs, triplets,
            // members, and methods. 
            if (request.Reason == ParseReason.Check) return CheckForErrors(request);

            // CodeSpan:
            // Parse the section of code containing the specified location to find the extent of the statement. Used in validating breakpoints. 
            if (request.Reason == ParseReason.CodeSpan) return FindStatementExtent(request);

            // CompleteWord:
            // Parse to get the partially completed word before the current position in order to show a list of possible completions (members,
            // arguments, methods). 
            if (request.Reason == ParseReason.CompleteWord) return GetCompletionSuggestions(request);

            // DisplayMemberList:
            // Parse the separator and the possible name before it, to obtain a list of members to be shown in a member completion list. 
            if (request.Reason == ParseReason.DisplayMemberList) return GetMemberList(request);

            // Goto:
            // Parse the identifier or expression at the specified location to obtain a possible URI of a file where the identifier is defined,
            // declared, or referenced. 
            if (request.Reason == ParseReason.Goto) return GetGotoTarget(request);

            // HighlightBraces:
            // Parse to find the matching language pairs (such as "{" and "}" or "<" and ">") that enclose the given location so they and their
            // contents can be highlighted. 
            if (request.Reason == ParseReason.HighlightBraces) return HighlightBraces(request, compiledProject);

            // MatchBraces:
            // Parse the language pair at the given location to finds its match. 
            if (request.Reason == ParseReason.MatchBraces) return FindMatchingBrace(request);

            // MemberSelect:
            // Parse the separator character before the current location to obtain a list of members for the class. 
            if (request.Reason == ParseReason.MemberSelect) return FindMembers(request, compiledProject);

            // MemberSelectAndHighlightBraces:
            // Parse the character at the current location to complete a member selection and to highlight the matching pair to the parsed
            // character (such as a ")" after a method name). 
            if (request.Reason == ParseReason.MemberSelectAndHighlightBraces) return FindMembersAndHighlightBraces(request);

            // MethodTip:
            // Parse the method name before the current position to produce a list of all overloaded method signatures that match the method name. 
            if (request.Reason == ParseReason.MethodTip) return GetMethodTip(request, compiledProject);

            return GetDefault(request, compiledProject);
        }

        private AuthoringScope GetDefault(ParseRequest request, CompiledProject compiledProject)
        {
            Debug.WriteLine("Default");
            return new BooScope(compiledProject, (BooSource)languageService.GetSource(request.View), request, this);
        }

        private CompiledProject GetCompiledProject(ParseRequest request)
        {
            var project = GetProject(request);
            return project.GetCompiledProject();
        }

        // QuickInfo:
        // Parse the identifier or selection at the given location to obtain type information to be shown in an IntelliSense quick info tool tip. 
        public string GetQuickInfo(int line, int column, ParseRequest request)
        {
            Debug.WriteLine("GetQuickInfo");
            var compiledProject = GetCompiledProject(request);
            var source = (BooSource)languageService.GetSource(request.View);
            var lineContents = source.GetLine(line);
            var parser = new LineEntityParser();

            if (lineContents.EndsWith(":"))
                lineContents = lineContents.Substring(0, lineContents.Length - 1);

            var invocations = parser.GetEntityNames(lineContents);

            var finder = new DeclarationFinder(compiledProject, source, request.FileName);
            var declarations = finder.Find(request.Line, request.Col, request.Reason);

            var node = declarations.Find(n => n.Name.Equals(invocations[0].Name, StringComparison.OrdinalIgnoreCase));

            return node == null ? "" : node.GetIntellisenseDescription();
        }

        private AuthoringScope FindMembersAndHighlightBraces(ParseRequest request)
        {
            Debug.WriteLine("FindMembersAndHighlightBraces");
            throw new System.NotImplementedException();
        }

        private AuthoringScope FindMembers(ParseRequest request, CompiledProject compiledProject)
        {
            Debug.WriteLine("FindMembers");
            return GetDefault(request, compiledProject);
        }

        private AuthoringScope FindMatchingBrace(ParseRequest request)
        {
            Debug.WriteLine("FindMatchingBrace");
            throw new System.NotImplementedException();
        }

        private AuthoringScope HighlightBraces(ParseRequest request, CompiledProject compiledProject)
        {
            Debug.WriteLine("HighlightBraces");

            var source = languageService.GetSource(request.View);
            var indexOfOriginal = source.GetPositionOfLineIndex(request.Line, request.Col - 1);

            var bracketFinder = new BracketPairFinder(BracketPairType.FromChar(request.Text[indexOfOriginal]));
            var bracketPairs = bracketFinder.FindPairs(request.Text);

            int? partner = bracketPairs.FindPartnerIndex(indexOfOriginal);

            if (partner != null)
            {
                request.Sink.FoundMatchingBrace = true;

                int nextLine, nextCol;

                source.GetLineIndexOfPosition(partner.Value, out nextLine, out nextCol);

                request.Sink.MatchPair(
                    new TextSpan
                    {
                        iStartLine = request.Line,
                        iEndLine = request.Line,
                        iStartIndex = request.Col - 1,
                        iEndIndex = request.Col
                    }, 
                    new TextSpan
                    {
                        iStartLine = nextLine,
                        iEndLine = nextLine,
                        iStartIndex = nextCol,
                        iEndIndex = nextCol + 1
                    }, 0);
            }

            return GetDefault(request, compiledProject);
        }

        private AuthoringScope GetGotoTarget(ParseRequest request)
        {
            Debug.WriteLine("GetGotoTarget");
            throw new System.NotImplementedException();
        }

        private AuthoringScope GetMemberList(ParseRequest request)
        {
            Debug.WriteLine("GetMemberList");
            throw new System.NotImplementedException();
        }

        private AuthoringScope GetCompletionSuggestions(ParseRequest request)
        {
            Debug.WriteLine("GetCompletionSuggestions");
            throw new System.NotImplementedException();
        }

        private AuthoringScope FindStatementExtent(ParseRequest request)
        {
            Debug.WriteLine("FindStatementExtent");
            throw new System.NotImplementedException();
        }

        private AuthoringScope CheckForErrors(ParseRequest request)
        {
            Debug.WriteLine("CheckForErrors");
            throw new System.NotImplementedException();
        }

        private AuthoringScope GetAutos(ParseRequest request)
        {
            Debug.WriteLine("GetAutos");
            throw new System.NotImplementedException();
        }

        private string methodName;

        private AuthoringScope GetMethodTip(ParseRequest request, CompiledProject compiledProject)
        {
            Debug.WriteLine("GetMethodTip");

            var source = languageService.GetSource(request.View);
            var previousWordSpan = GetPreviousWord(request);
            var previousWord = source.GetText(previousWordSpan);
            var method = FindInScope(request, compiledProject, previousWord) as MethodTreeNode;

            if (method != null)
                methodName = previousWord; // method name
            else
                previousWord = methodName; // were parsing a parameter, so used our stored method namae

            request.Sink.StartName(previousWordSpan, previousWord);
            request.Sink.StartParameters(previousWordSpan);

            // still need to add EndParameters here, otherwise the tip doesn't close

            var methods = new BooMethods(method);

            return new BooScope(compiledProject, (BooSource)languageService.GetSource(request.View), request, methods, this);
        }

        private IBooParseTreeNode FindInScope(ParseRequest request, CompiledProject compiledProject, string find)
        {
            var source = languageService.GetSource(request.View);
            var finder = new DeclarationFinder(compiledProject, (ILineView)source, request.FileName);
            var declarations = finder.Find(request.Line, request.Col, request.Reason);

            return declarations.Find(n => n.Name.Equals(find, StringComparison.OrdinalIgnoreCase)) as MethodTreeNode;
        }

        private TextSpan GetPreviousWord(ParseRequest request)
        {
            TextSpan[] spans = new TextSpan[1];

            if (request.View.GetWordExtent(request.Line, request.Col + 1, (uint)WORDEXTFLAGS.WORDEXT_PREVIOUS, spans) != VSConstants.S_OK || spans.Length == 0)
                throw new ParseRequestException("Couldn't find previous word.");

            return spans[0];
        }

        private BooProjectSources GetProject(ParseRequest request)
        {
            BooProjectSources project = BooProjectSources.Find(request.FileName);

            if (project != null)
                project.Update(request);

            return project;
        }
    }
}
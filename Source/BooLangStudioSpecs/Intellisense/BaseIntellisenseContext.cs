using System;
using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Intellisense;
using Boo.BooLangStudioSpecs.Document;
using Boo.BooLangStudioSpecs.Intellisense.Stubs;
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public abstract class BaseIntellisenseContext : BaseCompilerContext
    {
        private const string Caret = "~";

        protected CompiledProject Compile(out string line, out int lineNum, out int colNum, string text)
        {
            // kill off the first newline because of the style of text we're doing
            if (text.StartsWith(Environment.NewLine))
                text = text.Remove(0, Environment.NewLine.Length);

            GetLineAndCol(text, out line, out lineNum, out colNum);

            return Compile(text.Replace(Caret, ""));
        }

        private void GetLineAndCol(string text, out string line, out int lineNum, out int col)
        {
            var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                var currentLine = lines[i];

                if (currentLine.Contains(Caret))
                {
                    line = currentLine.Replace(Caret, "");
                    lineNum = i + 1;
                    col = currentLine.IndexOf(Caret); // don't add 1 to this because we'll be removing the caret char
                    return;
                }
            }

            line = "";
            lineNum = 0;
            col = 0;
        }

        protected DeclarationFinder CreateFinder(CompiledProject project, CaretLocation caretLocation, params string[] referencedNamespaces)
        {
            var lineView = new SimpleStubLineView(caretLocation.LineSource);
            var referenceLookup = new SimpleStubProjectReferenceLookup();

            referenceLookup.AddFakeNamespaces(referencedNamespaces);

            return new DeclarationFinder(project, referenceLookup, lineView, caretLocation.FileName);

        }

        [Obsolete]
        protected DeclarationFinder CreateFinder(CompiledProject project, string line, params string[] referencedNamespaces)
        {
            var lineView = new SimpleStubLineView(line);
            var referenceLookup = new SimpleStubProjectReferenceLookup();

            referenceLookup.AddFakeNamespaces(referencedNamespaces);

            return new DeclarationFinder(project, referenceLookup, lineView, Constants.DocumentFileName);
        }

        protected void ValidatePresenceOfDeclarations(IntellisenseDeclarations declarations, params string[] expectedDeclarationNames)
        {
            Dictionary<string, bool> foundDeclarations = GetExpectedDeclarations(expectedDeclarationNames, declarations);

            // assert each entry is true in the dictionary, meaning all declarations were
            // found in the list
            foreach (var name in foundDeclarations.Keys)
            {
                Assert.True(foundDeclarations[name],
                              "Expected to find declaration '" + name + "' in list of intellisense declarations, but didn't.");
            }
        }

        protected void ValidateNonPresenceOfDeclarations(IntellisenseDeclarations declarations, params string[] expectedDeclarationNames)
        {
            Dictionary<string, bool> foundDeclarations = GetExpectedDeclarations(expectedDeclarationNames, declarations);

            // assert each entry is true in the dictionary, meaning all declarations were
            // found in the list
            foreach (var name in foundDeclarations.Keys)
            {
                Assert.False(foundDeclarations[name],
                              "Expected to NOT find declaration '" + name + "' in list of intellisense declarations, but did.");
            }
        }

        private Dictionary<string, bool> GetExpectedDeclarations(string[] expectedDeclarationNames, IntellisenseDeclarations declarations)
        {
            var foundDeclarations = new Dictionary<string, bool>();

            // setup expectations
            foreach (var name in expectedDeclarationNames)
            {
                foundDeclarations[name] = false;
            }

            // iterate declarations, setting each found one in the foundDecs dictionary
            for (int i = 0; i < declarations.GetCount(); i++)
            {
                string name = declarations.GetName(i);

                if (foundDeclarations.ContainsKey(name))
                    foundDeclarations[name] = true;
            }
            return foundDeclarations;
        }

        protected IntellisenseDeclarations GetDeclarations(string code)
        {
            string line;
            int lineNum, colNum;

            var project = Compile(out line, out lineNum, out colNum, code);
            var finder = CreateFinder(project, line);

            return finder.Find(lineNum, colNum, ParseReason.None);
        }
    }
}
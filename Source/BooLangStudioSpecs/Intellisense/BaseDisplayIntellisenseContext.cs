using System;
using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Intellisense;
using Boo.BooLangStudioSpecs.Intellisense.Stubs;
using MbUnit.Framework;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public abstract class BaseDisplayIntellisenseContext : BaseCompilerContext
    {
        private const string Caret = "~";

        protected CompiledDocument Compile(out string line, out int lineNum, out int colNum, string text)
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

        protected DeclarationFinder CreateFinder(CompiledDocument document, string line, params string[] referencedNamespaces)
        {
            var lineView = new SimpleStubLineView(line);
            var referenceLookup = new SimpleStubProjectReferenceLookup();

            referenceLookup.AddFakeNamespaces(referencedNamespaces);

            return new DeclarationFinder(document, referenceLookup, lineView, "fileName");
        }

        protected void ValidatePresenceOfDeclarations(BooDeclarations declarations, params string[] expectedDeclarationNames)
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

            // assert each entry is true in the dictionary, meaning all declarations were
            // found in the list
            foreach (var name in foundDeclarations.Keys)
            {
                Assert.IsTrue(foundDeclarations[name],
                              "Expected to find declaration '" + name + "' in list of intellisense declarations, but didn't.");
            }
        }
    }
}
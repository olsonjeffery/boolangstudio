using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;

namespace Boo.BooLangStudioSpecs
{

    public abstract class LexingBaseFixture
    {
        protected antlr.TokenStream lexer;
        protected List<TokenInfo> tokens;
        protected Boo.BooLangService.BooScanner scanner;
        protected string rawCodeString = " ";

        public LexingBaseFixture()
        {
            lexer = GetLexer(rawCodeString);
            tokens = new List<TokenInfo>();
            scanner = new Boo.BooLangService.BooScanner();
        }

        public antlr.TokenStream GetLexer(string line)
        {
            return Boo.Lang.Parser.BooParser.CreateBooLexer(1, "", new StringReader(line));
        }

        protected int _mlState = 0;
        public void BuildTokens(string line)
        {
            scanner.SetSource(line, 0);
            bool moreTokens = true;
            //TokenInfo token = new TokenInfo();
            //int state = 0;
            tokens.Clear();
            while (moreTokens)
            {
                TokenInfo token = new TokenInfo();

                bool yetMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref _mlState);
                tokens.Add(token);
                moreTokens = yetMoreTokens;
                Console.WriteLine("type: " + token.Type.ToString() + " start: " + token.StartIndex.ToString() + " end: " + token.EndIndex + " ");
            }
        }
    }
}
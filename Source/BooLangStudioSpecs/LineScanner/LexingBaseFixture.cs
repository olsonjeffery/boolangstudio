using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;
using BooPegLexer;

namespace Boo.BooLangStudioSpecs
{

    public abstract class LexingBaseFixture : AutoMockingTestFixture
    {
        protected antlr.TokenStream lexer;
        protected List<TokenInfo> tokens;
        protected Boo.BooLangService.BooScanner scanner;
        protected string rawCodeString = " ";

        public LexingBaseFixture()
            : base()
        {
            lexer = GetLexer(rawCodeString);
            tokens = new List<TokenInfo>();
            scanner = new Boo.BooLangService.BooScanner(new PegLexer());
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

                moreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref _mlState);
                if (moreTokens)
                {
                    tokens.Add(token);
                    Console.WriteLine("type: " + token.Type.ToString() + " start: " + token.StartIndex.ToString() + " end: " + token.EndIndex + " ");
                }
                
            }
        }
    }
}
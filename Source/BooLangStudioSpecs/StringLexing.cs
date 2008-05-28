using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Microsoft.VisualStudio.Package;
using System.IO;
using Context = MbUnit.Framework.TestFixtureAttribute;
using Spec = MbUnit.Framework.TestAttribute;

namespace Boo.BooLangStudioSpecs
{
    [Context]
    public abstract class LexingBaseFixture
    {
        protected antlr.TokenStream lexer;
        protected List<TokenInfo> tokens;
        protected Boo.BooLangService.BooScanner scanner;
        protected string rawCodeString = " ";

        [SetUp]
        public virtual void SetUp()
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

                bool yetMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref _mlState );
                tokens.Add(token);
                moreTokens = yetMoreTokens;
            }
        }
    }

    [Context]
    public abstract class StringLexingContext : LexingBaseFixture
    {

        protected TokenInfo trimmed;
        protected void BuildTrimmed(TokenType type)
        {
            List<TokenInfo> trimList = new List<TokenInfo>(from i in tokens
                                                           where i.Type == type
                                                           select i);
            Assert.IsTrue(trimList.Count == 1, "Expected 1, actual " + trimList.Count.ToString());
            trimmed = trimList[0];
        }
    }

    [Context]
    public class WhenParsingSingleQuotes : StringLexingContext
    {
        
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            //                0         1         2         3
            //                012345678901234567890123456789012345
            rawCodeString = @"someMethodClass('aStringParameter')";
            // run this guy last
            BuildTokens(rawCodeString);
            
        }


        [Spec]
        public void TokenShouldStartAndEndAtAppropIndex()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.StartIndex == 16, "Failed start index check. Expected 16, Actual " + trimmed.StartIndex.ToString());
            Assert.IsTrue(trimmed.EndIndex == 33, "Failed end index check. Expected 33, Actual " + trimmed.EndIndex.ToString());
        }
        [Spec]
        public void TokenShouldHaveTokenTypeOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.Type == TokenType.String);
        }
        [Spec]
        public void TokenShouldHaveTokenColorOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.Color == TokenColor.String);
        }

        [Spec]
        public void PreceedingTokenShouldNotStepOnStringsColorizingInfo()
        {
            List<TokenInfo> parenTokens = new List<TokenInfo>(from i in tokens
                                                              where i.Type == TokenType.Delimiter
                                                              select i);
            Assert.IsTrue(parenTokens.Count == 2);
            Assert.IsTrue(parenTokens[0].StartIndex == 15, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.IsTrue(parenTokens[0].EndIndex == 15, "LPAREN end Actual " + parenTokens[0].EndIndex);
            Assert.IsTrue(parenTokens[1].StartIndex == 34, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.IsTrue(parenTokens[1].EndIndex == 34, "LPAREN end Actual " + parenTokens[0].EndIndex);

        }
    }

    /// <summary>
    /// These tests are repetitive of the ones above.. should merge into a 
    /// single set of data-driven tests
    /// </summary>
    [Context]
    public class WhenParsingDoubleQuotes : StringLexingContext
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            //               0         1         2         3
            //               01234567890123456678901234567890123345
            rawCodeString = "someMethodClass(\"aStringParameter\")";
            // run this guy last
            BuildTokens(rawCodeString);

        }


        [Spec]
        public void TokenShouldStartAndEndAtAppropIndex()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.StartIndex == 16, "Failed start index check. Expected 16, Actual " + trimmed.StartIndex.ToString());
            Assert.IsTrue(trimmed.EndIndex == 33, "Failed end index check. Expected 33, Actual " + trimmed.EndIndex.ToString());
        }
        [Spec]
        public void TokenShouldHaveTokenTypeOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.Type == TokenType.String);
        }
        [Spec]
        public void TokenShouldHaveTokenColorOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.Color == TokenColor.String);
        }

        [Spec]
        public void PreceedingTokenShouldNotStepOnStringsColorizingInfo()
        {
            List<TokenInfo> parenTokens = new List<TokenInfo>(from i in tokens
                                                              where i.Type == TokenType.Delimiter
                                                              select i);
            Assert.IsTrue(parenTokens.Count == 2, "Expected 2, actual paren count: "+parenTokens.Count.ToString());
            Assert.IsTrue(parenTokens[0].StartIndex == 15, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.IsTrue(parenTokens[0].EndIndex == 15, "LPAREN end Actual " + parenTokens[0].EndIndex);
            Assert.IsTrue(parenTokens[1].StartIndex == 34, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.IsTrue(parenTokens[1].EndIndex == 34, "LPAREN end Actual " + parenTokens[0].EndIndex);

        }

    }

    /// <summary>
    /// also repetitive
    /// </summary>
    [Context]
    public class WhenParsingTripleQuotes : StringLexingContext
    {

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            //               0         1            2
            //               012345678901122334567890123456677889
            rawCodeString = "methodCall(\"\"\"tripleQuotes\"\"\")";
            BuildTokens(rawCodeString);
        }

        [Spec]
        public void TokenShouldStartAndEndAtAppropIndex()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.StartIndex == 11, "Failed start index check. Expected 16, Actual " + trimmed.StartIndex.ToString());
            Assert.IsTrue(trimmed.EndIndex == 28, "Failed end index check. Expected 33, Actual " + trimmed.EndIndex.ToString());
        }
        [Spec]
        public void TokenShouldHaveTokenTypeOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.Type == TokenType.String);
        }
        [Spec]
        public void TokenShouldHaveTokenColorOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.IsTrue(trimmed.Color == TokenColor.String);
        }

        [Spec]
        public void PreceedingTokenShouldNotStepOnStringsColorizingInfo()
        {
            List<TokenInfo> parenTokens = new List<TokenInfo>(from i in tokens
                                                              where i.Type == TokenType.Delimiter
                                                              select i);
            Assert.IsTrue(parenTokens.Count == 2);
            Assert.IsTrue(parenTokens[0].StartIndex == 10, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.IsTrue(parenTokens[0].EndIndex == 10, "LPAREN end Actual " + parenTokens[0].EndIndex);
            Assert.IsTrue(parenTokens[1].StartIndex == 29, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.IsTrue(parenTokens[1].EndIndex == 29, "LPAREN end Actual " + parenTokens[0].EndIndex);

        }

    }

}

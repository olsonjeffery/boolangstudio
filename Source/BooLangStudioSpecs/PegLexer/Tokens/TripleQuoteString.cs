using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;
using Rhino.Mocks;
using Rhino.Testing.AutoMocking;
using BooPegLexer;
using Boo.BooLangService;

namespace Boo.BooLangStudioSpecs
{

    public class WhenParsingTheStartOfATripleQuoteRegion : ManualTokenTestFixture
    {
        public WhenParsingTheStartOfATripleQuoteRegion()
            : base()
        {
            //      00112234567
            line = "\"\"\" blah";
            offset = 0;
            state = 0;
            scanner.SetSource(line, offset);
            scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
        }

        [Fact]
        public void StateShouldBeFourteen()
        {
            Assert.True(state == 14, "Actual: " + state.ToString());
        }

        [Fact]
        public void TokenTypeShouldBeString()
        {
            Assert.True(token.Type == TokenType.String, "actual: " + token.Type.ToString());
        }

        [Fact]
        public void TokenColorShouldBeString()
        {
            Assert.True(token.Color == TokenColor.String, "actual: " + token.Color.ToString());
        }

        [Fact]
        public void StartIndexShouldBeZero()
        {
            Assert.True(token.StartIndex == 0, "Actual: " + token.StartIndex.ToString());
        }

        [Fact]
        public void EndIndexShouldBeNine()
        {
            Assert.True(token.EndIndex == 7, "Actual: " + token.EndIndex.ToString());
        }

    }


    public class WhenParsingIntraTripleQuoteStringRegion : ManualTokenTestFixture
    {
        public WhenParsingIntraTripleQuoteStringRegion()
            : base()
        {
            //      0123456789
            line = "allastring";
            offset = 0;
            state = 14;
            scanner.SetSource(line, offset);
            scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
        }

        [Fact]
        public void StateShouldBeFourteen()
        {
            Assert.True(state == 14, "Actual: " + state.ToString());
        }

        [Fact]
        public void TokenTypeShouldBeString()
        {
            Assert.True(token.Type == TokenType.String, "actual: " + token.Type.ToString());
        }

        [Fact]
        public void TokenColorShouldBeString()
        {
            Assert.True(token.Color == TokenColor.String, "actual: " + token.Color.ToString());
        }

        [Fact]
        public void StartIndexShouldBeZero()
        {
            Assert.True(token.StartIndex == 0, "Actual: " + token.StartIndex.ToString());
        }

        [Fact]
        public void EndIndexShouldBeNine()
        {
            Assert.True(token.EndIndex == 9, "Actual: " + token.EndIndex.ToString());
        }

    }

    public class WhenParsingTheEndOfATripleQuoteString : ManualTokenTestFixture
    {
        protected TokenInfo tripleQuoteStringCloseToken;
        protected TokenInfo whitespaceToken;
        int betweenRunCurrentIndex;
        bool resultOne;
        string remainingLine;

        public WhenParsingTheEndOfATripleQuoteString()
            : base()
        {
            //      012345678899001
            line = "the end \"\"\" ";
            offset = 0;
            state = 14;
            scanner.SetSource(line, offset);

            tripleQuoteStringCloseToken = new TokenInfo();
            resultOne = scanner.ScanTokenAndProvideInfoAboutIt(tripleQuoteStringCloseToken, ref state);

            betweenRunCurrentIndex = lexer.CurrentIndex;
            remainingLine = lexer.RemainingLine;

            whitespaceToken = new TokenInfo();
            scanner.ScanTokenAndProvideInfoAboutIt(whitespaceToken, ref state);
        }

        [Fact]
        public void RemainingLineAfterFirstParseShouldBeASingleSpace()
        {
            Assert.True(remainingLine == " ", "Actual: '" + remainingLine+"'");
        }

        [Fact]
        public void ResultOfFirstParseShouldBeTrue()
        {
            Assert.True(resultOne == true);
        }

        [Fact]
        public void SubstringIndexFromInBetweenCurrentIndexShouldBeASingleSpace()
        {
            Assert.True(line.Substring(betweenRunCurrentIndex) == " ", "Actual: '" + line.Substring(betweenRunCurrentIndex) + "'");
        }

        [Fact]
        public void CurrentIndexBetweenTokensShouldBeEleven()
        {
            Assert.True(betweenRunCurrentIndex == 11, "Actual: " + betweenRunCurrentIndex.ToString());
        }

        [Fact]
        public void StateShouldBeZero()
        {
            Assert.True(state == 0, "Actual: " + state.ToString());
        }

        [Fact]
        public void StringTokenTypeShouldBeString()
        {
            Assert.True(tripleQuoteStringCloseToken.Type == TokenType.String, "actual: " + tripleQuoteStringCloseToken.Type.ToString());
        }

        [Fact]
        public void StringTokenColorShouldBeString()
        {
            Assert.True(tripleQuoteStringCloseToken.Color == TokenColor.String, "actual: " + tripleQuoteStringCloseToken.Color.ToString());
        }

        [Fact]
        public void StringTokenStartIndexShouldBeZero()
        {
            Assert.True(tripleQuoteStringCloseToken.StartIndex == 0, "Actual: " + tripleQuoteStringCloseToken.StartIndex.ToString());
        }

        [Fact]
        public void StringTokenEndIndexShouldBeTen()
        {
            Assert.True(tripleQuoteStringCloseToken.EndIndex == 10, "Actual: " + tripleQuoteStringCloseToken.EndIndex.ToString());
        }

        [Fact]
        public void WhitespaceTokenTypeShouldBeWhitespace()
        {
            Assert.True(whitespaceToken.Type == TokenType.WhiteSpace, "actual: " + whitespaceToken.Type.ToString());
        }

        [Fact]
        public void WhitespaceTokenColorShouldBeText()
        {
            Assert.True(whitespaceToken.Color == TokenColor.Text, "actual: " + whitespaceToken.Color.ToString());
        }

        [Fact]
        public void WhitespaceTokenStartIndexShouldBeEleven()
        {
            Assert.True(whitespaceToken.StartIndex == 11, "Actual: " + whitespaceToken.StartIndex.ToString());
        }

        [Fact]
        public void WhitespaceTokenEndIndexShouldBeEleven()
        {
            Assert.True(whitespaceToken.EndIndex == 11, "Actual: " + whitespaceToken.EndIndex.ToString());
        }


    }
}
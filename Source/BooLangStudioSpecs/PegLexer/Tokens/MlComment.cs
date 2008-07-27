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

    public class WhenParsingTheStartOfAnMlCommentRegion : ManualTokenTestFixture
    {
        public WhenParsingTheStartOfAnMlCommentRegion()
            : base()
        {
            //      0123456789
            line = "/* comment";
            offset = 0;
            state = 0;
            scanner.SetSource(line, offset);
            scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
        }

        [Fact]
        public void StateShouldBeThirteen()
        {
            Assert.True(state == 13, "Actual: " + state.ToString());
        }

        [Fact]
        public void TokenTypeShouldBeComment()
        {
            Assert.True(token.Type == TokenType.Comment, "actual: " + token.Type.ToString());
        }

        [Fact]
        public void TokenColorShouldBeComment()
        {
            Assert.True(token.Color == TokenColor.Comment, "actual: " + token.Color.ToString());
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


    public class WhenParsingIntraMlCommentRegion : ManualTokenTestFixture
    {
        public WhenParsingIntraMlCommentRegion()
            : base()
        {
            //      0123456789
            line = "allcomment";
            offset = 0;
            state = 13;
            scanner.SetSource(line, offset);
            scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
        }

        [Fact]
        public void StateShouldBeThirteen()
        {
            Assert.True(state == 13, "Actual: " + state.ToString());
        }

        [Fact]
        public void TokenTypeShouldBeComment()
        {
            Assert.True(token.Type == TokenType.Comment, "actual: " + token.Type.ToString());
        }

        [Fact]
        public void TokenColorShouldBeComment()
        {
            Assert.True(token.Color == TokenColor.Comment, "actual: " + token.Color.ToString());
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

    public class WhenParsingTheEndOfAMlCommentRegion : ManualTokenTestFixture
    {
        protected TokenInfo commentCloseToken;
        protected TokenInfo whitespaceToken;
        int betweenRunCurrentIndex;
        bool resultOne;
        string remainingLine;

        public WhenParsingTheEndOfAMlCommentRegion()
            : base()
        {
            //      012345678901234
            line = "the end */ ";
            offset = 0;
            state = 13;
            scanner.SetSource(line, offset);

            commentCloseToken = new TokenInfo();
            resultOne = scanner.ScanTokenAndProvideInfoAboutIt(commentCloseToken, ref state);

            betweenRunCurrentIndex = lexer.CurrentIndex;
            remainingLine = lexer.RemainingLine;

            whitespaceToken = new TokenInfo();
            scanner.ScanTokenAndProvideInfoAboutIt(whitespaceToken, ref state);
        }

        [Fact]
        public void RemainingLineAfterFirstParseShouldBeASingleSpace()
        {
            Assert.True(remainingLine == " ", "Actual: " + remainingLine);
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
        public void CurrentIndexBetweenTokensShouldBeTen()
        {
            Assert.True(betweenRunCurrentIndex == 10, "Actual: " + betweenRunCurrentIndex.ToString());
        }

        [Fact]
        public void StateShouldBeZero()
        {
            Assert.True(state == 0, "Actual: " + state.ToString());
        }

        [Fact]
        public void CommentTokenTypeShouldBeComment()
        {
            Assert.True(commentCloseToken.Type == TokenType.Comment, "actual: " + commentCloseToken.Type.ToString());
        }

        [Fact]
        public void CommentTokenColorShouldBeComment()
        {
            Assert.True(commentCloseToken.Color == TokenColor.Comment, "actual: " + commentCloseToken.Color.ToString());
        }

        [Fact]
        public void CommentTokenStartIndexShouldBeZero()
        {
            Assert.True(commentCloseToken.StartIndex == 0, "Actual: " + commentCloseToken.StartIndex.ToString());
        }

        [Fact]
        public void CommentTokenEndIndexShouldBeNine()
        {
            Assert.True(commentCloseToken.EndIndex == 9, "Actual: " + commentCloseToken.EndIndex.ToString());
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
        public void WhitespaceTokenStartIndexShouldBeTen()
        {
            Assert.True(whitespaceToken.StartIndex == 10, "Actual: " + whitespaceToken.StartIndex.ToString());
        }

        [Fact]
        public void WhitespaceTokenEndIndexShouldBeTen()
        {
            Assert.True(whitespaceToken.EndIndex == 10, "Actual: " + whitespaceToken.EndIndex.ToString());
        }


    }
}
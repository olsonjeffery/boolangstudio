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

    public class TokenIdentifier : SingleTokenTestFixture
    {
        public TokenIdentifier()
            : base()
        {
            //      0         1
            //      012345678901
            line = "anIdentifier";
            offset = 0;

            expectedTokenType = TokenType.Identifier;
            expectedTokenColor = TokenColor.Text;
            expectedStartIndex = 0;
            expectedEndIndex = 11;

            BuildTokens(line, offset);
        }

    }

    public class TokenIdentifierStartingWithNumberShouldNotParseAsAnIdentifierButStillMaintainsPertinentInfo : AutoMockingTestFixture
    {

        PegLexer lexer;
        TokenType expectedTokenType;
        TokenColor expectedTokenColor;
        int expectedStartIndex;
        int expectedEndIndex;
        int state;
        BooScanner scanner;
        TokenInfo token;
        bool result;

        public TokenIdentifierStartingWithNumberShouldNotParseAsAnIdentifierButStillMaintainsPertinentInfo()
            : base()
        {
            //             0         1         2         3   
            //             012345678901234567890123456789012345678
            string line = "1aMalformedIdentifier + otherValidStuff";
            int offset = 0;

            expectedTokenType = TokenType.Unknown;
            expectedTokenColor = TokenColor.Text;
            expectedStartIndex = 0;
            expectedEndIndex = 38;
            state = 0;

            lexer = new PegLexer();
            scanner = new BooScanner(lexer);
            scanner.SetSource(line,offset);
            token = new TokenInfo();
            result = scanner.ScanTokenAndProvideInfoAboutIt(token,ref state);
            
        }

        [Fact]
        public void ResultShouldBeFalse()
        {
            Assert.False(result);
        }

        [Fact]
        public void TokenTypeShouldBeUnknown()
        {
            Assert.True(token.Type == TokenType.Unknown, "Actual: " + token.Type.ToString());
        }

        [Fact]
        public void TokenColorShouldBeText()
        {
            Assert.True(token.Color == TokenColor.Text, "Actual: " + token.Color.ToString());
        }

        [Fact]
        public void StartIndexShouldBeZero()
        {
            Assert.True(token.StartIndex == 0, "Actual: " + token.StartIndex.ToString());
        }

        [Fact]
        public void EndIndexShouldBeThirtyEight()
        {
            Assert.True(token.EndIndex == 38, "Actual: " + token.EndIndex.ToString());
        }

    }

}
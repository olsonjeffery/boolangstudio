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

    public class TokenMalformedSingleQuoteString : AutoTokenTestFixture
    {
        public TokenMalformedSingleQuoteString()
            : base()
        {
            //      0
            //      01234567
            line = "'foo bar";
            offset = 0;

            expectedTokenType = TokenType.String;
            expectedTokenColor = TokenColor.String;
            expectedStartIndex = 0;
            expectedEndIndex = 7;

            BuildTokens(line, offset);
        }

    }

    public class TokenMalformedDoubleQuoteString : AutoTokenTestFixture
    {
        public TokenMalformedDoubleQuoteString()
            : base()
        {
            //      0
            //      001234567
            line = "\"foo bar";
            offset = 0;

            expectedTokenType = TokenType.String;
            expectedTokenColor = TokenColor.String;
            expectedStartIndex = 0;
            expectedEndIndex = 7;

            BuildTokens(line, offset);
        }

    }
}
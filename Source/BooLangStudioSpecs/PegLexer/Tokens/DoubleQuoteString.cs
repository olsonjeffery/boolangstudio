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

    public class TokenBasicDoubleQuoteString : AutoTokenTestFixture
    {
        public TokenBasicDoubleQuoteString()
            : base()
        {
            //      0          1
            //      00123456789012345677
            line = "\"this is a string\"";
            offset = 0;

            expectedTokenType = TokenType.String;
            expectedTokenColor = TokenColor.String;
            expectedStartIndex = 0;
            expectedEndIndex = 17;

            BuildTokens(line, offset);
        }

    }

    public class TokenDoubleQuoteStringWithEscapedDelimiter : AutoTokenTestFixture
    {
        public TokenDoubleQuoteStringWithEscapedDelimiter()
            : base()
        {
            //      0          1           2         33
            //      0012345678901233445678901234567890011234567899
            line = "\"contains an \\\"embedded string\\\" string\"";
            offset = 0;

            expectedTokenType = TokenType.String;
            expectedTokenColor = TokenColor.String;
            expectedStartIndex = 0;
            expectedEndIndex = 39;

            BuildTokens(line, offset);
        }

    }

}
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

    public class TokenFloatLiteral : AutoTokenTestFixture
    {
        public TokenFloatLiteral()
            : base()
        {
            //      0
            //      0123
            line = "1.23";
            offset = 0;

            expectedTokenType = TokenType.Literal;
            expectedTokenColor = TokenColor.Number;
            expectedStartIndex = 0;
            expectedEndIndex = 3;

            BuildTokens(line, offset);
        }

    }

    public class TokenSignedFloatLiteral : AutoTokenTestFixture
    {
        public TokenSignedFloatLiteral()
            : base()
        {
            //      0
            //      01234
            line = "-1.23";
            offset = 0;

            expectedTokenType = TokenType.Literal;
            expectedTokenColor = TokenColor.Number;
            expectedStartIndex = 0;
            expectedEndIndex = 4;

            BuildTokens(line, offset);
        }

    }

}
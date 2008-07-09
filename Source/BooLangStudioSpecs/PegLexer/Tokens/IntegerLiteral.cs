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

    public class TokenIntegerLiteral : AutoTokenTestFixture
    {
        public TokenIntegerLiteral()
            : base()
        {
            //      0
            //      0123
            line = "1444";
            offset = 0;

            expectedTokenType = TokenType.Literal;
            expectedTokenColor = TokenColor.Number;
            expectedStartIndex = 0;
            expectedEndIndex = 3;

            BuildTokens(line, offset);
        }

    }

    public class TokenSignedIntegerLiteral : AutoTokenTestFixture
    {
        public TokenSignedIntegerLiteral()
            : base()
        {
            //      0
            //      01234
            line = "-2344";
            offset = 0;

            expectedTokenType = TokenType.Literal;
            expectedTokenColor = TokenColor.Number;
            expectedStartIndex = 0;
            expectedEndIndex = 4;

            BuildTokens(line, offset);
        }

    }

}
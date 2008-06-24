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

    public class TokenIdentifier : AutoTokenTestFixture
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

}
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

    public class TokenKeyword : AutoTokenTestFixture
    {
        public TokenKeyword()
            : base()
        {
            //      0
            //      012
            line = "def";
            offset = 0;

            expectedTokenType = TokenType.Keyword;
            expectedTokenColor = TokenColor.Keyword;
            expectedStartIndex = 0;
            expectedEndIndex = 2;

            BuildTokens(line, offset);
        }

    }

}
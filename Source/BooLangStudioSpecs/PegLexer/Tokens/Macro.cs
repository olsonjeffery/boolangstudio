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

    public class TokenMacro : AutoTokenTestFixture
    {
        public TokenMacro()
            : base()
        {
            //      0
            //      01234
            line = "using";
            offset = 0;

            expectedTokenType = TokenType.Keyword;
            expectedTokenColor = TokenColor.Keyword;
            expectedStartIndex = 0;
            expectedEndIndex = 4;

            BuildTokens(line, offset);
        }

    }

}
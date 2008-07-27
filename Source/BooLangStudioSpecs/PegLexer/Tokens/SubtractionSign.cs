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

    public class TokenSubtractionSign : AutoTokenTestFixture
    {
        public TokenSubtractionSign()
            : base()
        {
            //      0
            //      0
            line = "-";
            offset = 0;

            expectedTokenType = TokenType.Operator;
            expectedTokenColor = TokenColor.Text;
            expectedStartIndex = 0;
            expectedEndIndex = 0;

            BuildTokens(line, offset);
        }

    }

}
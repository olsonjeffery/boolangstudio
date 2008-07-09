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

    public class TokenDoubleWhackLineComment : AutoTokenTestFixture
    {
        public TokenDoubleWhackLineComment()
            : base()
        {
            //      0         1
            //      01234567890123456
            line = "// a line comment";
            offset = 0;

            expectedTokenType = TokenType.Comment;
            expectedTokenColor = TokenColor.Comment;
            expectedStartIndex = 0;
            expectedEndIndex = 16;

            BuildTokens(line, offset);
        }

    }

}
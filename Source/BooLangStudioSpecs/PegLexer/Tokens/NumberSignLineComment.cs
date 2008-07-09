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

    public class TokenNumberSignLineComment : AutoTokenTestFixture
    {
        public TokenNumberSignLineComment()
            : base()
        {
            //      0         1
            //      0123456789012345
            line = "# a line comment";
            offset = 0;

            expectedTokenType = TokenType.Comment;
            expectedTokenColor = TokenColor.Comment;
            expectedStartIndex = 0;
            expectedEndIndex = 15;

            BuildTokens(line, offset);
        }

    }

}
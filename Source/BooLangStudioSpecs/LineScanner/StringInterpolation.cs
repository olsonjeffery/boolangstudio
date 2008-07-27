using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;

namespace Boo.BooLangStudioSpecs
{
    public class WhenParsingStringsThatContainStringInterpolation : LexingBaseFixture
    {

        private TokenInfo stringToken;

        public WhenParsingStringsThatContainStringInterpolation()
            : base()
        {
            //                          1         2
            //               01234566789012345678901223456789
            rawCodeString = "val = \"blah ${foo} yay\"";
            BuildTokens(rawCodeString);

            List<TokenInfo> stringTokens = new List<TokenInfo>(from i in tokens
                                              where i.Type == TokenType.String
                                              select i
                                              );
            Assert.True(stringTokens.Count == 1, "more than one string token, actual: " + stringTokens.Count.ToString());
            stringToken = stringTokens[0];
        }
        
        [Fact]
        public void StringTokenStartsAndEndsInProperPlaceWithDoubleQuotes()
        {
            Assert.True(stringToken.StartIndex == 6, "Start index mismatch, expected 6, actual " + stringToken.StartIndex.ToString());
            Assert.True(stringToken.EndIndex == 22, "End index mismatch, expected 22, actual " + stringToken.EndIndex.ToString());
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;

namespace Boo.BooLangStudioSpecs
{

    
    public class WhenParsingMalformedStringTokensWithNoClosingQuotes : LexingBaseFixture
    {
        public WhenParsingMalformedStringTokensWithNoClosingQuotes()
            : base()
        {

        }

        [Spec]
        public void MalformedSingleQuoteTokenShouldBeTreatedAsAStringToken()
        {
            //               0         1         2         3         4
            //               012345678901234567890123456789012345678901
            rawCodeString = "print 'hello this should be a string token";
            Helper(rawCodeString, 6, 41);
            
        }

        [Spec]
        public void MalformedDoubleQuoteTokenShouldBeTreatedAsAStringToken()
        {
            //               0         1         2         3         4
            //               0123456678901234567890123456789012345678901
            rawCodeString = "print \"hello this should be a string token";
            // should be 41.. but the BooScanner.ResolveTokenStartAndEndIndex() puts
            // an extra num in because its expecting the closing quote.. harmless.
            Helper(rawCodeString, 6, 41);

        }

        [Spec]
        public void WellFormedDoubleQuoteTokenShouldBeTreatedAsAStringToken()
        {
            //               0         1         2         3         4
            //               0123456678901234567890123456789012345678901
            rawCodeString = "print \"hello this should be a string token\"";
            Helper(rawCodeString, 6, 42);

        }

        public void Helper(string rawCodeString, int start, int end)
        {
            BuildTokens(rawCodeString);

            List<TokenInfo> stringTokens = new List<TokenInfo>(from i in tokens
                                                               where i.Type == TokenType.String
                                                               select i);
            Assert.True(stringTokens.Count == 1, "Not expected string token count! Expected 1, Actual: " + stringTokens.Count.ToString());
            Assert.True(stringTokens[0].StartIndex == start, "string token start index mismatch! Expected "+start.ToString()+", Actual: " + stringTokens[0].StartIndex.ToString());
            Assert.True(stringTokens[0].EndIndex == end, "string token end index mismatch! Expected "+end.ToString()+", Actual: " + stringTokens[0].EndIndex.ToString());
        }

        [Spec]
        public void MalformedStringWithNoWhiteSpacePaddingBeforeBeginningShouldStillParseAsStringToken()
        {
            //               0          1         2         3         4
            //               0123456678901234567890123456789012345678901
            rawCodeString = "foo = bar +'malformed string";
            Helper(rawCodeString, 10, 25);
        }

    }

}
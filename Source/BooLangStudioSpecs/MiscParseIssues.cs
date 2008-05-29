using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Microsoft.VisualStudio.Package;
using System.IO;
using Context = MbUnit.Framework.TestFixtureAttribute;
using Spec = MbUnit.Framework.TestAttribute;

namespace Boo.BooLangStudioSpecs
{

    [Context]
    public class WhenParsingMalformedStringTokensWithNoClosingQuotes : LexingBaseFixture
    {
        public override void SetUp()
        {
            base.SetUp();

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
            Assert.IsTrue(stringTokens.Count == 1, "Not expected string token count! Expected 1, Actual: " + stringTokens.Count.ToString());
            Assert.IsTrue(stringTokens[0].StartIndex == start, "string token start index mismatch! Expected "+start.ToString()+", Actual: " + stringTokens[0].StartIndex.ToString());
            Assert.IsTrue(stringTokens[0].EndIndex == end, "string token end index mismatch! Expected "+end.ToString()+", Actual: " + stringTokens[0].EndIndex.ToString());
        }

    }

    [Context]
    public class WhenWorkingWithTheInternalParseTracker : LexingBaseFixture
    {
        public override void SetUp()
        {
            base.SetUp();
        }

        [Spec]
        public void InternalParserPositionShouldTrackLexerTokenPositions()
        {
            rawCodeString = "foo = 'bar'";
            scanner.SetSource(rawCodeString,0);
            tokens.Clear();
            bool moreTokens = true;
            while (moreTokens)
            {
                TokenInfo token = new TokenInfo();
                moreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref _mlState);
                Assert.IsTrue(scanner.InternalCurrentLinePosition == token.EndIndex + 1, "Internal track pos mismatch! Expected: "+(token.EndIndex+1).ToString()+" Actual: "+scanner.InternalCurrentLinePosition);
                
                int lengthCount = 0;
                if ((token.EndIndex-token.StartIndex+1) >= rawCodeString.Length)
                    lengthCount = 1;
                else
                    lengthCount = token.EndIndex-token.StartIndex+1;
                if (scanner.InternalCurrentLinePosition <= rawCodeString.Length)
                {
                    Console.WriteLine("Current Token: '" + rawCodeString.Substring(token.StartIndex, lengthCount) + "' type: " + token.Type.ToString());
                    Console.WriteLine("What's left: '" + scanner.RemainingCurrentLine + "'");
                }
            }
        }

    }
}
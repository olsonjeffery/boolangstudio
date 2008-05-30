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

    }

    
    public class WhenWorkingWithTheInternalParseTracker : LexingBaseFixture
    {
        public WhenWorkingWithTheInternalParseTracker()
            : base()
        {
        }

        /// <summary>
        /// Long-test is long.
        /// </summary>
        [Spec]
        public void InternalParserPositionShouldTrackLexerTokenPositions()
        {
            //                         1
            //               01234567890
            rawCodeString = "foo = 'bar'";
            scanner.SetSource(rawCodeString,0);
            tokens.Clear();
            bool moreTokens = true;
            bool lastToken = false;
            while (moreTokens)
            {
                TokenInfo token = new TokenInfo();
                moreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref _mlState);
                
                if (lastToken == true)
                {
                    Assert.True(!moreTokens,"Last token flag set but moreTokens is true! Token Type: "+token.Type.ToString()+ " start: "+token.StartIndex.ToString()+ "end: "+token.EndIndex.ToString()+" line length: "+rawCodeString.Length.ToString());
                    break;
                }
                

                if (scanner.InternalCurrentLinePosition < rawCodeString.Length-1)
                    Assert.True(scanner.InternalCurrentLinePosition == token.EndIndex + 1, "Internal track pos mismatch! Type: "+token.Type.ToString()+" Expected: "+(token.EndIndex+1).ToString()+" Actual: "+scanner.InternalCurrentLinePosition);
                else
                    lastToken = true;

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

    public class WhenParsingStringsThatContainStringInterpolation : LexingBaseFixture
    {

        public WhenParsingStringsThatContainStringInterpolation()
            : base()
        {
        }

        // tests go here

    }
}
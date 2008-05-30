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
            scanner.SetSource(rawCodeString, 0);
            tokens.Clear();
            bool moreTokens = true;
            bool lastToken = false;
            while (moreTokens)
            {
                TokenInfo token = new TokenInfo();
                moreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref _mlState);

                if (lastToken == true)
                {
                    Assert.True(!moreTokens, "Last token flag set but moreTokens is true! Token Type: " + token.Type.ToString() + " start: " + token.StartIndex.ToString() + "end: " + token.EndIndex.ToString() + " line length: " + rawCodeString.Length.ToString());
                    break;
                }


                if (scanner.InternalCurrentLinePosition < rawCodeString.Length - 1)
                    Assert.True(scanner.InternalCurrentLinePosition == token.EndIndex + 1, "Internal track pos mismatch! Type: " + token.Type.ToString() + " Expected: " + (token.EndIndex + 1).ToString() + " Actual: " + scanner.InternalCurrentLinePosition);
                else
                    lastToken = true;

                int lengthCount = 0;
                if ((token.EndIndex - token.StartIndex + 1) >= rawCodeString.Length)
                    lengthCount = 1;
                else
                    lengthCount = token.EndIndex - token.StartIndex + 1;
                if (scanner.InternalCurrentLinePosition <= rawCodeString.Length)
                {
                    Console.WriteLine("Current Token: '" + rawCodeString.Substring(token.StartIndex, lengthCount) + "' type: " + token.Type.ToString());
                    Console.WriteLine("What's left: '" + scanner.RemainingCurrentLine + "'");
                }
            }
        }

    }
}

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

    // yes, i know.. DRY... these should be refactored
    // into one class with some combinatorial goodness...
    //
    // I can't wait to switch back to writing tests
    // in boo

    
    public class WhenParsingCodeStringsWithDoubleWhackLineComments : LexingBaseFixture
    {
        TokenInfo lineCommentToken;
        
        public WhenParsingCodeStringsWithDoubleWhackLineComments() :
            base()
        {
            //               0         1         2         3         4
            //               012345678901234567890123456789012345678901
            rawCodeString = "someMethodCall('a string') // line Comment";
            BuildTokens(rawCodeString);
            List<TokenInfo> commentTokens = new List<TokenInfo>(from i in tokens
                                                    where i.Type == TokenType.Comment
                                                    select i);
            Assert.True(commentTokens.Count == 1);
            lineCommentToken = commentTokens[0];
        }

        [Spec]
        public void TheLineCommentTokenShouldStartAtTheProperPlaceAndEndAtEOL()
        {
            Assert.True(lineCommentToken.StartIndex == 27, "Expected start at 27, actual " + lineCommentToken.StartIndex.ToString());
            Assert.True(lineCommentToken.EndIndex == 41, "expected line comment to end at 41, actual " + lineCommentToken.EndIndex.ToString());
        }

        [Spec]
        public void LastValidTokenShouldBeTheCommentToken()
        {
            Assert.True(tokens[tokens.Count - 1].Type == TokenType.Comment, "Not expected tokentype: " + tokens[tokens.Count - 1].Type.ToString());
            
        }

        [Spec]
        public void ShouldParseAttemptedDoubleLineCommentAsOneLineComment()
        {
            //               0         1         2         3         4         5         6
            //               01234567890123456789012345678901234567890123456789012345678901234
            rawCodeString = "someMethodCall('a string') // line Comment // double line comment";
            BuildTokens(rawCodeString);
            List<TokenInfo> commentTokens = new List<TokenInfo>(from i in tokens
                                                                where i.Type == TokenType.Comment
                                                                select i);
            Assert.True(commentTokens.Count == 1, "Expected 1, actual: "+commentTokens.Count.ToString());
            //Assert.Fail("comment 1's start: "+commentTokens[0].StartIndex.ToString()+" comment 2's start: "+commentTokens[1].StartIndex.ToString());
            lineCommentToken = commentTokens[0];
        }
    }


    
    public class WhenParsingCodeStringsWithPoundSignLineComments : LexingBaseFixture
    {
        TokenInfo lineCommentToken;

        public WhenParsingCodeStringsWithPoundSignLineComments()
            : base()
        {
            //               0         1         2         3         4
            //               012345678901234567890123456789012345678901
            rawCodeString = "someMethodCall('a string') # line Comment";
            BuildTokens(rawCodeString);
            List<TokenInfo> commentTokens = new List<TokenInfo>(from i in tokens
                                                                where i.Type == TokenType.Comment
                                                                select i);
            Assert.True(commentTokens.Count == 1);
            lineCommentToken = commentTokens[0];
        }

        [Spec]
        public void TheLineCommentTokenShouldStartAtTheProperPlaceAndEndAtEOL()
        {
            Assert.True(lineCommentToken.StartIndex == 27, "Expected start at 27, actual " + lineCommentToken.StartIndex.ToString());
            Assert.True(lineCommentToken.EndIndex == 40, "expected line comment to end at 41, actual " + lineCommentToken.EndIndex.ToString());
        }

        [Spec]
        public void LastValidTokenShouldBeTheCommentToken()
        {
            Assert.True(tokens[tokens.Count - 1].Type == TokenType.Comment, "Not expected tokentype: " + tokens[tokens.Count - 1].Type.ToString());

        }

        [Spec]
        public void ShouldParseAttemptedDoubleLineCommentAsOneLineComment()
        {
            //               0         1         2         3         4         5         6
            //               01234567890123456789012345678901234567890123456789012345678901234
            rawCodeString = "someMethodCall('a string') # line Comment # double line comment";
            BuildTokens(rawCodeString);
            List<TokenInfo> commentTokens = new List<TokenInfo>(from i in tokens
                                                                where i.Type == TokenType.Comment
                                                                select i);
            Assert.True(commentTokens.Count == 1, "Expected 1, actual: " + commentTokens.Count.ToString());
            //Assert.Fail("comment 1's start: "+commentTokens[0].StartIndex.ToString()+" comment 2's start: "+commentTokens[1].StartIndex.ToString());
            lineCommentToken = commentTokens[0];
        }
    }

}
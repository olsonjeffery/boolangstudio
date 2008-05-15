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
    public class WhenParsingABasicMultiLineCommentInMultipleLinesOfCode : LexingBaseFixture
    {
        private List<string> rawLines = new List<string>();
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            rawLines = new List<string>();
            //            0         1         2
            //            012345678901234567890123456789
            rawLines.Add("class SomeClassName:"); // 0
            rawLines.Add("  def constructor():"); // 1
            rawLines.Add("    pass"); // 2
            rawLines.Add("  def HelloWorld():"); // 3
            rawLines.Add("    someVal = 1"); // 4
            //            0         1         2
            //            012345678901234567890123456789
            rawLines.Add("    /* start of multi-liner"); // 5
            rawLines.Add("    some comments go here"); // 6
            //            0         1         2
            //            012345678901234567890123456789
            rawLines.Add("    and we are done */"); // 7
            rawLines.Add("    aStatement()"); // 8

        }

        [Spec]
        public void EverythingInTheMLCommentRegionShouldParseAsAMLComment()
        {
            // reset any _mlState weirdness
            _mlState = 0;
            BuildTokens(rawLines[5]);
            foreach (TokenInfo token in tokens)
            {
                Console.WriteLine("start: " + token.StartIndex + " end: " + token.EndIndex + " type: " + token.Type);
                Assert.IsTrue(token.Type == TokenType.Comment || token.Type == TokenType.WhiteSpace, "Unexpected tokentype in line 5 (Comment/Whitespace): " + token.Type.ToString());
            }

            // we should now be in a ml comment region
            Assert.IsTrue(_mlState == 13, "unexpected _mlState value (expected 13), actual: " + _mlState.ToString());

            Console.WriteLine("Line 6");
            BuildTokens(rawLines[6]);
            // every token should still be a ml_comment
            foreach (TokenInfo token in tokens)
            {
                Console.WriteLine("start: " + token.StartIndex + " end: " + token.EndIndex + " type: " + token.Type);

                Assert.IsTrue(token.Type == TokenType.WhiteSpace || token.Type == TokenType.Comment, "unexpected tokentype in line 6 (Comment/Whitespace): " + token.Type.ToString());
            }
            // should STILL be in _mlState == 13
            Assert.IsTrue(_mlState == 13, "unexpected _mlState value (expected 13), actual: " + _mlState.ToString());

            Console.WriteLine("Line 7");
            // now to process line 7
            BuildTokens(rawLines[7]);
            foreach (TokenInfo token in tokens)
            {
                Console.WriteLine("start: " + token.StartIndex + " end: " + token.EndIndex + " type: " + token.Type);

                Assert.IsTrue(token.Type == TokenType.WhiteSpace || token.Type == TokenType.Comment, "unexpected tokentype in line 7 (Comment/Whitespace): " + token.Type.ToString());
            }

            // NOW we should be back to _mlState == 0
            Assert.IsTrue(_mlState == 0, "unexpected _mlState.. expected 0, actual " + _mlState.ToString());

            BuildTokens(rawLines[8]);
            foreach (TokenInfo token in tokens)
            {
                Assert.IsTrue(token.Type == TokenType.Identifier || token.Type == TokenType.Delimiter || token.Type == TokenType.WhiteSpace, "unexpected tokentype at " + token.StartIndex.ToString() + "," + token.EndIndex.ToString() + " : " + token.Type.ToString());
            }
        }
    }

            [Context]
    public class WhenParsingAMultiLineCommentWithEmbeededQuotesInMultipleLinesOfCode : LexingBaseFixture
    {
        private List<string> rawLines = new List<string>();
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            rawLines = new List<string>();
            //            0         1         2
            //            012345678901234567890123456789
            rawLines.Add("class SomeClassName:"); // 0
            rawLines.Add("  def constructor():"); // 1
            rawLines.Add("    pass"); // 2
            rawLines.Add("  def HelloWorld():"); // 3
            rawLines.Add("    someVal = 1"); // 4
            //            0         1         2
            //            012345678901234567890123456789
            rawLines.Add("    /* start of multi-liner"); // 5
            rawLines.Add("    some \"comments\" go here"); // 6
            //            0         1         2
            //            012345678901234567890123456789
            rawLines.Add("    and we are done */"); // 7
            rawLines.Add("    aStatement()"); // 8
            
        }

        [Spec]
        public void EverythingInTheMLCommentRegionShouldParseAsAMLComment()
        {
            // reset any _mlState weirdness
            _mlState = 0;
            BuildTokens(rawLines[5]);
            foreach (TokenInfo token in tokens)
            {
                Console.WriteLine("start: " + token.StartIndex + " end: " + token.EndIndex + " type: " + token.Type);
                Assert.IsTrue(token.Type == TokenType.Comment || token.Type == TokenType.WhiteSpace, "Unexpected tokentype in line 5 (Comment/Whitespace): " + token.Type.ToString());
            }

            // we should now be in a ml comment region
            Assert.IsTrue(_mlState == 13, "unexpected _mlState value (expected 13), actual: " + _mlState.ToString());

            Console.WriteLine("Line 6");
            BuildTokens(rawLines[6]);
            // every token should still be a ml_comment
            foreach (TokenInfo token in tokens)
            {
                Console.WriteLine("start: " + token.StartIndex + " end: " + token.EndIndex + " type: " + token.Type);
                
                Assert.IsTrue(token.Type == TokenType.WhiteSpace || token.Type == TokenType.Comment, "unexpected tokentype in line 6 (Comment/Whitespace): " + token.Type.ToString());
            }
            // should STILL be in _mlState == 13
            Assert.IsTrue(_mlState == 13, "unexpected _mlState value (expected 13), actual: " + _mlState.ToString());

            Console.WriteLine("Line 7");
            // now to process line 7
            BuildTokens(rawLines[7]);
            foreach (TokenInfo token in tokens)
            {
                Console.WriteLine("start: " + token.StartIndex + " end: " + token.EndIndex + " type: " + token.Type);
                
                Assert.IsTrue(token.Type == TokenType.WhiteSpace || token.Type == TokenType.Comment, "unexpected tokentype in line 7 (Comment/Whitespace): " + token.Type.ToString());
            }

            // NOW we should be back to _mlState == 0
            Assert.IsTrue(_mlState == 0, "unexpected _mlState.. expected 0, actual " + _mlState.ToString());

            BuildTokens(rawLines[8]);
            foreach (TokenInfo token in tokens)
            {
                Assert.IsTrue(token.Type == TokenType.Identifier || token.Type == TokenType.Delimiter || token.Type == TokenType.WhiteSpace, "unexpected tokentype at " + token.StartIndex.ToString() + "," + token.EndIndex.ToString() + " : " + token.Type.ToString());
            }
        }
    }
}
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
  
      
    public abstract class StringLexingContext : LexingBaseFixture
    {

        protected TokenInfo trimmed;
        protected void BuildTrimmed(TokenType type)
        {
            List<TokenInfo> trimList = new List<TokenInfo>(from i in tokens
                                                           where i.Type == type
                                                           select i);
            Assert.True(trimList.Count == 1, "Expected 1, actual " + trimList.Count.ToString());
            trimmed = trimList[0];
        }
    }

    
    public class WhenParsingSingleQuotes : StringLexingContext
    {
        
        public WhenParsingSingleQuotes()
            :base()
        {
            //                0         1         2         3
            //                012345678901234567890123456789012345
            rawCodeString = @"someMethodClass('aStringParameter')";
            // run this guy last
            BuildTokens(rawCodeString);
            
        }


        [Spec]
        public void TokenShouldStartAndEndAtAppropIndex()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.StartIndex == 16, "Failed start index check. Expected 16, Actual " + trimmed.StartIndex.ToString());
            Assert.True(trimmed.EndIndex == 33, "Failed end index check. Expected 33, Actual " + trimmed.EndIndex.ToString());
        }
        [Spec]
        public void TokenShouldHaveTokenTypeOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.Type == TokenType.String);
        }
        [Spec]
        public void TokenShouldHaveTokenColorOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.Color == TokenColor.String);
        }

        [Spec]
        public void PreceedingTokenShouldNotStepOnStringsColorizingInfo()
        {
            List<TokenInfo> parenTokens = new List<TokenInfo>(from i in tokens
                                                              where i.Type == TokenType.Delimiter
                                                              select i);
            Assert.True(parenTokens.Count == 2, "Actual paren count: "+parenTokens.Count.ToString());
            Assert.True(parenTokens[0].StartIndex == 15, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.True(parenTokens[0].EndIndex == 15, "LPAREN end Actual " + parenTokens[0].EndIndex);
            Assert.True(parenTokens[1].StartIndex == 34, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.True(parenTokens[1].EndIndex == 34, "LPAREN end Actual " + parenTokens[0].EndIndex);

        }
    }

    /// <summary>
    /// These tests are repetitive of the ones above.. should merge into a 
    /// single set of data-driven tests
    /// </summary>
    
    public class WhenParsingDoubleQuotes : StringLexingContext
    {
        public WhenParsingDoubleQuotes()
            : base()
        {
            //               0         1         2         3
            //               01234567890123456678901234567890123345
            rawCodeString = "someMethodClass(\"aStringParameter\")";
            // run this guy last
            BuildTokens(rawCodeString);

        }


        [Spec]
        public void TokenShouldStartAndEndAtAppropIndex()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.StartIndex == 16, "Failed start index check. Expected 16, Actual " + trimmed.StartIndex.ToString());
            Assert.True(trimmed.EndIndex == 33, "Failed end index check. Expected 33, Actual " + trimmed.EndIndex.ToString());
        }
        [Spec]
        public void TokenShouldHaveTokenTypeOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.Type == TokenType.String);
        }
        [Spec]
        public void TokenShouldHaveTokenColorOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.Color == TokenColor.String);
        }

        [Spec]
        public void PreceedingTokenShouldNotStepOnStringsColorizingInfo()
        {
            List<TokenInfo> parenTokens = new List<TokenInfo>(from i in tokens
                                                              where i.Type == TokenType.Delimiter
                                                              select i);
            Assert.True(parenTokens.Count == 2, "Expected 2, actual paren count: "+parenTokens.Count.ToString());
            Assert.True(parenTokens[0].StartIndex == 15, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.True(parenTokens[0].EndIndex == 15, "LPAREN end Actual " + parenTokens[0].EndIndex);
            Assert.True(parenTokens[1].StartIndex == 34, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.True(parenTokens[1].EndIndex == 34, "LPAREN end Actual " + parenTokens[0].EndIndex);

        }

    }

    /// <summary>
    /// also repetitive
    /// </summary>
    
    public class WhenParsingTripleQuotes : StringLexingContext
    {

        public WhenParsingTripleQuotes()
            : base()
        {
            //               0         1            2
            //               012345678901122334567890123456677889
            rawCodeString = "methodCall(\"\"\"tripleQuotes\"\"\")";
            BuildTokens(rawCodeString);
        }

        [Spec]
        public void TokenShouldStartAndEndAtAppropIndex()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.StartIndex == 11, "Failed start index check. Expected 16, Actual " + trimmed.StartIndex.ToString());
            Assert.True(trimmed.EndIndex == 28, "Failed end index check. Expected 33, Actual " + trimmed.EndIndex.ToString());
        }
        [Spec]
        public void TokenShouldHaveTokenTypeOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.Type == TokenType.String);
        }
        [Spec]
        public void TokenShouldHaveTokenColorOfString()
        {
            BuildTrimmed(TokenType.String);
            Assert.True(trimmed.Color == TokenColor.String);
        }

        [Spec]
        public void PreceedingTokenShouldNotStepOnStringsColorizingInfo()
        {
            List<TokenInfo> parenTokens = new List<TokenInfo>(from i in tokens
                                                              where i.Type == TokenType.Delimiter
                                                              select i);
            Assert.True(parenTokens.Count == 2);
            Assert.True(parenTokens[0].StartIndex == 10, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.True(parenTokens[0].EndIndex == 10, "LPAREN end Actual " + parenTokens[0].EndIndex);
            Assert.True(parenTokens[1].StartIndex == 29, "LPAREN start Actual " + parenTokens[0].StartIndex);
            Assert.True(parenTokens[1].EndIndex == 29, "LPAREN end Actual " + parenTokens[0].EndIndex);

        }

    }

    public class WhenParsingEmptyLinesConsistingOfOnlyWhitespace : LexingBaseFixture
    {
        public WhenParsingEmptyLinesConsistingOfOnlyWhitespace()
            : base()
        {

        }

        [Fact]
        public void ShouldNotBarfOnLineOfOnlyTabs()
        {
            rawCodeString = "\t\t\t";
            BuildTokens(rawCodeString);
            Assert.True(tokens.Count == 1, "Token count mismatch.. expected 1, actual: " + tokens.Count.ToString());
        }

        [Fact]
        public void ShouldNotBarfOnLineOfOnlySpaces()
        {
            rawCodeString = "    ";
            BuildTokens(rawCodeString);
            Assert.True(tokens.Count == 1, "Token count mismatch.. expected 1, actual: " + tokens.Count.ToString());
        }


    }
  
}

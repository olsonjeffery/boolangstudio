/*
 * Created by SharpDevelop.
 * User: jeff_2
 * Date: 6/8/2008
 * Time: 6:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using Xunit;
using Boo.BooLangService;
using Microsoft.VisualStudio.Package;
using BooPegLexer;

namespace Boo.BooLangStudioSpecs
{
	/// <summary>
	/// Description of WhenSettingTheSourceInBooScanner.
	/// </summary>
	public abstract class WhenProcessingALineWithBooScanner : AutoMockingTestFixture
	{
		protected BooScanner scanner;
		//                       0         1
		//                       012345678901234
		protected string line = "  print 'hello'";
		protected int offset = 0;
		protected PegLexer lexer;
		
		public WhenProcessingALineWithBooScanner()
			: base()
		{
			lexer = new PegLexer();
			scanner = new BooScanner(lexer);
			
			scanner.SetSource(line,offset);

		}
	}
	
	public class TokenProcessingTestFixture : WhenProcessingALineWithBooScanner
	{
	  protected int lexerPos = 0;
		protected bool isMoreTokens = false;
		protected TokenInfo token = new TokenInfo();
		protected int state = 0;
		protected string remainingLine = string.Empty;
		
		public TokenProcessingTestFixture()
			: base()
		{
		  
		}
	}
	
	public class AndWhenTheScannerHasProcessedTheFirstToken : TokenProcessingTestFixture
	{
		
		public AndWhenTheScannerHasProcessedTheFirstToken()
			: base()
		{
		  lexerPos = 2;
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token,ref state);
			remainingLine = line.Substring(lexerPos);
		}
		
		[Fact]
		public void StateShouldBeZero()
		{
			Assert.True(state == 0, "Actual: "+state.ToString());
		}
		
		[Fact]
		public void TokenTypeShouldBeWhiteSpace()
		{
			Assert.True(token.Type == TokenType.WhiteSpace,"Actual: "+token.Type.ToString());
		}
		
		[Fact]
		public void TokenColorShouldBeText()
		{
			Assert.True(token.Color == TokenColor.Text, "Actual: "+token.Color.ToString());
		}
		
		[Fact]
		public void TokenStartIndexShouldBeZero()
		{
			Assert.True(token.StartIndex == 0, "Actual: "+token.StartIndex.ToString());
		}
		
		[Fact]
		public void TokenEndIndexShouldBeOne()
		{
			Assert.True(token.EndIndex == 1, "Actual: "+token.EndIndex.ToString());
		}
		
		[Fact]
		public void IsMoreTokensValueShouldBeTrue()
		{
			Assert.True(isMoreTokens, "Actual: "+isMoreTokens.ToString());
		}
		
		[Fact] void TheLexersCurrentIndexShouldBeTwo()
		{
		  Assert.True(scanner.Lexer.CurrentIndex == 2, "Actual: "+scanner.Lexer.CurrentIndex);
		}
	}
	
	public class AndWhenTheScannerHasProcessedTheSecondToken : TokenProcessingTestFixture
	{
		
		public AndWhenTheScannerHasProcessedTheSecondToken()
			: base()
		{
		  lexerPos = 2;
			// first token
		  isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token,ref state);
			// second token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			remainingLine = line.Substring(lexerPos);
		}
		
    [Fact]
		public void StateShouldBeZero()
		{
			Assert.True(state == 0, "Actual: "+state.ToString());
		}
		
		[Fact]
		public void TokenTypeShouldBeKeyword()
		{
			Assert.True(token.Type == TokenType.Keyword,"Actual: "+token.Type.ToString());
		}
		
		[Fact]
		public void TokenColorShouldBeKeyword()
		{
			Assert.True(token.Color == TokenColor.Keyword, "Actual: "+token.Color.ToString());
		}
		
		[Fact]
		public void TokenStartIndexShouldBeTwo()
		{
			Assert.True(token.StartIndex == 2, "Actual: "+token.StartIndex.ToString());
		}
		
		[Fact]
		public void TokenEndIndexShouldBeSix()
		{
			Assert.True(token.EndIndex == 6, "Actual: "+token.EndIndex.ToString());
		}
		
		[Fact]
		public void IsMoreTokensValueShouldBeTrue()
		{
			Assert.True(isMoreTokens, "Actual: "+isMoreTokens.ToString());
		}
		
		[Fact] void TheLexersCurrentIndexShouldBeSeven()
		{
		  Assert.True(scanner.Lexer.CurrentIndex == 7, "Actual: "+scanner.Lexer.CurrentIndex);
		}
	}
	
	public class AndWhenTheScannerHasProcessedTheThirdToken : TokenProcessingTestFixture
	{
		
		public AndWhenTheScannerHasProcessedTheThirdToken()
			: base()
		{
		  lexerPos = 7;
			// first token
		  isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token,ref state);
			// second token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			// third token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			remainingLine = line.Substring(lexerPos);
		}
		
    [Fact]
		public void StateShouldBeZero()
		{
			Assert.True(state == 0, "Actual: "+state.ToString());
		}
		
		[Fact]
		public void TokenTypeShouldBeWhitespace()
		{
			Assert.True(token.Type == TokenType.WhiteSpace,"Actual: "+token.Type.ToString());
		}
		
		[Fact]
		public void TokenColorShouldBeText()
		{
			Assert.True(token.Color == TokenColor.Text, "Actual: "+token.Color.ToString());
		}
		
		[Fact]
		public void TokenStartIndexShouldBeSeven()
		{
			Assert.True(token.StartIndex == 7, "Actual: "+token.StartIndex.ToString());
		}
		
		[Fact]
		public void TokenEndIndexShouldBeSeven()
		{
			Assert.True(token.EndIndex == 7, "Actual: "+token.EndIndex.ToString());
		}
		
		[Fact]
		public void IsMoreTokensValueShouldBeTrue()
		{
			Assert.True(isMoreTokens, "Actual: "+isMoreTokens.ToString());
		}
		
		[Fact] void TheLexersCurrentIndexShouldBeEight()
		{
		  Assert.True(scanner.Lexer.CurrentIndex == 8, "Actual: "+scanner.Lexer.CurrentIndex);
		}
	}
	
	public class AndWhenTheScannerHasProcessedTheFourthToken : TokenProcessingTestFixture
	{
		
		public AndWhenTheScannerHasProcessedTheFourthToken()
			: base()
		{
		  lexerPos = 15;
			// first token
		  isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token,ref state);
			// second token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			// third token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			// fourth token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			remainingLine = string.Empty;
		}
		
    [Fact]
		public void StateShouldBeZero()
		{
			Assert.True(state == 0, "Actual: "+state.ToString());
		}
		
		[Fact]
		public void TokenTypeShouldBeString()
		{
			Assert.True(token.Type == TokenType.String,"Actual: "+token.Type.ToString());
		}
		
		[Fact]
		public void TokenColorShouldBeString()
		{
			Assert.True(token.Color == TokenColor.String, "Actual: "+token.Color.ToString());
		}
		
		[Fact]
		public void TokenStartIndexShouldBeEight()
		{
			Assert.True(token.StartIndex == 8, "Actual: "+token.StartIndex.ToString());
		}
		
		[Fact]
		public void TokenEndIndexShouldBeFourteen()
		{
			Assert.True(token.EndIndex == 14, "Actual: "+token.EndIndex.ToString());
		}
		
		[Fact]
		public void IsMoreTokensValueShouldBeTrue()
		{
			Assert.True(isMoreTokens, "Actual: "+isMoreTokens.ToString());
		}
		
		[Fact] void TheLexersCurrentIndexShouldBeFifteen()
		{
		  Assert.True(scanner.Lexer.CurrentIndex == 15, "Actual: "+scanner.Lexer.CurrentIndex);
		}
	}
	
	public class AndWhenTheScannerHasProcessedTheFifthToken : TokenProcessingTestFixture
	{
		
		public AndWhenTheScannerHasProcessedTheFifthToken()
			: base()
		{
		  lexerPos = 15;
			// first token
		  isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token,ref state);
			// second token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			// third token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			// fourth token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			// fifth token
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
			remainingLine = string.Empty;
		}
		
    [Fact]
		public void StateShouldBeZero()
		{
			Assert.True(state == 0, "Actual: "+state.ToString());
		}
		
		[Fact]
		public void IsMoreTokensValueShouldBeFalse()
		{
			Assert.True(isMoreTokens == false, "Actual: "+isMoreTokens.ToString());
		}
		
		[Fact] void TheLexersCurrentIndexShouldBeFifteen()
		{
		  Assert.True(scanner.Lexer.CurrentIndex == 15, "Actual: "+scanner.Lexer.CurrentIndex);
		}
	}
	
}

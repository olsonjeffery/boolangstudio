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

namespace Boo.BooLangStudioSpecs
{
	/// <summary>
	/// Description of WhenSettingTheSourceInBooScanner.
	/// </summary>
	public abstract class WhenSettingTheSourceInBooScanner : AutoMockingTestFixture
	{
		protected BooScanner scanner;
		public WhenSettingTheSourceInBooScanner()
			: base()
		{
			scanner = new BooScanner();
		}
	}
	
	public class AndWhenTheOffsetIsZero : WhenSettingTheSourceInBooScanner
	{
		protected string line = "\t\tprint 'hello'";
		protected int offset = 0;
		
		public AndWhenTheOffsetIsZero()
			: base()
		{
			
			scanner.SetSource(line,offset);                  
		}
		
		[Fact]
		public void OffsetShouldBeZero()
		{
			Assert.True(scanner.Offset == offset, "Actual: "+scanner.Offset);
		}
		
		[Fact]
		public void LinePropertyShouldReturnEntireLine()
		{
			Assert.True(scanner.Line == line, "Actual: "+scanner.Line);
		}
		
		[Fact]
		public void RawLinePropertyShouldReturnEntireLine()
		{
			Assert.True(scanner.RawLine == line, "Actual: "+scanner.RawLine);
		}
		
		[Fact]
		public void LexerPositionShouldBeZero()
		{
			Assert.True(scanner.LexerIndex == 0, "Actual: "+scanner.LexerIndex.ToString());
		}
	}
	
	public class AndWhenTheLexerHasConsumedOneToken : AndWhenTheOffsetIsZero
	{
		protected int lexerPos = 2;
		protected bool isMoreTokens = false;
		protected TokenInfo token = new TokenInfo();
		protected int state = 0;
		protected string remainingLine = string.Empty;
		
		public AndWhenTheLexerHasConsumedOneToken()
			: base()
		{
			isMoreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token,ref state);
			remainingLine = line.Substring(lexerPos);
		}
		
		[Fact]
		public void StateShouldBeZero()
		{
			Assert.True(state == 0, "Actual: "+state.ToString());
		}
		
		[Fact]
		public void LexerIndexShouldBeTwo()
		{
			Assert.True(scanner.LexerIndex == lexerPos,"Actual: "+scanner.LexerIndex.ToString());
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
		public void RemainingLinePropertyShouldBeProper()
		{
			Assert.True(scanner.RemainingLine == remainingLine, "Expected: '"+remainingLine+"', Actual: '"+scanner.RemainingLine+"'");
		}
		
		[Fact]
		public void IsMoreTokensValueShouldBeTrue()
		{
			Assert.True(isMoreTokens, "Actual: "+isMoreTokens.ToString());
		}
	}
}

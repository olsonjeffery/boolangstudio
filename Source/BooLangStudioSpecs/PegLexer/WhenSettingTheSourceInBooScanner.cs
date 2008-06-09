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
	public abstract class WhenSettingTheSourceInBooScanner : AutoMockingTestFixture
	{
		protected BooScanner scanner;
		protected string line = "\t\tprint 'hello'";
		protected int offset = 0;
		protected PegLexer lexer;
		
		public WhenSettingTheSourceInBooScanner()
			: base()
		{
			lexer = Mocks.PartialMock<PegLexer>();
			scanner = new BooScanner(lexer);
			
			scanner.SetSource(line,offset);

		}
	}
	
	public class AndWhenTheScannerHasProcessedOneToken : WhenSettingTheSourceInBooScanner
	{
		protected int lexerPos = 2;
		protected bool isMoreTokens = false;
		protected TokenInfo token = new TokenInfo();
		protected int state = 0;
		protected string remainingLine = string.Empty;
		
		public AndWhenTheScannerHasProcessedOneToken()
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
	}
}

using System;
using System.IO;
using BooColorizerParser;
using Xunit;
using Boo.BooLangService;
using Microsoft.VisualStudio.Package;


namespace Boo.BooLangStudioSpecs
{
	public abstract class WhenTranslatingTokens
	{
		protected BooScanner scanner;
		protected ColorizerToken pegToken;
		protected TokenInfo ideToken;
		public WhenTranslatingTokens()
			:base()
		{
			pegToken = new ColorizerToken();
			ideToken = new TokenInfo();
			scanner = new BooScanner();
		}
	}
	
	public class AndPegTokenTypeIsWhitespace : WhenTranslatingTokens
	{
		public AndPegTokenTypeIsWhitespace()
			: base()
		{
		  pegToken = new ColorizerToken(PegTokenType.Whitespace, 0, 1);
	    scanner.TranslatePegToken(pegToken,ideToken);
		}
		
		[Fact]
		public void IdeTokenStartIndexShouldBeZero()
		{
			Assert.True(ideToken.StartIndex == pegToken.StartIndex,"Actual: "+ideToken.StartIndex.ToString());
		}
		
		[Fact]
		public void IdeTokenEndIndexShouldBeOne()
		{
			Assert.True(ideToken.EndIndex == pegToken.EndIndex, "Actual: "+ideToken.EndIndex.ToString());
		}
		
		[Fact]
		public void IdeTokenTypeShouldBeWhitespace()
		{
			Assert.True(ideToken.Type == TokenType.WhiteSpace, "Actual: "+ideToken.Type.ToString());
		}
		
		[Fact]
		public void IdeTokenColorShouldBeText()
		{
			Assert.True(ideToken.Color == TokenColor.Text, "Actual: "+ideToken.Color.ToString());
		}
	}
}
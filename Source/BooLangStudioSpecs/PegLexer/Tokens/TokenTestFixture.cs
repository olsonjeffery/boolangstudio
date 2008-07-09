using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;
using Rhino.Mocks;
using Rhino.Testing.AutoMocking;
using BooPegLexer;
using Boo.BooLangService;

namespace Boo.BooLangStudioSpecs
{
  public abstract class AutoTokenTestFixture : AutoMockingTestFixture
  {
    protected BooScanner scanner;
    
    protected List<TokenInfo> tokens = new List<TokenInfo>();
    
    protected string line = string.Empty;
    protected int offset = 0;
    
    protected TokenType expectedTokenType;
    protected TokenColor expectedTokenColor;
    protected int expectedStartIndex;
    protected int expectedEndIndex;
    
    public AutoTokenTestFixture()
      : base()
    {
    	scanner = new BooScanner(new PegLexer());
    }
    
    public virtual void BuildTokens(string codeLine, int startOffset)
    {
      bool moreTokens = true;
      int state = 0;
      scanner.SetSource(codeLine, startOffset);
      while(moreTokens)
      {
        TokenInfo token = new TokenInfo();
        moreTokens = scanner.ScanTokenAndProvideInfoAboutIt(token, ref state);
        if (moreTokens)
          // no EOL token, since the IDE doesn't care about it anywho
          tokens.Add(token);
      }
    }
    
    [Fact]
    public void ShouldOnlyBeOneToken()
    {
      Assert.True(tokens.Count == 1, "Expected: 1 Actual: "+tokens.Count.ToString());
    }
    
    [Fact]
    public void VerifyTokenType()
    {
    	Assert.True(tokens[0].Type == expectedTokenType, "Expected: "+expectedTokenType.ToString()+" Actual: "+tokens[0].Type.ToString());
    }
    
    [Fact]
    public void VerifyTokenColor()
    {
    	Assert.True(tokens[0].Color == expectedTokenColor , "Expected: "+expectedTokenColor.ToString()+" Actual: "+tokens[0].Type.ToString());
    }
    
    [Fact]
    public void VerifyStartIndex()
    {
      Assert.True(tokens[0].StartIndex == expectedStartIndex, "Expected: "+expectedStartIndex.ToString()+" Actual: "+tokens[0].StartIndex.ToString());
    }
    
    [Fact]
    public void VerifyEndIndex()
    {
      Assert.True(tokens[0].EndIndex == expectedEndIndex, "Expected: "+expectedEndIndex.ToString()+" Actual: "+tokens[0].EndIndex.ToString());
    }
    
    
  }

  public class ManualTokenTestFixture : AutoMockingTestFixture
  {

      protected PegLexer lexer;
      protected TokenType expectedTokenType;
      protected TokenColor expectedTokenColor;
      protected int expectedStartIndex;
      protected int expectedEndIndex;
      protected int state;
      protected BooScanner scanner;
      protected TokenInfo token;
      protected bool result;
      protected string line;
      protected int offset;

      public ManualTokenTestFixture()
          : base()
      {
          //             0         1         2         3   
          //             012345678901234567890123456789012345678
          line = string.Empty;
          offset = 0;

          expectedTokenType = TokenType.Unknown;
          expectedTokenColor = TokenColor.Text;
          expectedStartIndex = 0;
          expectedEndIndex = 0;
          state = 0;

          lexer = new PegLexer();
          scanner = new BooScanner(lexer);
          token = new TokenInfo();
      }
  }
}
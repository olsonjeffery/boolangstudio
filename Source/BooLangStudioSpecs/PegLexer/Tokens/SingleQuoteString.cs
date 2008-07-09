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
  
  public class TokenBasicSingleQuoteString : AutoTokenTestFixture
  {
    public TokenBasicSingleQuoteString()
      : base()
    {
      //      0         1
      //      012345678901234567
      line = "'this is a string'";
      offset = 0;
      
      expectedTokenType = TokenType.String;
      expectedTokenColor = TokenColor.String;
      expectedStartIndex = 0;
      expectedEndIndex = 17;
      
      BuildTokens(line, offset);
    }
    
  }
  
  public class TokenSingleQuoteStringWithEscapedDelimiter : AutoTokenTestFixture
  {
  	public TokenSingleQuoteStringWithEscapedDelimiter()
  		: base()
  	{
      //      0          1
      //      0122345678901234567
      line = @"'i\'m a string'";
      offset = 0;
      
      expectedTokenType = TokenType.String;
      expectedTokenColor = TokenColor.String;
      expectedStartIndex = 0;
      expectedEndIndex = 14;
      
      BuildTokens(line, offset);
  	}

  }
  
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Package;
using Boo.Lang.Parser;
using Microsoft.VisualStudio.TextManager.Interop;
using BooPegLexer;

namespace Boo.BooLangService
{
    public partial class BooScanner : IScanner
    {
        #region properties and members
        private PegLexer _lexer = null;
		public PegLexer Lexer
		{
			get
			{
				return _lexer;
			}
		}
        #endregion
		
        /// <summary>
        /// default ctor... news up a lexer
        /// </summary>
        public BooScanner()
        {
        	_lexer = new PegLexer();
        	_lexer.InitializeAndBindPegs(_lexer.GetDefaultKeywordList(),new string[] { });
        }
		
        /// <summary>
        /// ctor that takes a lexer as an argument.. assume the lexer has
        /// already had it's PEGs bound...
        /// </summary>
        /// <param name="buffer"></param>
        public BooScanner(PegLexer lexer)
        {
        	_lexer = lexer;
        	_lexer.InitializeAndBindPegs(lexer.GetDefaultKeywordList(), lexer.GetDefaultMacroList());
        }
        
        public BooScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer) 
            : this()
        {
            //
        }

        #region lexer/colorizing-related
		
        public void TranslatePegToken(PegToken token, TokenInfo ideToken)
        {
        	// setting ide token and coloring info
        	switch(token.Type)
        	{
        		case PegTokenType.Whitespace:
        	    ideToken.Type = TokenType.WhiteSpace;
        			ideToken.Color = TokenColor.Text;
              break;
            case PegTokenType.Keyword:
              ideToken.Type = TokenType.Keyword;
              ideToken.Color = TokenColor.Keyword;
              break;
            case PegTokenType.Macro:
              ideToken.Type = TokenType.Keyword;
              ideToken.Color = TokenColor.Keyword;
              break;
            case PegTokenType.SingleQuoteString:
              ideToken.Type = TokenType.String;
              ideToken.Color = TokenColor.String;
              break;
        		default:
              ideToken.Type = TokenType.Unknown;
        			ideToken.Color = TokenColor.Text;
        			break;
        	}
        	ideToken.StartIndex = token.StartIndex;
        	ideToken.EndIndex = token.EndIndex;
        }
        
        PegToken pegToken = new PegToken();
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
        	_lexer.NextToken(pegToken,ref state);
        	if (pegToken.Type == PegTokenType.EOL)
        		return false;
        	TranslatePegToken(pegToken, tokenInfo);
        	
        	return true;
        }

        public  void SetSource(string source, int offset)
        {
        	_lexer.SetSource(source.Substring(offset));
        }
        
        #endregion

    }

}

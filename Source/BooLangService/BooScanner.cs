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
        private ILexer __lexer = null;
		public ILexer Lexer
		{
			get
			{
				return __lexer;
			}
			set
			{
				__lexer = value;
				__lexer.Initialize(new string[] { }, new string[] { });
			}
		}
        #endregion
		
        /// <summary>
        /// empty ctor 
        /// </summary>
        public BooScanner()
        {
        	//
        }
		
        /// <summary>
        /// ctor that takes a lexer as an argument.. assume the lexer has
        /// already had it's PEGs bound...
        /// </summary>
        /// <param name="buffer"></param>
        public BooScanner(ILexer lexer)
        {
        	Lexer = lexer;
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
        	  // words
            case PegTokenType.Keyword:
              ideToken.Type = TokenType.Keyword;
              ideToken.Color = TokenColor.Keyword;
              break;
            case PegTokenType.Macro:
              ideToken.Type = TokenType.Keyword;
              ideToken.Color = TokenColor.Keyword;
              break;
            case PegTokenType.Identifier:
              ideToken.Type = TokenType.Identifier;
              ideToken.Color = TokenColor.Text;
              break;
            // whitespace
        		case PegTokenType.Whitespace:
        	    ideToken.Type = TokenType.WhiteSpace;
                ideToken.Color = TokenColor.Text;
              break;
            // strings
            case PegTokenType.SingleQuoteString:
              ideToken.Type = TokenType.String;
              ideToken.Color = TokenColor.String;
              break;
            case PegTokenType.DoubleQuoteString:
              ideToken.Type = TokenType.String;
              ideToken.Color = TokenColor.String;
              break;
            case PegTokenType.Comma:
              ideToken.Type = TokenType.Text;
              ideToken.Color = TokenColor.Text;
              break;
            case PegTokenType.DoubleWhackLineComment:
              ideToken.Type = TokenType.Comment;
              ideToken.Color = TokenColor.Comment;
              break;
            case PegTokenType.NumberSignLineComment:
              ideToken.Type = TokenType.Comment;
              ideToken.Color = TokenColor.Comment;
              break;
            // the default case..
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
        	Lexer.NextToken(pegToken,ref state);
        	TranslatePegToken(pegToken, tokenInfo);
            if (pegToken.Type == PegTokenType.EOL)
                return false;
        	
        	return true;
        }

        public  void SetSource(string source, int offset)
        {
        	Lexer.SetSource(source.Substring(offset));
        }
        
        #endregion

    }

}

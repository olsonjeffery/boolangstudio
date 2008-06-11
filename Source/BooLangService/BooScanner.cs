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
        	_lexer.InitializeAndBindPegs(lexer.GetDefaultKeywordList(), new string[] { });
        }
        
        public BooScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer) 
            : this()
        {
            //
        }

        #region lexer/colorizing-related
		
        private TokenInfo _reusableIdeToken = new TokenInfo();
        public TokenInfo TranslatePegToken(PegToken token)
        {
        	// setting ide token and coloring info
        	switch(token.Type)
        	{
        		case PegTokenType.WhiteSpace:
        			_reusableIdeToken.Type = TokenType.WhiteSpace;
        			_reusableIdeToken.Color = TokenColor.Text;
        			break;
        		default:
        			_reusableIdeToken.Type = TokenType.Unknown;
        			_reusableIdeToken.Color = TokenColor.Text;
        			break;
        	}
        	_reusableIdeToken.StartIndex = token.StartIndex;
        	_reusableIdeToken.EndIndex = token.EndIndex;
        	
        	return _reusableIdeToken;
        }
        
        PegToken pegToken = new PegToken();
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
        	_lexer.NextToken(pegToken,ref state);
        	tokenInfo = TranslatePegToken(pegToken);
        	if (pegToken.Type == PegTokenType.EOL)
        		return false;
        	
        	return true;
        }

        public  void SetSource(string source, int offset)
        {
        	_lexer.SetSource(source.Substring(offset));
        }
        
        #endregion

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BooColorizerParser;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

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
        
        ColorizerToken colorizerToken = new ColorizerToken();
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
        	Lexer.NextToken(colorizerToken,ref state);
        	TranslatePegToken(colorizerToken, tokenInfo);
            if (colorizerToken.Type == PegTokenType.EOL)
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

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
        }
		
        /// <summary>
        /// ctor that takes a lexer as an argument
        /// </summary>
        /// <param name="buffer"></param>
        public BooScanner(PegLexer lexer)
        {
        	_lexer = lexer;
        }
        
        public BooScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer) 
            : this()
        {
            //
        }

        #region lexer/colorizing-related

        PegToken pegToken = null;
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
        	if(_lexer.NextToken(tokenInfo,pegToken,ref state) == false)
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

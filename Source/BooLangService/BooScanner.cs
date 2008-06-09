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
        private BooPegLexer.BooPegLexer _lexer = new BooPegLexer.BooPegLexer();

        
        private string _rawLine = string.Empty;
        public string RawLine
        {
        	get
        	{
        		return _rawLine;
        	}
        }
        public string Line
        {
        	get
        	{
        		if (Offset >= _rawLine.Length)
        			return string.Empty;
        		return _rawLine.Substring(Offset);
        	}
        }
        public string RemainingLine
        {
        	get
        	{
        		if (LexerIndex >= Line.Length)
        			return string.Empty;
        		return Line.Substring(LexerIndex);
        	}
        }
        
        private int _lexerIndex = 0;
        public int LexerIndex
        {
        	get 
        	{
        		return _lexerIndex;
        	}
        }
        
        private int _offset = 0;
        public int Offset
        {
        	get 
        	{
        		return _offset;
        	}
        }
        #endregion

        public BooScanner()
        {
        	//
        }

        public BooScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer) 
            : this()
        {
            //
        }

        #region lexer/colorizing-related

        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
        	if(_lexer.NextToken(tokenInfo,this.RemainingLine,ref state) == false)
        		return false;
        	_lexerIndex = tokenInfo.EndIndex + 1;
        	return true;
        }

        public  void SetSource(string source, int offset)
        {
            _rawLine = source;
            _offset = offset;
            _lexerIndex = 0;
        }
        #endregion

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Package;
using Boo.Lang.Parser;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangService
{
    public partial class BooScanner : IScanner
    {
        #region properties and members
        private antlr.TokenStream lexer;

        #endregion

        public BooScanner()
        {
            _currentLine = " ";
            lexer = null;
        }

        public BooScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer) 
            : this()
        {
            //
        }

        #region IScanner Members

        private antlr.CommonToken _reusableToken = null;
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            try
            {
                _reusableToken = lexer.nextToken() as antlr.CommonToken;
            }
            catch (Exception e)
            {
                // deal with the event of a trailing excl. point
                if (_currentLine[_currentLine.Length - 1] == '!')
                {
                    SetSource(_currentLine.Substring(0, _currentLine.Length - 1) + 'A', _offset);
                }
                    
            }

            // resolve token start and stop positions. Need
            // to do this first.
            ResolveBooTokenStartAndEndIndex(_reusableToken, tokenInfo);

            // if we get an EOF token, we're done.
            if (_reusableToken.Type == 1)
            {
                tokenInfo.Type = TokenType.WhiteSpace;
                // if we're in a ML_COMMENT zone, let's
                // just make sure that everything is
                // parsed as a comment...
                if (state == 13)
                {
                    tokenInfo.StartIndex = 0;
                    tokenInfo.EndIndex = _currentLine.Length;
                    tokenInfo.Type = TokenType.Comment;
                    tokenInfo.Color = TokenColor.Comment;
                }

                return false;
            }
            else if (state == 13)
            {
                _reusableToken.Type = BooLexer.ML_COMMENT;
            }
            if (_reusableToken.Type == BooLexer.ML_COMMENT)
            {
                state = 13;
                // how to determine if we're "out" of the ML_COMMENT?
                // if the token's getText() ends with "", then we
                // should assume that we're ending an ML_COMMENT region..

                if (_reusableToken.getFilename().Equals("LEAVINGML_COMMENT"))
                {
                    state = 0;
                }
                // handle an issue where we're hitting an endless loop
                // in the parser
                if (_reusableToken.Type == 120 && _reusableToken.getColumn() >= _currentLine.Length)
                {
                    tokenInfo.Type = TokenType.WhiteSpace;
                    return false;
                }
            }

            // set up token color and type
            ResolveBooTokenTypeAndColor(_reusableToken, tokenInfo);
            return true;
        }

        private string _currentLine;
        private int _offset;

        public  void SetSource(string source, int offset)
        {
            _offset = offset;
            _currentLine = source;
            if (_currentLine == string.Empty)
            {
                lexer = BooParser.CreateBooLexer(1, "", new StringReader(" "));
            }
            else
            {
               lexer = BooParser.CreateBooLexer(1, "", new StringReader(_currentLine));
            }
        }
        #endregion

    }

}

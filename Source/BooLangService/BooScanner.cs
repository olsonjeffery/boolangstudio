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

        #region internal parse tracker stuff
        private int _internalCurrentLinePosition = 0;
        public int InternalCurrentLinePosition
        {
            get
            {
                return _internalCurrentLinePosition;
            }
        }

        public char ConsumeInternalLineChar()
        {
            _internalCurrentLinePosition += 1;
            return _currentLine[_internalCurrentLinePosition - 1];
        }

        public string RemainingCurrentLine
        {
            get
            {
                if (_internalCurrentLinePosition < _currentLine.Length)
                    return _currentLine.Substring(_internalCurrentLinePosition);
                else
                    return string.Empty;
            }
        }

        #endregion

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
                else if (_currentLine[InternalCurrentLinePosition] == ' ' || (_currentLine[InternalCurrentLinePosition] == '"' || _currentLine[InternalCurrentLinePosition] == '\''))
                {
                    ConsumeInternalLineChar();
                    if (_currentLine[InternalCurrentLinePosition] == '"' || _currentLine[InternalCurrentLinePosition] == '\'')
                    {
                        // if we're here, that means we're inside of a
                        // malformed string token, most likely...
                        if (_currentLine[InternalCurrentLinePosition] == '"')
                        {
                            _reusableToken.setType(BooLexer.DOUBLE_QUOTED_STRING);
                            
                        }
                        else if (_currentLine[InternalCurrentLinePosition] == '\'')
                            _reusableToken.setType(BooLexer.SINGLE_QUOTED_STRING);

                        _reusableToken.setText(_currentLine.Substring(InternalCurrentLinePosition+1));
                        _reusableToken.setColumn(_internalCurrentLinePosition + 1);
                        // also a hint to the start and end index setting down the way...
                        _reusableToken.setLine(-10);
                        
                    }
                }
                    
            }

            // resolve token start and stop positions. Need
            // to do this first.
            ResolveBooTokenStartAndEndIndex(_reusableToken, tokenInfo);

            // here is where we set the internal tracker stuff
            _internalCurrentLinePosition = tokenInfo.EndIndex + 1;

            // if our internal tracker tells us we're at the end,
            // let's cut out all of that garbage crap the lexer
            // keeps returning to us
            // also we need to bail is we stumble upon some EOL's
            if (_reusableToken.Type == 1 || _reusableToken.Type == 9)
            {
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
        public string CurrentLine {
            get {
                return _currentLine;
            }}
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

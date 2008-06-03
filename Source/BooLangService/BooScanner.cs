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
        private antlr.TokenStream _lexer;

        #endregion

        public BooScanner()
        {
            _currentLine = " ";
            _lexer = null;
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
                if (_internalCurrentLinePosition >= _currentLine.Length)
                    return _currentLine.Length - 1;
                else
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

        public void SetInternalCurrentLinePosition(int val)
        {
            _internalCurrentLinePosition = val;
        }


        /// <summary>
        /// Handles any neccesary lexer token mangling in the even that the lexer throws an exception (malformed string tokens, mostly)
        /// </summary>
        /// <param name="currentLine">current code line</param>
        /// <param name="offset">integer offset.. from the ide</param>
        /// <param name="internalCurrentLinePosition">internal parser (our) reckoning of where we are in the current line</param>
        /// <param name="lexerToken">the token returned by the boo lexer</param>
        public void DealWithLexerException(string currentLine, int offset, int internalCurrentLinePosition, antlr.CommonToken lexerToken)
        {
            // deal with the event of a trailing excl. point
            if (currentLine[internalCurrentLinePosition] == '!')
            {
                SetSource(currentLine.Substring(0, currentLine.Length - 1) + 'A', offset);
            }
            else if (currentLine[internalCurrentLinePosition] == ' ')
            {
                internalCurrentLinePosition += 1;
                if (currentLine[internalCurrentLinePosition] == '"' || currentLine[internalCurrentLinePosition] == '\'')
                {
                    DealWithMalformedStringTokens(lexerToken, currentLine, internalCurrentLinePosition);
                }

            }
            else if (currentLine[internalCurrentLinePosition] == '"' || currentLine[internalCurrentLinePosition] == '\'')
            {
                DealWithMalformedStringTokens(lexerToken, currentLine, internalCurrentLinePosition);
            }
        }

        public void DealWithMalformedStringTokens(antlr.CommonToken lexerToken, string currentLine, int internalCurrentLinePosition)
        {
            // if we're here, that means we're inside of a
            // malformed string token, most likely...
            if (currentLine[internalCurrentLinePosition] == '"')
            {
                lexerToken.setType(BooLexer.DOUBLE_QUOTED_STRING);

            }
            else if (currentLine[internalCurrentLinePosition] == '\'')
                lexerToken.setType(BooLexer.SINGLE_QUOTED_STRING);

            lexerToken.setText(currentLine.Substring(internalCurrentLinePosition + 1));
            lexerToken.setColumn(internalCurrentLinePosition + 1);
            // also a hint to the start and end index setting down the way...
            lexerToken.setLine(-10);
        }

        /// <summary>
        /// Does some "home-grown" lexing, in the event of the lexer returning a null token,
        /// possibly in the case of a multiline comment
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="internalCurrentLinePosition"></param>
        /// <returns></returns>
        public antlr.CommonToken DealWithNullLexerToken(string currentLine, int internalCurrentLinePosition)
        {
            antlr.CommonToken lexerToken = new antlr.CommonToken();
            throw new NotImplementedException();
        }

        private bool doLineParseBailOut = false;
        public bool ScanTokenWrapper(TokenInfo tokenInfo, ref int state, antlr.TokenStream lexer, string currentLine, int offset)
        {
            // this is to short circuit the line parsing process.
            if (doLineParseBailOut)
            {
                doLineParseBailOut = false;
                tokenInfo.Type = TokenType.WhiteSpace;
                return false;
            }
                
            try
            {
                _reusableToken = lexer.nextToken() as antlr.CommonToken;
            }
            catch (Exception e)
            {
                DealWithLexerException(currentLine, offset, InternalCurrentLinePosition, _reusableToken);
            }

            // something here to deal with null _reusableTokens
            if (_reusableToken == null)
            {
                _reusableToken = DealWithNullLexerToken(currentLine, InternalCurrentLinePosition);
            }

            // resolve token start and stop positions. Need
            // to do this first.
            ResolveBooTokenStartAndEndIndex(_reusableToken, tokenInfo);

            // here is where we check for single line comments
            // don't check if we're in a string token...
            if ((_reusableToken.Type != BooLexer.SINGLE_QUOTED_STRING || _reusableToken.Type != BooLexer.DOUBLE_QUOTED_STRING)&&currentLine.Length > 0)
            {
                while (currentLine[InternalCurrentLinePosition] == ' ' || currentLine[InternalCurrentLinePosition] == '\t')
                {
                    _internalCurrentLinePosition += 1;
                    if (_internalCurrentLinePosition > currentLine.Length)
                        break;
                }
                // catching line comments
                if ((currentLine[InternalCurrentLinePosition] == '/' && currentLine[InternalCurrentLinePosition + 1] == '/')||currentLine[InternalCurrentLinePosition] == '#')
                {
                    DealWithComment(InternalCurrentLinePosition,currentLine.Length - 1, tokenInfo);
                    doLineParseBailOut = true;
                    return true;
                }
                
            }
            

            // here is where we set the internal tracker stuff
            SetInternalCurrentLinePosition(tokenInfo.EndIndex + 1);

            // if we should happen upon a an EOF or EOL token, let's
            // split
            if (_reusableToken.Type == 1 || _reusableToken.Type == 9)
            {
                return false;
            }

            // set up token color and type
            ResolveBooTokenTypeAndColor(_reusableToken, tokenInfo);
            return true;
        }

        public void DealWithComment(int start, int end, TokenInfo tokenInfo)
        {
            tokenInfo.StartIndex = start;
            tokenInfo.EndIndex = end;
            tokenInfo.Color = TokenColor.Comment;
            tokenInfo.Type = TokenType.Comment;
        }

        #endregion

        private antlr.CommonToken _reusableToken = null;
        
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
            return ScanTokenWrapper(tokenInfo, ref state, _lexer, _currentLine, _offset);
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
            _internalCurrentLinePosition = 0;
            if (_currentLine == string.Empty)
            {
                _lexer = BooParser.CreateBooLexer(1, "", new StringReader(" "));
            }
            else
            {
               _lexer = BooParser.CreateBooLexer(1, "", new StringReader(_currentLine));
            }
        }
        #endregion

    }

}

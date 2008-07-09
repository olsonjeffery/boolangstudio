using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using BooPegLexer;

namespace Boo.BooLangService
{
    public partial class BooScanner : IScanner
    {


        #region token builder helper methods

        public void TranslatePegToken(PegToken token, TokenInfo ideToken)
        {
            // setting ide token and coloring info
            switch (token.Type)
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
                case PegTokenType.LeftParen:
                    ideToken.Type = TokenType.Delimiter;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.RightParen:
                    ideToken.Type = TokenType.Delimiter;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.AdditionSign:
                    ideToken.Type = TokenType.Operator;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.SubtractionSign:
                    ideToken.Type = TokenType.Operator;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.DivisionSign:
                    ideToken.Type = TokenType.Operator;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.MultiplicationSign:
                    ideToken.Type = TokenType.Operator;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.EqualsSign:
                    ideToken.Type = TokenType.Operator;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.Period:
                    ideToken.Type = TokenType.Operator;
                    ideToken.Color = TokenColor.Text;
                    ideToken.Trigger = TokenTriggers.MemberSelect;
                    break;
                case PegTokenType.LeftSquareBracket:
                    ideToken.Type = TokenType.Delimiter;
                    ideToken.Color = TokenColor.Text;
                    ideToken.Trigger = TokenTriggers.ParameterStart | TokenTriggers.MatchBraces;
                    break;
                case PegTokenType.RightSquareBracket:
                    ideToken.Type = TokenType.Delimiter;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.LeftCurlyBrace:
                    ideToken.Type = TokenType.Delimiter;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.RightCurlyBrace:
                    ideToken.Type = TokenType.Delimiter;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.Splice:
                    ideToken.Type = TokenType.Operator;
                    ideToken.Color = TokenColor.Text;
                    break;
                case PegTokenType.FloatLiteral:
                    ideToken.Type = TokenType.Literal;
                    ideToken.Color = TokenColor.Number;
                    break;
                case PegTokenType.IntegerLiteral:
                    ideToken.Type = TokenType.Literal;
                    ideToken.Color = TokenColor.Number;
                    break;
                case PegTokenType.MlComment:
                    ideToken.Type = TokenType.Comment;
                    ideToken.Color = TokenColor.Comment;
                    break;
                case PegTokenType.MlCommentOpen:
                    ideToken.Type = TokenType.Comment;
                    ideToken.Color = TokenColor.Comment;
                    break;
                case PegTokenType.MlCommentClose:
                    ideToken.Type = TokenType.Comment;
                    ideToken.Color = TokenColor.Comment;
                    break;
                case PegTokenType.TripleQuoteString:
                    ideToken.Type = TokenType.String;
                    ideToken.Color = TokenColor.String;
                    break;
                case PegTokenType.TripleQuoteStringOpen:
                    ideToken.Type = TokenType.String;
                    ideToken.Color = TokenColor.String;
                    break;
                case PegTokenType.TripleQuoteStringClose:
                    ideToken.Type = TokenType.String;
                    ideToken.Color = TokenColor.String;
                    break;
                case PegTokenType.MalformedSingleQuoteString:
                    ideToken.Type = TokenType.String;
                    ideToken.Color = TokenColor.String;
                    break;
                case PegTokenType.MalformedDoubleQuoteString:
                    ideToken.Type = TokenType.String;
                    ideToken.Color = TokenColor.String;
                    break;
                case PegTokenType.Exclamation:
                    ideToken.Type = TokenType.Text;
                    ideToken.Type = TokenType.Text;
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
        #endregion

    }
}

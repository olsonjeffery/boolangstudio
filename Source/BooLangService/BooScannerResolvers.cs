using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using Boo.Lang.Parser;

namespace Boo.BooLangService
{
    public partial class BooScanner : IScanner
    {


        #region token builder helper methods

        public void ResolveBooTokenStartAndEndIndex(antlr.CommonToken token, TokenInfo tokenInfo)
        {
            if (token == null)
                throw new NotImplementedException(
                    "\n               01234567890123456789012345678901234567890123456789\n" +
                    "current line: '" + _currentLine + "' last internal pos: " + InternalCurrentLinePosition.ToString());
            int oneCharBack = token.getColumn() - 1;
            int lengthOfTokenText = token.getText() == null ? 0 : token.getText().Length;
            int oneCharAfterToken = token.getColumn() + lengthOfTokenText;
            
            // single/double quoted string
            if (token.Type == BooLexer.SINGLE_QUOTED_STRING || token.Type == BooLexer.DOUBLE_QUOTED_STRING)
            {
                tokenInfo.StartIndex = oneCharBack;
                tokenInfo.EndIndex = oneCharAfterToken;
                // for malformed (ie no closing quote) strings
                if (token.getLine() == -10)
                    tokenInfo.EndIndex -= 1;
            }
            else if (token.Type == BooLexer.TRIPLE_QUOTED_STRING)
            {
                tokenInfo.StartIndex = oneCharBack;
                tokenInfo.EndIndex = oneCharBack+ 5 + token.getText().Length;
            }
            else if (token.Type == 1)
            {
                return;
            }
            else
            {
                tokenInfo.StartIndex = oneCharBack;
                tokenInfo.EndIndex = oneCharBack + (token.getText().Length - 1);
            }
        }

        public void ResolveBooTokenTypeAndColor(antlr.CommonToken token,TokenInfo tokenInfo)
        {
            tokenInfo.Color = TokenColor.Text;

            switch (token.Type)
            {
                case BooLexer.EOF:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.NULL_TREE_LOOKAHEAD:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.INDENT:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.DEDENT:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.ELIST:
                    tokenInfo.Type = TokenType.Text;
                    break;
                case BooLexer.DLIST:
                    tokenInfo.Type = TokenType.Text;
                    break;
                case BooLexer.ESEPARATOR:
                    tokenInfo.Type = TokenType.Text;
                    break;
                case BooLexer.EOL:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.ABSTRACT:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.AND:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.AS:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.BREAK:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.CONTINUE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.CAST:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.CHAR:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.CLASS:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.CONSTRUCTOR:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.DEF:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.DESTRUCTOR:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.DO:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.ELIF:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.ELSE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.ENSURE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.ENUM:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.EVENT:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.EXCEPT:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.FAILURE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.FINAL:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.FROM:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.FOR:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.FALSE:
                    tokenInfo.Type = TokenType.Literal;
                    break;
                case BooLexer.GET:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.GOTO:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.IMPORT:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.INTERFACE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.INTERNAL:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.IS:
                    tokenInfo.Type = TokenType.Operator;
                    tokenInfo.Color = TokenColor.Keyword;
                    break;
                case BooLexer.ISA:
                    tokenInfo.Type = TokenType.Operator;
                    tokenInfo.Color = TokenColor.Keyword;
                    break;
                case BooLexer.IF:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.IN:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.NOT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.NULL:
                    tokenInfo.Type = TokenType.Literal;
                    break;
                case BooLexer.OF:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.OR:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.OVERRIDE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.PASS:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.NAMESPACE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.PARTIAL:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.PUBLIC:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.PROTECTED:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.PRIVATE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.RAISE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.REF:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.RETURN:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.SET:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.SELF:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.SUPER:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.STATIC:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.STRUCT:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.THEN:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.TRY:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.TRANSIENT:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.TRUE:
                    tokenInfo.Type = TokenType.Literal;
                    break;
                case BooLexer.TYPEOF:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.UNLESS:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.VIRTUAL:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.WHILE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.YIELD:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.TRIPLE_QUOTED_STRING:
                    tokenInfo.Type = TokenType.String;
                    tokenInfo.Color = TokenColor.String;
                    break;
                case BooLexer.EOS:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.DOUBLE_QUOTED_STRING:
                    tokenInfo.Type = TokenType.String;;
                    tokenInfo.Color = TokenColor.String;
                    break;
                case BooLexer.SINGLE_QUOTED_STRING:
                    // TODO single quote doesnt get colorized.. why?
                    tokenInfo.Type = TokenType.String;
                    tokenInfo.Color = TokenColor.String;
                    break;
                case BooLexer.ID:
                    tokenInfo.Type = TokenType.Identifier;
                    break;
                case BooLexer.LBRACK:
                    tokenInfo.Type = TokenType.Delimiter;
                    tokenInfo.Trigger = TokenTriggers.ParameterStart | TokenTriggers.MatchBraces;
                    break;
                case BooLexer.RBRACK:
                    tokenInfo.Type = TokenType.Delimiter;
                    //TODO:tokenInfo.Trigger = TokenTriggers.ParameterEnd | TokenTriggers.MatchBraces;
                    break;
                case BooLexer.LPAREN:
                    tokenInfo.Type = TokenType.Delimiter;
                    //TODO: tokenInfo.Trigger = TokenTriggers.ParameterStart | TokenTriggers.MatchBraces;
                    break;
                case BooLexer.RPAREN:
                    tokenInfo.Type = TokenType.Delimiter;
                    //TODO: tokenInfo.Trigger = TokenTriggers.ParameterEnd | TokenTriggers.MatchBraces;
                    break;
                case BooLexer.ASSIGN:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.SUBTRACT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.COMMA:
                    tokenInfo.Type = TokenType.Text;
                    //TODO: COMMA.. tokenInfo.Trigger = TokenTriggers.ParameterNext;
                    break;
                case BooLexer.ASSEMBLY_ATTRIBUTE_BEGIN:
                    tokenInfo.Type = TokenType.Text;
                    break;
                case BooLexer.SPLICE_BEGIN:
                    tokenInfo.Type = TokenType.Operator;
                    tokenInfo.Color = TokenColor.Keyword;
                    // TODO: what to do with the splice operator?
                    break;
                case BooLexer.DOT:
                    tokenInfo.Type = TokenType.Delimiter;
                    tokenInfo.Trigger = TokenTriggers.MemberSelect;
                    // TODO: DOT OPERATOR.. NEED TO STUB OUT STUFF FOR INTELLISENSE HERE
                    break;
                case BooLexer.COLON:
                    tokenInfo.Type = TokenType.Operator;
                    // TODO: single indentation when pressing return here?
                    break;
                case BooLexer.MULTIPLY:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                //TODO: case BooLexer.NULLABLE_SUFFIX .. not implemented keyword?
                case BooLexer.BITWISE_OR:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.LBRACE:
                    tokenInfo.Type = TokenType.Text;
                    break;
                case BooLexer.RBRACE:
                    tokenInfo.Type = TokenType.Text;
                    break;
                case BooLexer.QQ_BEGIN:
                    tokenInfo.Type = TokenType.Text;
                    // TODO:brace matching?
                    break;
                case BooLexer.QQ_END:
                    tokenInfo.Type = TokenType.Text;
                    // TODO:brace matching?
                    break;
                case BooLexer.INPLACE_BITWISE_OR:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.INPLACE_BITWISE_AND:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.INPLACE_SHIFT_LEFT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.INPLACE_SHIFT_RIGHT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.CMP_OPERATOR:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.GREATER_THAN:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.LESS_THAN:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.ADD:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.EXCLUSIVE_OR:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.DIVISION:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.MODULUS:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.BITWISE_AND:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.SHIFT_LEFT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.SHIFT_RIGHT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.EXPONENTIATION:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.INCREMENT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.DECREMENT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.ONES_COMPLEMENT:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.INT:
                    tokenInfo.Type = TokenType.Keyword;
                    // TODO: SHOULD THIS BE A KEYWORD?
                    break;
                case BooLexer.LONG:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.RE_LITERAL:
                    tokenInfo.Type = TokenType.Literal;
                    break;
                case BooLexer.DOUBLE:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.FLOAT:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.TIMESPAN:
                    tokenInfo.Type = TokenType.Keyword;
                    break;
                case BooLexer.LINE_CONTINUATION:
                    tokenInfo.Type = TokenType.Operator;
                    break;
                case BooLexer.SL_COMMENT:
                    tokenInfo.Type = TokenType.Comment;
                    tokenInfo.Color = TokenColor.Comment;
                    break;
                case BooLexer.ML_COMMENT:
                    tokenInfo.Type = TokenType.Comment;
                    tokenInfo.Color = TokenColor.Comment;
                    break;
                case BooLexer.WS:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.X_RE_LITERAL:
                    tokenInfo.Type = TokenType.Literal;
                    break;
                case BooLexer.NEWLINE:
                    tokenInfo.Type = TokenType.WhiteSpace;
                    break;
                case BooLexer.ESCAPED_EXPRESSION:
                    tokenInfo.Type = TokenType.Text;
                    // TODO: what is this for?
                    break;
                case BooLexer.DQS_ESC:
                    tokenInfo.Type = TokenType.Text;
                    // TODO: wtf is this?
                    break;
                case BooLexer.SQS_ESC:
                    tokenInfo.Type = TokenType.Text;
                    // TODO: again, wtf is this?
                    break;
                case BooLexer.SESC:
                    tokenInfo.Type = TokenType.Text;
                    // TODO: getting tired of wondering what these cryptic keywords are
                    break;
                case BooLexer.RE_CHAR:
                    tokenInfo.Type = TokenType.Literal;
                    // TODO these RE_* tokens are part of inline regex, i believe..
                    // how to display them>?
                    break;
                case BooLexer.X_RE_CHAR:
                    tokenInfo.Type = TokenType.Literal;
                    // TODO: what's the diff between RE_* and X_RE_* ?
                    break;
                case BooLexer.RE_ESC:
                    tokenInfo.Type = TokenType.Literal;
                    break;
                case BooLexer.DIGIT_GROUP:
                    tokenInfo.Type = TokenType.Literal;
                    tokenInfo.Color = TokenColor.Number;
                    // TODO: literal digits.. i assume
                    break;
                case BooLexer.REVERSE_DIGIT_GROUP:
                    tokenInfo.Type = TokenType.Literal;
                    tokenInfo.Color = TokenColor.Number;
                    break;
                case BooLexer.ID_PREFIX:
                    tokenInfo.Type = TokenType.Identifier;
                    // TODO: should be parsed an id?
                    break;
                case BooLexer.ID_LETTER:
                    tokenInfo.Type = TokenType.Identifier;
                    break;
                case BooLexer.DIGIT:
                    tokenInfo.Type = TokenType.Literal;
                    tokenInfo.Color = TokenColor.Number;
                    break;
                case BooLexer.HEXDIGIT:
                    tokenInfo.Type = TokenType.Literal;
                    tokenInfo.Color = TokenColor.Number;
                    break;
                default:
                    tokenInfo.Type = TokenType.Text;
                    break;
            }

            if (tokenInfo.Type == TokenType.Keyword) 
                tokenInfo.Color = TokenColor.Keyword;
            
                
        }

        #endregion

    }
}

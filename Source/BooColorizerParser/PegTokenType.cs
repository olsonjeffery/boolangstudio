using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooColorizerParser
{
  public enum PegTokenType
  {
    Keyword,
    Macro,
    Identifier,
    Whitespace,
    SingleQuoteString,
    DoubleQuoteString,
    Comma,
    DoubleWhackLineComment,
    NumberSignLineComment,
    LeftParen,
    RightParen,
    AdditionSign,
    SubtractionSign,
    DivisionSign,
    MultiplicationSign,
    EqualsSign,
    Period,
    LeftSquareBracket,
    RightSquareBracket,
    LeftCurlyBrace,
    RightCurlyBrace,
    Splice,
    FloatLiteral,
    IntegerLiteral,
    MlComment,
    MlCommentOpen,
    MlCommentClose,
    TripleQuoteString,
    TripleQuoteStringOpen,
    TripleQuoteStringClose,
    MalformedSingleQuoteString,
    MalformedDoubleQuoteString,
    Exclamation,
    EOL
  }
}

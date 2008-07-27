namespace BooPegLexer

import System

public enum PegTokenType:
	Whitespace
	EOL
	SingleQuoteString
	DoubleQuoteString
	Identifier
	Keyword
	Macro
	AdditionSign
	SubtractionSign
	EqualsSign
	Comma
	LeftParen
	RightParen
	QqOpen
	QqClose
	DoubleWhackLineComment
	NumberSignLineComment
	DivisionSign
	MultiplicationSign
	Period
	LeftSquareBracket
	RightSquareBracket
	LeftCurlyBrace
	RightCurlyBrace
	Splice
	IntegerLiteral
	FloatLiteral
	TimespanLiteral
	MlComment
	MlCommentOpen
	MlCommentClose
	TripleQuoteString
	TripleQuoteStringOpen
	TripleQuoteStringClose
	MalformedSingleQuoteString
	MalformedDoubleQuoteString
	Exclamation

	
	


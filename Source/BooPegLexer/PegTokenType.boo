namespace BooPegLexer

import System

public enum PegTokenType:
	EOL
	Unknown
	Identifier
	Keyword
	Macro
	WhiteSpace
	SingleQuoteString
	MalformedSingleQuoteString
	DoubleQuoteString
	MalformedDoubleQuoteString
	TrippleQuoteString
	MalformedTrippleQuoteString
	DoubleWhackLineComment
	NumberSignLineComment
	MultiLineComment
	MultiLineCommentClose
	QqOpen
	QqClose
	LeftParen
	RightParen
	AdditionOperator
	SubtractionOpearator
	MultiplicationOpeartor
	
	


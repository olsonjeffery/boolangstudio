namespace BooPegLexer

import System

public class PegToken:
	
	def constructor():
		pass
	
	[property(Type)]
	_type as PegTokenType
	
	[property(StartIndex)]
	_startIndex as int
	
	[property(EndIndex)]
	_endIndex as int

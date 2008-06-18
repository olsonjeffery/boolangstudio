namespace BooPegLexer

import System
import System.Collections.Generic

public interface ILexer:
	def NextToken(token as PegToken, ref state as int) as bool
	def SetSource(line as string)
	def Initialize(keywords as (string), macros as (string))
	Macros as List of string:
		get
	Keywords as List of string:
		get
	Line as string:
		get
	RemainingLine as string:
		get
	CurrentIndex as int:
		get
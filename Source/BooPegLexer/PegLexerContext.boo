namespace BooPegLexer

import System
import Boo.Pegs

public class PegLexerContext(PegContext):
  
  [property(Token)]
  private _token as PegToken
  
  public def constructor():
    super("")
    _token = PegToken()
  
  public def constructor(line as string):
    super(line)
    _token = PegToken()
  
  public def constructor(line as string, token as PegToken):
    super(line)
    _token = token


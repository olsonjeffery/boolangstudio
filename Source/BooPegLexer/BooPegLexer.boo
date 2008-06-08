namespace BooPegLexer

import System
import System.Collections.Generic
import Microsoft.VisualStudio.Package
import Boo.Pegs

public class BooPegLexer:
  
  #region properties
  private _line = ''
  public Line:
    get:
      return _line
    
  private _offset = 0
  public Offset:
    get:
      return _offset
  
  private _keywords = Dictionary [of string, bool]()
  public Keywords:  
    get:
      return _keywords
  
  private _macros = Dictionary [of string, bool]()
  public Macros:
    get:
      return _macros
  
  private _currentIndex = 0
  private _lastIndex = 0
  #endregion
  
  #region ctors
  public def constructor():
    pass
   
  public def constructor(line as string, offset as int):
    _line = line
    _offset = offset
  
  #endregion
  
  #region Common public interface
  public def SetSource(line as string, offset as int):
    _line = line
    _offset = offset
    _currentIndex = offset
    _lastIndex = offset
  	
  public def NextToken(token as TokenInfo, ref state as int) as bool:
    
    # logic goes here
    
    return true
    
  #endregion
    
  #region PEG-related members and fields
  
  
  
  public def BindPeg(keywords as (string), macros as (string)):
    pass
  
  #endregion
namespace BooPegLexer

import System
import System.Collections.Generic
import Microsoft.VisualStudio.Package
import Boo.Pegs
import Boo.Lang.Compiler
import Boo.Lang.Compiler.MetaProgramming

public class PegLexer:
  
  #region properties
  private _keywords = List of string()
  public Keywords:  
    get:
      return _keywords
  
  private _macros = List of string()
  public Macros:
    get:
      return _macros
      
  private _line as string = string.Empty
  public EntireLine as string:
  	get:
  		return _line
  
  private _currentIndex as int = 0
  
  public RemainingLine as string:
  	get:
  		return _line.Substring(_currentIndex)
  
  #endregion
  
  #region ctors
  public def constructor():
    pass  
  #endregion
  
  #region Common public interface  	
  public def SetSource(line as string):
  	_line = line
  	_currentPos = 0
  
  public def NextToken(ideToken as TokenInfo, pegToken as PegToken, ref state as int) as bool:
    
    # logic goes here
    ideToken.Type = TokenType.Unknown
    ideToken.Color = TokenColor.Text
    ideToken.StartIndex = 0
    ideToken.EndIndex = 0
    
    result = false
    # try and guess what the next token type is..
    if (state == 13):
      # we're in a multi-line comment zone, the only hope
      # is to match it against a ML-comment, otherwise
      # we return the entire line as a ml-comment token
      result = self.InMultiLineComment(ideToken,pegToken,state)
    elif (state == 14):
      # we're in a tripple-quote zone, ditto as above
      result = self.InTrippleQuoteString(ideToken,pegToken,state)
    else:
      # otherwise, try and figure out what the next token
      # is gonna be
      result = self.InGeneralLexingCase(ideToken,pegToken,state)
    
    # what to do with a false result?
    
    # turn the appropriate peg context loose on it..
    
    # set the tokenInfo
    
    return false
    
  #endregion
  
  #region Logic related..
  
  public virtual def InMultiLineComment(tokenInfo as TokenInfo, pegToken as PegToken, ref state as int):
  	return false
  
  public virtual def InTrippleQuoteString(tokenInfo as TokenInfo, pegToken as PegToken, ref state as int):
  	return false
  
  public virtual def InGeneralLexingCase(tokenInfo as TokenInfo, pegToken as PegToken, ref state as int):
  	return false
  
  #endregion
  
  #region PEG related members and fields
  
  # identifiers and keywords
  private Keyword
  
  IsKeyword = FunctionExpression() do (ctx as PegContext):
    identifier = text(ctx)
    return identifier in Keywords
  
  IsMacro = FunctionExpression() do (ctx as PegContext):
    identifier = text(ctx)
    return identifier in Macros
  
  # meant to be ran once on class setup...?
  public def InitializeAndBindPegs(keywords as (string), macros as (string)):
    peg:
      Keyword = ++[a-z],IsKeyword
  
  #endregion
  
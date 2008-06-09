namespace BooPegLexer

import System
import System.Collections.Generic
import Microsoft.VisualStudio.Package
import Boo.Pegs
import Boo.Lang.Compiler
import Boo.Lang.Compiler.MetaProgramming

public class BooPegLexer:
  
  #region properties
  private _keywords = List of string()
  public Keywords:  
    get:
      return _keywords
  
  private _macros = List of string()
  public Macros:
    get:
      return _macros
  #endregion
  
  #region ctors
  public def constructor():
    pass  
  #endregion
  
  #region Common public interface  	
  public def NextToken(token as TokenInfo, line as string, ref state as int) as bool:
    
    # logic goes here
    token.Type = TokenType.Unknown
    token.Color = TokenColor.Text
    token.StartIndex = 0
    token.EndIndex = 0
    
    result = false
    # try and guess what the next token type is..
    if (state == 13):
      # we're in a multi-line comment zone, the only hope
      # is to match it against a ML-comment, otherwise
      # we return the entire line as a ml-comment token
      result = self.InMultiLineComment(token,line,state)
    elif (state == 14):
      # we're in a tripple-quote zone, ditto as above
      result = self.InTrippleQuoteString(token,line,state)
    else:
      # otherwise, try and figure out what the next token
      # is gonna be
      result = self.InGeneralLexingCase(token,line,state)
    
    # what to do with a false result?
    
    # turn the appropriate peg context loose on it..
    
    # set the tokenInfo
    
    return false
    
  #endregion
  
  #region Logic related..
  
  public virtual def InMultiLineComment(tokenInfo as TokenInfo, line as string, ref state as int):
  	return false
  
  public virtual def InTrippleQuoteString(tokenInfo as TokenInfo, line as string, ref state as int):
  	return false
  
  public virtual def InGeneralLexingCase(tokenInfo as TokenInfo, line as string, ref state as int):
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
  
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
    
    # try and guess what the next token type is..
    if (state == 13):
      # we're in a multi-line comment zone, the only hope
      # is to match it against a ML-comment, otherwise
      # we return the entire line as a ml-comment token
      self.InMultiLineComment()
    elif (state == 14):
      # we're in a tripple-quote zone, ditto as above
      self.InTrippleQuoteString(token,line,state)
    else:
      # otherwise, try and figure out what the next token
      # is gonna be
      self.GeneralLexingCase(token,line,state)
    
    # turn the appropriate peg context loose on it..
    
    # set the tokenInfo
    
    return false
    
  #endregion
  
  #region Logic related..
  
  public virtual def InMultiLineComment() as bool:
  	return false
  
  public virtual def InTrippleQuoteString(tokenInfo as TokenInfo, line as string, ref state as int):
  	return false
  
  public virtual def GeneralLexingCase(tokenInfo as TokenInfo, line as string, ref state as int):
  	return false
  
  #endregion
  
  #region PEG related members and fields
  
  # identifiers and keywords
  private Keyword as PegContext
  private Identifier as PegContext
  
  # string literals
  private SingleQuotedString as PegContext
  private DoubleQuotedString as PegContext
  private TrippleQuotedString as PegContext
  private InterpolatedString as PegContext
  
  # comment realted
  private DoubleWhackLineComment as PegContext
  private NumberSymbolLineComment as PegContext
  
  # delimiters
  private LeftParen as PegContext
  private RightParen as PegContext
  private OpenQq as PegContext
  private CloseQq as PegContext
  private Comma as PegContext
  private Colon as PegContext
  
  # numeric literals
  private IntegerNumber as PegContext
  private DecimalNumber as PegContext
  
  # misc
  private InlineRegex as PegContext
  
  # whitespace
  private Whitespace as PegContext
  private NewLine as PegContext
  private Space as PegContext
  private Tab as PegExpression
  private EndOfFile as PegExpression
  
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
  
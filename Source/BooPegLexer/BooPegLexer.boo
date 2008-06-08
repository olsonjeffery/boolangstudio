namespace BooPegLexer

import System
import System.Collections.Generic
import Microsoft.VisualStudio.Package
import Boo.Pegs
import Boo.Lang.Compiler
import Boo.Lang.Compiler.MetaProgramming

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
  private Tab as PegContext
  private EndOfFile as PegContext
  
  IsKeyword = FunctionExpression() do (ctx as PegContext):
    identifier = text(ctx)
    return identifier in Keywords.Keys
    
  IsNotKeyword = FunctionExpression() do (ctx as PegContext):
    identifier = text(ctx)
    return identifier not in Keywords.Keys
  
  # meant to be ran once on class setup...?
  public def InitializeAndBindPegs(keywords as (string), macros as (string)):
    peg:
      Keyword = ++[a-z],IsKeyword
  
  #endregion
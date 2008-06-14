namespace BooPegLexer

import System
import System.Collections.Generic
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
  
  private _lineLength as int:
    get:
      return _line.Length
  
  private _currentIndex as int = 0
  public CurrentIndex as int:
    get:
      return _currentIndex
    set:
      _currentIndex = value

  
  public RemainingLine as string:
  	get:
  	  if CurrentIndex >= _line.Length:
  	    return string.Empty
  	  else:
  	    return _line.Substring(CurrentIndex)
  
  #endregion
  
  #region ctors
  public def constructor():
    pass  
  #endregion
  
  #region Common public interface  	
  public def SetSource(line as string):
  	_line = line
  	_currentIndex = 0
  
  public def NextToken(token as PegToken, ref state as int) as bool:
    
    result = false
    
    # we've hit EOL
    if RemainingLine.Equals(string.Empty):
      return false
    
    # try and guess what the next token type is..
    if (state == 13):
      # we're in a multi-line comment zone, the only hope
      # is to match it against a ML-comment, otherwise
      # we return the entire line as a ml-comment token
      result = InMultiLineComment(token,state)
    elif (state == 14):
      # we're in a tripple-quote zone, ditto as above
      result = InTrippleQuoteString(token,state)
    else:
      # otherwise, try and figure out what the next token
      # is gonna be
      result = InGeneralLexingCase(token,state)
    
    # if nothing in the items above can parse it,
    # we'll just mark the rest of the line as unparsable
    # for now
    if result == false:
      token.StartIndex = _currentIndex
      token.EndIndex = _line.Length-1
      _currentIndex = _line.Length
    
    return result
    
  #endregion
  
  #region Logic related..
  
  public def GetContext(pegToken as PegToken) as PegContext:
    ctx = PegLexerContext(RemainingLine,pegToken)
    ctx.Token.Type = PegTokenType.EOL
    ctx.Token.StartIndex = 0
    ctx.Token.EndIndex = 0
    return ctx
  
  public virtual def InMultiLineComment(pegToken as PegToken, ref state as int):
  	return false
  
  public virtual def InTrippleQuoteString(pegToken as PegToken, ref state as int):
  	return false
  
  public virtual def InGeneralLexingCase(pegToken as PegToken, ref state as int):
    ctx = GetContext(pegToken)
    result = ctx.Match(self.BooTokenPeg)
    if not result:
      raise Exception("Match has failed! BARF!!!!")
    return result
  
  #endregion
  
  #region PEG related members and fields
  
  # identifiers and keywords 
  IsKeyword = FunctionExpression() do (ctx as PegContext):
    identifier = text(ctx)
    return identifier in Keywords
  
  IsMacro = FunctionExpression() do (ctx as PegContext):
    identifier = text(ctx)
    return identifier in Macros
  
  public def HandlePegMatch(ctx as PegLexerContext, type as PegTokenType):
    line = text(ctx)
    ctx.Token.Type = type
    ctx.Token.StartIndex = _currentIndex
    ctx.Token.EndIndex = _currentIndex + line.Length-1
    _currentIndex += line.Length
    print "Type: "+ctx.Token.Type+" Start: "+ctx.Token.StartIndex+ " End: "+ctx.Token.EndIndex +" New _currentIndex: "+_currentIndex
  	
  private BooTokenPeg as ChoiceExpression
  
  # meant to be ran once on class setup...?
  public def InitializeAndBindPegs(keywords as (string), macros as (string)):
    Keywords.AddRange(keywords)
    Macros.AddRange(macros)
    peg:
      self.BooTokenPeg = [Words,Whitespace,Strings,MiscOperators,Delimiters]
      
      # words
      Words = [Keyword,Identifier,Macro]
      Keyword = ++[a-z], IsKeyword,{$HandlePegMatch(PegTokenType.Keyword)}
      Identifier = [a-z,A-Z,'_'],--[a-z,A-Z,'_',0-9], not IsKeyword,not IsMacro,{$HandlePegMatch(PegTokenType.Identifier)}
      Macro = ++[a-z], IsMacro,{$HandlePegMatch(PegTokenType.Macro)}
      
      Whitespace = ++whitespace(),{$HandlePegMatch(PegTokenType.Whitespace)}
      
      # strings
      Strings = [SingleQuoteString]
      SingleQuoteString = "'",--(not "'", any()),"'",{$HandlePegMatch(PegTokenType.SingleQuoteString)}
      
      # misc operators
      MiscOperators = [AdditionSign,SubtractionSign,EqualsSign]
      AdditionSign = ++'+',{$HandlePegMatch(PegTokenType.AdditionSign)}
      SubtractionSign = ++'-',{$HandlePegMatch(PegTokenType.SubtractionSign)}
      EqualsSign = ++'=',{$HandlePegMatch(PegTokenType.EqualsSign)}
      
      # delimiters
      Delimiters = [LeftParen,RightParen,QqOpen,QqClose]
      LeftParen = '(',{$HandlePegMatch(PegTokenType.LeftParen)}
      RightParen = ')',{$HandlePegMatch(PegTokenType.RightParen)}
      QqOpen = '[|',{$HandlePegMatch(PegTokenType.QqOpen)}
      QqClose = '|]',{$HandlePegMatch(PegTokenType.QqClose)}
      
  
  public def GetDefaultKeywordList() as (string):
  	return ("def","class","interface","get","set","namespace","public","private","protected","internal","virtual","override","abstract","static","final","partial","transient","if","elif","else","raise","except","ensure","try","for","while","null","true","false","and","or","is","isa","not","in","as","do","break","continue","cast","import","from","goto","of","ref","self","super","typeof","yield","pass","return","char","string","int","callable","enum","struct","event","constructor","destructor")
  
  public def GetDefaultMacroList() as (string):
  	return ("print", "assert", "using")
  
  #endregion
  
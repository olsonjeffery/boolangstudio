namespace BooPegLexer

import System
import System.Collections.Generic
import Boo.Pegs

public class PegLexer(ILexer):
  
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
  public Line as string:
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
  	  if CurrentIndex >= _lineLength:
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
      token.Type = PegTokenType.EOL
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
    
    # here is where we deal with entering/exiting
    # ml comment regions and tripple quotes
    if token.Type == PegTokenType.MlCommentClose or token.Type == PegTokenType.TripleQuoteStringClose:  
      state = 0
    elif token.Type == PegTokenType.TripleQuoteStringOpen:
      state = 14
    elif token.Type == PegTokenType.MlCommentOpen:  
      state = 13
    
    # if nothing in the items above can parse it,
    # we'll just mark the rest of the line as unparsable
    # for now
    if result == false:
      token.StartIndex = _currentIndex unless state == 13 or state == 14
      token.EndIndex = _line.Length-1
      _currentIndex = _line.Length
    
    return result
    
  #endregion
  
  #region Logic related..
  
  public def GetContext(pegToken as PegToken) as PegLexerContext:
    ctx = PegLexerContext(RemainingLine,pegToken)
    ctx.Token.Type = PegTokenType.EOL
    ctx.Token.StartIndex = 0
    ctx.Token.EndIndex = 0
    return ctx
  
  public virtual def InMultiLineComment(pegToken as PegToken, ref state as int):
  	ctx = GetContext(pegToken)
  	if not ctx.Match(self.MlCommentClose):
  	  ctx.Token.Type = PegTokenType.MlComment
  	  ctx.Token.StartIndex = 0 # ?
  	  _currentIndex = Line.Length
  	  ctx.Token.EndIndex = _currentIndex-1
  	  return false
  	return true
  
  public virtual def InTrippleQuoteString(pegToken as PegToken, ref state as int):
  	ctx = GetContext(pegToken)
  	if not ctx.Match(self.TripleQuoteStringClose):
  	  ctx.Token.Type = PegTokenType.TripleQuoteString
  	  ctx.Token.StartIndex = 0 # ?
  	  _currentIndex = Line.Length
  	  ctx.Token.EndIndex = _currentIndex-1
  	  return false
  	return true
  
  public virtual def InGeneralLexingCase(pegToken as PegToken, ref state as int):
    ctx = GetContext(pegToken)
    return ctx.Match(self.BooTokenPeg)
  
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
    _currentIndex = ctx.Token.EndIndex + 1
    
  private BooTokenPeg as ChoiceExpression
  private MlCommentClose as PegRule
  private TripleQuoteStringClose as PegRule
  
  public def ResetKeywordsAndMacros():
    Keywords.Clear()
    Macros.Clear()
    
    Keywords.AddRange(GetDefaultKeywordList())
    Macros.AddRange(GetDefaultMacroList())
  
  public def Initialize():
    peg:
      self.BooTokenPeg = (Comments / Words / Whitespace / NumericLiterals / Strings / MiscOperators / Delimiters)
      
      # comments
      Comments = [DoubleWhackLineComment,NumberSignLineComment,MlComment,MlCommentOpen]
      DoubleWhackLineComment = "//",--any(),{$HandlePegMatch(PegTokenType.DoubleWhackLineComment)}
      NumberSignLineComment = "#",--any(),{$HandlePegMatch(PegTokenType.DoubleWhackLineComment)}
      MlComment = "/*",--(not "*/",any()),"*/",{$HandlePegMatch(PegTokenType.MlComment)}
      MlCommentOpen = "/*",--(not "*/",any()),{$HandlePegMatch(PegTokenType.MlCommentOpen)}
      
      self.MlCommentClose = [Mlc]
      Mlc = --(not "*/",any()),"*/",{$HandlePegMatch(PegTokenType.MlCommentClose)}
      
      # words
      
      Words = [Keyword,Identifier,Macro]
      Keyword = ++[a-z], IsKeyword,{$HandlePegMatch(PegTokenType.Keyword)}
      Identifier = [a-z,A-Z,'_'],--[a-z,A-Z,'_',0-9], not IsKeyword,not IsMacro,{$HandlePegMatch(PegTokenType.Identifier)}
      Macro = ++[a-z], IsMacro,{$HandlePegMatch(PegTokenType.Macro)}
      
      # whitespace
      Whitespace = ++whitespace(),{$HandlePegMatch(PegTokenType.Whitespace)}
      
      # numeric literals
      NumericLiterals = [FloatLiteral,IntegerLiteral]
      FloatLiteral = --'-',++[0-9],'.',++[0-9],{$HandlePegMatch(PegTokenType.FloatLiteral)}
      IntegerLiteral = --'-',++[0-9],{$HandlePegMatch(PegTokenType.IntegerLiteral)}
      
      # strings
      Strings = [SingleQuoteString,TripleQuoteString,TripleQuoteStringOpen,DoubleQuoteString]
      SingleQuoteString = "'",--SingleQuoteStringCharacter,"'",{$HandlePegMatch(PegTokenType.SingleQuoteString)}
      SingleQuoteStringCharacter = ("\\\\" / "\\'" / (not "'", any()))
      DoubleQuoteString = '"',--DoubleQuoteStringCharacter,'"',{$HandlePegMatch(PegTokenType.DoubleQuoteString)}
      DoubleQuoteStringCharacter = ("\\\\" / '\\"' / (not '"', any()))
      TripleQuoteString = '"""',--(not '"""',any()),'"""',{$HandlePegMatch(PegTokenType.TripleQuoteString)}
      TripleQuoteStringOpen = '"""',--(not '"""',any()),{$HandlePegMatch(PegTokenType.TripleQuoteStringOpen)}
      
      self.TripleQuoteStringClose = [Tqsc]
      Tqsc = --(not '"""',any()),'"""',{$HandlePegMatch(PegTokenType.TripleQuoteStringClose)}
      
      
      # misc operators
      MiscOperators = [AdditionSign,SubtractionSign,EqualsSign,Comma,DivisionSign,MultiplicationSign,Period,Splice]
      AdditionSign = ++'+',{$HandlePegMatch(PegTokenType.AdditionSign)}
      SubtractionSign = ++'-',{$HandlePegMatch(PegTokenType.SubtractionSign)}
      EqualsSign = ++'=',{$HandlePegMatch(PegTokenType.EqualsSign)}
      Comma = ',',{$HandlePegMatch(PegTokenType.Comma)}
      DivisionSign = '/',{$HandlePegMatch(PegTokenType.DivisionSign)}
      MultiplicationSign = '*',{$HandlePegMatch(PegTokenType.MultiplicationSign)}
      Period = '.',{$HandlePegMatch(PegTokenType.Period)}
      Splice = '$',{$HandlePegMatch(PegTokenType.Splice)}
      
      # delimiters
      Delimiters = [LeftParen,RightParen,QqOpen,QqClose,LeftSquareBracket,RightSquareBracket,LeftCurlyBrace,RightCurlyBrace]
      LeftParen = '(',{$HandlePegMatch(PegTokenType.LeftParen)}
      RightParen = ')',{$HandlePegMatch(PegTokenType.RightParen)}
      QqOpen = '[|',{$HandlePegMatch(PegTokenType.QqOpen)}
      QqClose = '|]',{$HandlePegMatch(PegTokenType.QqClose)}
      LeftSquareBracket = '[',{$HandlePegMatch(PegTokenType.LeftSquareBracket)}
      RightSquareBracket = ']',{$HandlePegMatch(PegTokenType.RightSquareBracket)}
      LeftCurlyBrace = '{',{$HandlePegMatch(PegTokenType.LeftCurlyBrace)}
      RightCurlyBrace = '}',{$HandlePegMatch(PegTokenType.RightCurlyBrace)}
  
  # meant to be ran once on class setup...?
  public def Initialize(keywords as (string), macros as (string)):
    ResetKeywordsAndMacros()
    Keywords.AddRange(keywords)
    Keywords.AddRange(macros)
    Initialize()
    
      
  
  public def GetDefaultKeywordList() as (string):
  	return ("def","class","interface","get","set","namespace","public","private","protected","internal","virtual","override","abstract","static","final","partial","transient","if","unless","elif","else","raise","except","ensure","try","for","while","null","true","false","and","or","is","isa","not","in","as","do","break","continue","cast","import","from","goto","of","ref","self","super","typeof","yield","pass","return","char","string","int","callable","enum","struct","event","constructor","destructor")
  
  public def GetDefaultMacroList() as (string):
  	return ("print", "assert", "using")
  
  #endregion
  
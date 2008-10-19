using System.Collections.Generic;

namespace BooColorizerParser
{
  public interface ILexer
  {
    void Initialize(string[] strings, string[] strings1);
    bool NextToken(object token, ref int state);
    void SetSource(string line);
    List<string> Macros { get;}
    List<string> Keywords { get;}
    string Line { get;}
    string RemainingLine { get;}
    int CurrentIndex { get;}
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooColorizerParser
{
  public class BooTokenLexer : ILexer
  {
    private ColorizerParser _parser; 

    public BooTokenLexer()
    {
      _parser = new ColorizerParser();
    }

    public void Initialize(string[] strings, string[] strings1)
    {
      throw new System.NotImplementedException();
    }

    public bool NextToken(object token, ref int state)
    {
      throw new System.NotImplementedException();
    }

    public void SetSource(string line)
    {
      throw new System.NotImplementedException();
    }

    public List<string> Macros
    {
      get { throw new System.NotImplementedException(); }
    }

    public List<string> Keywords
    {
      get { throw new System.NotImplementedException(); }
    }

    public string Line
    {
      get { throw new System.NotImplementedException(); }
    }

    public string RemainingLine
    {
      get { throw new System.NotImplementedException(); }
    }

    public int CurrentIndex
    {
      get { throw new System.NotImplementedException(); }
    }
  }
}

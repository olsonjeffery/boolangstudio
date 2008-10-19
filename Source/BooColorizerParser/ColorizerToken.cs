using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooColorizerParser
{
  public class ColorizerToken
  {
    PegTokenType _type;
    private int _startIndex;
    private int _endIndex;

    public ColorizerToken()
    {
      SetupColorizerToken(PegTokenType.Whitespace, 0, 0);
    }

    public ColorizerToken(PegTokenType type, int startIndex, int endIndex)
    {
      SetupColorizerToken(type, startIndex, endIndex);
    }

    private void SetupColorizerToken(PegTokenType type, int startIndex, int endIndex)
    {
      _type = type;
      _startIndex = startIndex;
      _endIndex = endIndex;
    }

    public PegTokenType Type
    {
      get { return _type; }
    }

    public int StartIndex
    {
      get { return _startIndex; }
    }

    public int EndIndex
    {
      get { return _endIndex; }
    }
  }
}

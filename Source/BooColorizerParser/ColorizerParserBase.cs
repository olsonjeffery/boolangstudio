using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OMetaSharp;

namespace BooColorizerParser
{
  public class ColorizerParserBase<T> : Parser<T>
  {
    public virtual void HandleMatch(OMetaList<HostExpression> result, PegTokenType type)
    {
    }
  }
}

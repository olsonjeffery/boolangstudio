using System;

namespace BooColorizerParserGenerator
{
  public class Program
  {
    public static void Main()
    {
      var grammarCodeGenerator = new OMetaGrammarCodeGenerator<BooColorizerParser.ColorizerParser>();
      var ometacsPath = Environment.CurrentDirectory + @"\..\..\..\BooColorizerParser\ColorizerParser.ometacs";
      var generatedParserPath = Environment.CurrentDirectory + @"\..\..\..\BooColorizerParser\ColorizerParser.cs";
      grammarCodeGenerator.CompileGrammarFromSourceAndOutputToFile(ometacsPath, generatedParserPath);
    }
  }
}
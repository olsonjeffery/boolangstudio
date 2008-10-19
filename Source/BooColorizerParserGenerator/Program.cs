namespace BooColorizerParserGenerator
{
  public class Program
  {
    public static void Main()
    {
      var grammarCodeGenerator = new OMetaGrammarCodeGenerator();
      grammarCodeGenerator.CompileGrammarFromSourceAndOutputToFile();
    }
  }
}
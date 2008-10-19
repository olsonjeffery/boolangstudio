using System;
using System.IO;
using BooColorizerParser;
using OMetaSharp;
using OMetaSharp.OMetaCS;

namespace BooColorizerParserGenerator
{
  public class OMetaGrammarCodeGenerator : OMetaCSConsoleProgram<BooColorizerParserGenerated>
  {
    public void CompileGrammarFromSourceAndOutputToFile()
    {
      // We want to make the default case simple and have it
      // "magically" just work.   
      var ometacsPath = Environment.CurrentDirectory + @"\..\..\..\BooColorizerParser\BooColorizerParser.ometacs";
      var generatedParserPath = Environment.CurrentDirectory + @"\..\..\..\BooColorizerParser\BooColorizerParserGenerated.cs";
      var generatedCode = 
        Grammars.ParseGrammarThenOptimizeThenTranslate<OMetaParser, OMetaOptimizer, OMetaTranslator>
          (File.ReadAllText(ometacsPath),
          parser => parser.Grammar,
          translator => translator.OptimizeGrammar,
          optimizer => optimizer.Trans
          );
      Grammars.WriteGeneratedCode(generatedCode, generatedParserPath);
    }
  }
}
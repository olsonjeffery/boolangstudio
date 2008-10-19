using System;
using System.IO;
using BooColorizerParser;
using OMetaSharp;
using OMetaSharp.OMetaCS;

namespace BooColorizerParserGenerator
{
  public class OMetaGrammarCodeGenerator<TGrammar>  : OMetaCSConsoleProgram<TGrammar> where TGrammar : OMeta<char>, new()
  {
    public void CompileGrammarFromSourceAndOutputToFile(string ometacsPath, string generatedClassPath)
    {
      // We want to make the default case simple and have it
      // "magically" just work.   
      var generatedCode = 
        Grammars.ParseGrammarThenOptimizeThenTranslate<OMetaParser, OMetaOptimizer, OMetaTranslator>
          (File.ReadAllText(ometacsPath),
          parser => parser.Grammar,
          translator => translator.OptimizeGrammar,
          optimizer => optimizer.Trans
          );
      Grammars.WriteGeneratedCode(generatedCode, generatedClassPath);
    }
  }
}
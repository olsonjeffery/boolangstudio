using System.IO;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using BooLangService;

namespace Boo.BooLangService.Document
{
    public class BooDocumentCompiler
    {
        private readonly BooCompiler compiler = new BooCompiler();
        private readonly BooDocumentVisitor visitor = new BooDocumentVisitor();

        public BooDocumentCompiler()
        {
            compiler.Parameters.OutputWriter = new StringWriter();
            compiler.Parameters.Pipeline = new ResolveExpressions();
            compiler.Parameters.Pipeline.BreakOnErrors = false;
            compiler.Parameters.Pipeline.Add(visitor);
        }

        public CompiledDocument Compile(string filename, string source)
        {
            compiler.Parameters.Input.Add(new StringInput(filename, source));
            
            compiler.Run();

            return new CompiledDocument(visitor.Document);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Compiler.TypeSystem;
using BooLangService;

namespace Boo.BooLangService.Document
{
    public class BooDocumentCompiler
    {
        public CompiledDocument Compile(string filename, string source)
        {
            BooDocumentVisitor visitor = CompileDocument(filename, source);

            return new CompiledDocument(visitor.Document, visitor.ImportedNamespaces, source);
        }

        private BooDocumentVisitor CompileDocument(string filename, string source)
        {
            var visitor = new BooDocumentVisitor();
            BooCompiler compiler = CreateCompiler(visitor);

            compiler.Parameters.Input.Add(new StringInput(filename, source));
            compiler.Run();

            return visitor;
        }

        private BooCompiler CreateCompiler(BooDocumentVisitor visitor)
        {
            var compiler = new BooCompiler();

            compiler.Parameters.OutputWriter = new StringWriter();
            compiler.Parameters.Pipeline = new ResolveExpressions();
            compiler.Parameters.Pipeline.BreakOnErrors = false;
            compiler.Parameters.Pipeline.Add(visitor);

            return compiler;
        }
    }
}
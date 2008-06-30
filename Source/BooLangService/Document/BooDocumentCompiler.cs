using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Compiler.TypeSystem;
using BooLangService;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangService.Document
{
    public class BooDocumentCompiler
    {
        public CompiledDocument Compile(string filename, string source)
        {
            return Compile(filename, source, null);
        }

        public CompiledDocument Compile(string filename, string source, IList<Assembly> references)
        {
            BooDocumentVisitor visitor = CompileDocument(filename, source, references);

            return new CompiledDocument(
                visitor.Document,
                visitor.ImportedNamespaces,
                visitor.ReferencePoints,
                source
            );
        }

        private BooDocumentVisitor CompileDocument(string filename, string source, IList<Assembly> references)
        {
            var visitor = new BooDocumentVisitor();
            BooCompiler compiler = CreateCompiler(visitor);

            compiler.Parameters.Input.Add(new StringInput(filename, source));
            compiler.Parameters.References.Extend(references);
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
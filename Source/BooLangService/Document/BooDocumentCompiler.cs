using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// Compiles Boo documents into a format usable by the intellisense provider.
    /// </summary>
    public class BooDocumentCompiler
    {
        /// <summary>
        /// Compiles a Boo file into a CompiledDocument.
        /// </summary>
        /// <param name="filename">Name of the file to compile</param>
        /// <param name="source">Source code to compile</param>
        /// <returns>CompiledDocument from the source</returns>
        public CompiledDocument Compile(string filename, string source)
        {
            return Compile(filename, source, null);
        }

        /// <summary>
        /// Compiles a Boo file into a CompiledDocument, with a set of referenced assemblies.
        /// </summary>
        /// <param name="filename">Name of the file to compile</param>
        /// <param name="source">Source code to compile</param>
        /// <param name="references">Additional assemblies to be referenced by the compiler</param>
        /// <returns>CompiledDocument from the source</returns>
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
            compiler.Parameters.Pipeline = new ResolveExpressions { BreakOnErrors = false };
            compiler.Parameters.Pipeline.Add(visitor);

            return compiler;
        }
    }
}
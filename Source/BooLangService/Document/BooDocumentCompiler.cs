using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Boo.BooLangProject;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Compiler.Steps;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// Compiles Boo documents into a format usable by the intellisense provider.
    /// </summary>
    public class BooDocumentCompiler
    {
        private BooDocumentVisitor visitor;
        private BooCompiler compiler;
        private readonly List<Assembly> references = new List<Assembly>();

        public BooDocumentCompiler()
        {
            visitor = new BooDocumentVisitor();
            compiler = CreateCompiler(visitor);
        }

        /// <summary>
        /// Compiles a Boo file into a CompiledDocument.
        /// </summary>
        /// <returns>CompiledDocument from the source</returns>
        public CompiledProject Compile()
        {
            compiler.Run();

            return new CompiledProject(
                visitor.Project,
                references
            );
        }

        public void AddSource(string fileName, string source)
        {
            compiler.Parameters.Input.Add(new StringInput(fileName, source));
        }

        public void Reset()
        {
            visitor = new BooDocumentVisitor();
            compiler = CreateCompiler(visitor);
        }

        private BooCompiler CreateCompiler(BooDocumentVisitor visitor)
        {
            var newCompiler = compiler ?? new BooCompiler();

            newCompiler.Parameters.OutputWriter = new StringWriter();
            newCompiler.Parameters.Pipeline = new Compile { BreakOnErrors = false };
            newCompiler.Parameters.Pipeline.Add(visitor);

            return newCompiler;
        }

        public void AddReference(IReference reference)
        {
            var assembly = reference.GetAssembly();
            
            references.Add(assembly);
            
            compiler.Parameters.AddAssembly(assembly);
        }
    }
}
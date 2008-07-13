using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
            return Compile(null);
        }

        /// <summary>
        /// Compiles a Boo file into a CompiledDocument, with a set of referenced assemblies.
        /// </summary>
        /// <param name="references">Additional assemblies to be referenced by the compiler</param>
        /// <returns>CompiledDocument from the source</returns>
        public CompiledProject Compile(IList<Assembly> references)
        {
            if (references != null)
                compiler.Parameters.References.Extend(references);

            compiler.Run();

            return new CompiledProject(
                visitor.Project
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
    }

    public class IntellisenseResolveExpressions : Parse
    {
        public IntellisenseResolveExpressions()
        {
            Add(new InitializeTypeSystemServices());
            Add(new PreErrorChecking());

            Add(new MergePartialClasses());

            Add(new InitializeNameResolutionService());
            Add(new IntroduceGlobalNamespaces());

            Add(new BindTypeDefinitions());
            Add(new BindGenericParameters());
            Add(new BindNamespaces());
            Add(new BindBaseTypes());

            Add(new IntroduceModuleClasses());

            Add(new BindTypeDefinitions());
            Add(new BindGenericParameters());
            Add(new BindEnumMembers());
            Add(new BindBaseTypes());

            Add(new BindMethods());
            Add(new ResolveTypeReferences());
            Add(new BindTypeMembers());

            Add(new ProcessInheritedAbstractMembers());
            Add(new CheckMemberNames());

            Add(new ExpandProperties());
        }
    }
}
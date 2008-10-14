using System.Reflection;
using Boo.BooLangService;
using Boo.BooLangService.Document;
using Boo.BooLangService.Intellisense;
using Boo.BooLangStudioSpecs.Document;
using Boo.BooLangStudioSpecs.Intellisense.Stubs;
using Microsoft.VisualStudio.Package;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class CompilationOutput
    {
        private readonly CompiledProject project;
        private readonly CaretLocation caretLocation;
        private Assembly[] references;

        public CompilationOutput(CompiledProject project, CaretLocation caretLocation)
        {
            this.project = project;
            this.caretLocation = caretLocation;
        }

        public CompiledProject Project
        {
            get { return project; }
        }

        public CaretLocation CaretLocation
        {
            get { return caretLocation; }
        }

        public CompilationOutput SetReferences(params Assembly[] references)
        {
            this.references = references;
            
            return this;
        }

        public IntellisenseDeclarations GetDeclarations()
        {
            var finder = CreateFinder(references);

            if (!CaretLocation.IsValid)
                throw new CaretNotFoundException("Caret not found in any documents, test cannot continue.");

            return finder.Find(CaretLocation, ParseReason.None);
        }

        protected DeclarationFinder CreateFinder(params Assembly[] assemblies)
        {
            var lineView = new StubSource(CaretLocation.LineSource);

            return new DeclarationFinder(Project, lineView, CaretLocation.FileName);
        }
    }
}
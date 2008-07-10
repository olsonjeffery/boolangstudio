using System;
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
        private string[] references;

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

        public CompilationOutput SetReferences(params string[] references)
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

        protected DeclarationFinder CreateFinder(params string[] referencedNamespaces)
        {
            var lineView = new SimpleStubLineView(CaretLocation.LineSource);
            var referenceLookup = new SimpleStubProjectReferenceLookup();

            if (referencedNamespaces != null)
                referenceLookup.AddFakeNamespaces(referencedNamespaces);

            return new DeclarationFinder(Project, referenceLookup, lineView, CaretLocation.FileName);
        }
    }

    internal class CaretNotFoundException : Exception
    {
        public CaretNotFoundException(string message) : base(message)
        {}
    }
}
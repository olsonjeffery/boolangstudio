using Boo.BooLangService.Document;
using Boo.BooLangStudioSpecs.Document;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class CompilationOutput
    {
        private readonly CompiledProject project;
        private readonly CaretLocation caretLocation;

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
    }
}
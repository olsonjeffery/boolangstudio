using System.Collections.Generic;
using System.Text;
using Boo.BooLangService.Document;
using Boo.BooLangService.VSInterop;
using EnvDTE;
using VSLangProj;

namespace Boo.BooLangService.Document
{
    public class CompiledDocumentCache
    {
        private readonly IDictionary<string, CompiledDocument> documents = new Dictionary<string, CompiledDocument>();
        private readonly BooLangService service;

        public CompiledDocumentCache(BooLangService service)
        {
            this.service = service;
        }

        public CompiledDocument Get(string fileName, string content)
        {
            if (!IsUsable(fileName, content))
                CompileContent(fileName, content);

            UpdateReferences(fileName);

            return documents[fileName];
        }

        private void UpdateReferences(string fileName)
        {
            // get
            var document = documents[fileName];

            // get project and references
            var projects = new ProjectHierarchy(service);
            VSProject project = projects.GetContainingProject(fileName);

            
            // update references against compiled document
            // store
        }

        private void CompileContent(string fileName, string content)
        {
            var compiler = new BooDocumentCompiler();

            CompiledDocument document = compiler.Compile(fileName, content);

            documents[fileName] = document;
        }

        private bool IsUsable(string fileName, string content)
        {
            return documents.ContainsKey(fileName) && IsUnchanged(fileName, content);
        }

        private bool IsUnchanged(string fileName, string content)
        {
            return documents[fileName].Content == content;
        }
    }
}
using System.Collections.Generic;
using System.Text;
using Boo.BooLangService.Document;

namespace Boo.BooLangService.Document
{
    public class CompiledDocumentCache
    {
        private readonly IDictionary<string, CompiledDocument> documents = new Dictionary<string, CompiledDocument>();

        public CompiledDocument Get(string fileName, string content)
        {
            if (!IsUsable(fileName, content))
                CompileContent(fileName, content);

            return documents[fileName];
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
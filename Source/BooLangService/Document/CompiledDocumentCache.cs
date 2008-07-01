using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Boo.BooLangService.Document;
using Boo.BooLangService.VSInterop;
using BooLangService;
using VSLangProj;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// Cache for open documents. Compiled documents that haven't changed can
    /// be retrieved from here without needing a recompile.
    /// </summary>
    public class CompiledDocumentCache
    {
        private readonly IDictionary<string, CompiledDocument> documents = new Dictionary<string, CompiledDocument>();
        private readonly BooLangService service;

        public CompiledDocumentCache(BooLangService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets a CompiledDocument for a Boo source file, either compiling the source or
        /// retrieving it from the cache.
        /// </summary>
        /// <param name="fileName">Boo file name</param>
        /// <param name="content">Source code</param>
        /// <returns>CompiledDocument</returns>
        public CompiledDocument Get(string fileName, string content)
        {
            if (!AlreadyCompiled(fileName, content))
                CompileContent(fileName, content);

            return documents[fileName];
        }

        private void CompileContent(string fileName, string content)
        {
            var compiler = new BooDocumentCompiler();
            var references = GetReferencedAssemblies(fileName);

            documents[fileName] = compiler.Compile(fileName, content, references); ;
        }

        private IList<Assembly> GetReferencedAssemblies(string fileName)
        {
            var referencedAssemblies = new List<Assembly>();
            var projects = new ProjectHierarchy(service);
            var project = projects.GetContainingProject(fileName);

            foreach (Reference reference in project.References)
            {
                if (reference.SourceProject == null)
                {
                    // assembly reference
                    var assembly = AssemblyHelper.FindInCurrentAppDomainOrLoad(reference.Path);

                    if (assembly != null)
                        referencedAssemblies.Add(assembly);
                }
                else
                {
                    // project reference
                    // what a faff - get the output file path
                    string fullPath = reference.SourceProject.Properties.Item("FullPath").Value.ToString();
                    string outputPath = reference.SourceProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
                    string outputDir = Path.Combine(fullPath, outputPath);
                    string outputFileName = reference.SourceProject.Properties.Item("OutputFileName").Value.ToString();
                    string assemblyPath = Path.Combine(outputDir, outputFileName);
                    var assembly = AssemblyHelper.FindInCurrentAppDomainOrLoad(assemblyPath);

                    if (assembly != null)
                        referencedAssemblies.Add(assembly);
                }
            }

            return referencedAssemblies;
        }

        private bool AlreadyCompiled(string fileName, string content)
        {
            return documents.ContainsKey(fileName) && IsUnchanged(fileName, content);
        }

        private bool IsUnchanged(string fileName, string content)
        {
            return documents[fileName].Content == content;
        }
    }
}
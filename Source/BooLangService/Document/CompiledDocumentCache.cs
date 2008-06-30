using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Boo.BooLangService.Document;
using Boo.BooLangService.VSInterop;
using BooLangService;
using EnvDTE;
using Microsoft.VisualStudio.Package;
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

            var references = GetReferencedAssemblies(fileName);

            CompiledDocument document = compiler.Compile(fileName, content, references);

            documents[fileName] = document;
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
                    var assembly = AssemblyHelper.FindInCurrentAppDomainOrLoad(reference.Path);

                    if (assembly != null)
                        referencedAssemblies.Add(assembly);
                }
                else
                {
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
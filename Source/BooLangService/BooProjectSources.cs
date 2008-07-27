using System;
using System.Collections.Generic;
using System.IO;
using Boo.BooLangService.Document;
using BooLangService;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell.Interop;

namespace Boo.BooLangProject
{
    /// <summary>
    /// Represents a Boo project, in terms of source files and parsing.
    /// Acts as a facade over the boo compiler, to allow easier compilation of
    /// all the Boo files in a project.
    /// </summary>
    public class BooProjectSources
    {
        private static readonly IList<BooProjectSources> loadedProjects = new List<BooProjectSources>();
        private readonly List<SourceFile> files = new List<SourceFile>();
        private readonly List<IReference> references = new List<IReference>();
        private HierarchyListener hierarchyListener;
        private CompiledProject compiledProject;

        public void StartWatchingHierarchy(IVsHierarchy hierarchy)
        {
            hierarchyListener = new HierarchyListener(hierarchy);
            hierarchyListener.ItemAdded += hierarchyListener_ItemAdded;
            hierarchyListener.Complete += hierarchyListener_Completed;
            hierarchyListener.StartListening();
        }

        private void hierarchyListener_ItemAdded(object sender, HierarchyEventArgs e)
        {
            var file = new SourceFile();

            file.Path = e.FileName;
            file.Source = GetSource(e.FileName);

            files.Add(file);
        }

        private void hierarchyListener_Completed(object sender, EventArgs e)
        {
            // do the first time compile
            // this is done while the project is loading because the user is already waiting for
            // the project to finish loading, an extra second or two won't be noticable. However,
            // if the first compile is done when they trigger intellisense for the first time, then
            // it's very noticable.
            Compile();
        }

        /// <summary>
        /// Static collection of projects that've been loaded into the currently opened
        /// solution.
        /// </summary>
        public static IList<BooProjectSources> LoadedProjects
        {
            get { return loadedProjects; }
        }

        /// <summary>
        /// Finds a project in the loaded projects collection that contains the
        /// specified file.
        /// </summary>
        /// <param name="file">Path of a file to find in a project.</param>
        /// <returns>Project containing specified file.</returns>
        public static BooProjectSources Find(string file)
        {
            foreach (var project in LoadedProjects)
            {
                if (project.HasFile(file))
                    return project;
            }

            return null;
        }

        /// <summary>
        /// Gets the compiled output for the current project, compiling it if needed.
        /// </summary>
        /// <returns>Compiled project.</returns>
        public CompiledProject GetCompiledProject()
        {
            if (RequiresRecompilation)
                Compile();

            return compiledProject;
        }

        /// <summary>
        /// Updates the compiled project when a parse request is raised.
        /// </summary>
        /// <param name="request">Parse request that raised the update.</param>
        public void Update(ParseRequest request)
        {
            ReloadSources();

            if (HasDirtyFiles)
                ResetCompiledProject();
        }

        private bool HasDirtyFiles
        {
            get { return files.Exists(f => f.IsDirty); }
        }

        private void ReloadSources()
        {
            // TODO: This needs to take into account open but unsaved files
            foreach (var file in files)
            {
                var latestSource = GetSource(file.Path);

                if (latestSource == file.Source) continue;

                file.Source = latestSource;
                file.IsDirty = true;
            }
        }

        /// <summary>
        /// Checks whether a project contains the specified file.
        /// </summary>
        /// <param name="fileName">File name to find.</param>
        /// <returns>Whether a project contains the file.</returns>
        private bool HasFile(string fileName)
        {
            return files.Exists(e => e.Path == fileName);
        }

        /// <summary>
        /// Resets the compiled project, so it'll be recompiled on the next call.
        /// </summary>
        private void ResetCompiledProject()
        {
            compiledProject = null;
        }

        /// <summary>
        /// Iterates the files and compiles them en-masse.
        /// </summary>
        private void Compile()
        {
            var compiler = new BooDocumentCompiler();

            files.ForEach(f => compiler.AddSource(f.Path, f.Source));
            references.ForEach(r => compiler.AddReference(r));

            compiledProject = compiler.Compile();

            files.ForEach(f => f.IsDirty = false);
        }

        /// <summary>
        /// Gets the contents of a file.
        /// </summary>
        /// <param name="fileName">File to get the source of</param>
        /// <returns></returns>
        private string GetSource(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        /// <summary>
        /// Whether the project requires recompilation.
        /// </summary>
        private bool RequiresRecompilation
        {
            get { return compiledProject == null; }
        }

        public List<IReference> References
        {
            get { return references; }
        }
    }

    internal class SourceFile
    {
        public string Source { get; set; }
        public string Path { get; set; }
        public bool IsDirty { get; set; }
    }
}
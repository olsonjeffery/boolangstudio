using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Microsoft.Build.BuildEngine;
using System.Configuration;
using Microsoft.Win32;
using System.Reflection;
using System.IO;

namespace Boo.BooLangProject
{
    [Guid(GuidList.guidBooLangProjectFactoryClassString)]
    [ComVisible(true)]
    public class BooLangProjectFactory : ProjectFactory
    {
        private ProjectPackage package;
        public BooLangProjectFactory(Package package)
            : base(package)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\BooLangStudio");
            
            // The path should be in the registry, but if it's not then do your best by looking
            // at the location of the current assembly!
            string booBinPath = (string)(key != null ? 
                key.GetValue("BooBinPath") : 
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\Dependencies\boo\build\"));

            this.package = (ProjectPackage)package;
            this.BuildEngine.GlobalProperties["BoocToolPath"] = new BuildProperty("BoocToolPath", booBinPath);
            this.BuildEngine.GlobalProperties["BooBinPath"] = new BuildProperty("BooBinPath",booBinPath);
			this.BuildEngine.GlobalProperties["GenerateFullPaths"] = new BuildProperty("GenerateFullPaths", "True");
        }

        protected override ProjectNode CreateProject()
        {
            BooProjectNode project = new BooProjectNode(this.package);
            project.SetSite((IOleServiceProvider)
                (((IServiceProvider)this.Package).GetService(typeof(IOleServiceProvider)))
                );
            return project;
        }

        protected override string ProjectTypeGuids(string file)
        {
            return base.ProjectTypeGuids(file);
        }
    }
}
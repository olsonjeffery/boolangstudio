using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Microsoft.Build.BuildEngine;
using System.Configuration;
using Microsoft.Win32;

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

            string booBinPath = (string)Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\VisualStudio\9.0Exp\Configuration\Packages\{55663be2-a969-4279-82c5-a6f27936f4f7}").GetValue("BooBinPath");
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
using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Microsoft.Build.BuildEngine;
using System.Configuration;

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
            string path = System.Environment.CurrentDirectory;
            System.Configuration.Configuration config =
                ConfigurationManager.OpenExeConfiguration(path+@"\BooLangStudio.dll");
            AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");

            string boocToolPath = appSettings.Settings["BoocToolPath"].Value;
            string booBinPath = appSettings.Settings["BooBinPath"].Value;
            this.package = (ProjectPackage)package;
            this.BuildEngine.GlobalProperties["BoocToolPath"] = new BuildProperty("BoocToolPath",boocToolPath);
            this.BuildEngine.GlobalProperties["BooBinPath"] = new BuildProperty("BooBinPath",booBinPath);
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
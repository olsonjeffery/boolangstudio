using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Microsoft.Build.BuildEngine;
using System.Configuration;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

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
            this.package = (ProjectPackage)package;
            this.BuildEngine.GlobalProperties["GenerateFullPaths"] = new BuildProperty("GenerateFullPaths", "True");

            string booBinPath = "";

            RegistryKey booBinPathKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\BooLangStudio");
            if (booBinPathKey != null)
                booBinPath = booBinPathKey.GetValue("BooBinPath") as string;
            
            if(string.IsNullOrEmpty(booBinPath))
            {
                IVsOutputWindowPane generalOut = Package.GetOutputPane(
                    VSConstants.GUID_OutWindowGeneralPane,
                    "Error List");
                
                if(generalOut!=null)
                    generalOut.OutputStringThreadSafe(Resources.BooBinPathMissing);
            }

            this.BuildEngine.GlobalProperties["BooBinPath"] = new BuildProperty("BooBinPath", booBinPath);
        }

        protected override ProjectNode CreateProject()
        {
            var provider = (IServiceProvider)Package;

            BooProjectNode project = new BooProjectNode(package, (BooLangService.BooLangService)provider.GetService(typeof(BooLangService.BooLangService)));

            project.SetSite((IOleServiceProvider)provider.GetService(typeof(IOleServiceProvider)));

            return project;
        }

        protected override object PreCreateForOuter(IntPtr outerProjectIUnknown)
        {
            object ret = base.PreCreateForOuter(outerProjectIUnknown);

            return ret;
        }

        protected override string ProjectTypeGuids(string file)
        {
            return base.ProjectTypeGuids(file);
        }
    }
}
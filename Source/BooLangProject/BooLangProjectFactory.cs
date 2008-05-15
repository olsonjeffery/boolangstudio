using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Microsoft.Build.BuildEngine;

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
        }

        protected override ProjectNode CreateProject()
        {
            BooProjectNode project = new BooProjectNode(this.package);
            project.SetSite((IOleServiceProvider)
                (((IServiceProvider)this.Package).GetService(typeof(IOleServiceProvider)))
                );
            return project;
        }

        
    }
}
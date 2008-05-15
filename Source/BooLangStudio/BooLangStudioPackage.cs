// VsPkg.cs : Implementation of BooLangService
//

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Package;
using Boo.BooLangService;
using Boo.BooLangProject;

namespace Boo.BooLangStudio
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the registration utility (regpkg.exe) that this class needs
    // to be registered as package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // A Visual Studio component can be registered under different regitry roots; for instance
    // when you debug your package you want to register it in the experimental hive. This
    // attribute specifies the registry root to use if no one is provided to regpkg.exe with
    // the /root switch.
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\9.0")]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration(false, "#110", "#112", "1.0", IconResourceID = 400)]
    // In order be loaded inside Visual Studio in a machine that has not the VS SDK installed, 
    // package needs to have a valid load key (it can be requested at 
    // http://msdn.microsoft.com/vstudio/extend/). This attributes tells the shell that this 
    // package has a load key embedded in its resources.
    [ProvideLoadKey("Standard", "1.0", "BooLangService", "Boo", 1)]
    [ProvideService(typeof(Boo.BooLangService.BooLangService))]
    [ProvideLanguageExtension(typeof(Boo.BooLangService.BooLangService), Boo.BooLangService.BooLangService.LanguageExtension)]
    [ProvideLanguageService(typeof(Boo.BooLangService.BooLangService), Boo.BooLangService.BooLangService.LanguageName, 0,
        CodeSense=true,
        MatchBraces=true,
        EnableCommenting=true,
        ShowCompletion=false)]
    
    [ProvideProjectFactory(typeof(BooLangProjectFactory),
        "Boo",
        "Boo Project Files (*.booproj);*.booproj",
        "booproj",
        "booproj",
        //@"..\..\..\BooLangProject\Templates\Projects",
        @".\NullPath",
        LanguageVsTemplate="Boo",
        NewProjectRequireNewFolderVsTemplate=false
        )]
    [ProvideProjectItem(typeof(BooLangProjectFactory),
        "Boo",
        @".\NullPath",
        32)]
    [RegisterMsBuildTargets("Boo_0.8.1",
        @"e:\projects\BooStudio\Dependencies\boo\build\Boo.Microsoft.Build.targets"
        )]
    [Guid(GuidList.guidBooLangStudioPkgString)]
    public sealed class BooLangStudioPackage : ProjectPackage
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public BooLangStudioPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members
        private Boo.BooLangService.BooLangService _service;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            // registering project factory...
            this.RegisterProjectFactory(new BooLangProjectFactory(this));
            base.Initialize();

            
            _service = new Boo.BooLangService.BooLangService();
            _service.SetSite(this);
            IServiceContainer container = (IServiceContainer)this;
            container.AddService(typeof(Boo.BooLangService.BooLangService), _service, true);
        }
        #endregion

    }
}
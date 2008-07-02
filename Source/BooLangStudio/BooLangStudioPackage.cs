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
using Microsoft.VisualStudio;

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
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\9.0Exp")]
    [InstalledProductRegistration(true, "#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideLoadKey("Standard", "1.0", "BooLangStudio", "Boo", 113)]
    [ProvideService(typeof(Boo.BooLangService.BooLangService))]
    [ProvideLanguageExtension(
        typeof(Boo.BooLangService.BooLangService),
        Boo.BooLangService.BooLangService.LanguageExtension)]
    [ProvideLanguageService(typeof(Boo.BooLangService.BooLangService),
        Boo.BooLangService.BooLangService.LanguageName,
        111,
        CodeSense = true,
        DefaultToInsertSpaces = true,
        EnableCommenting = true,
        MatchBraces = true,
        ShowCompletion = true,
        ShowMatchingBrace = true)]
    [ProvideProjectFactory(typeof(BooLangProjectFactory),
        "Boo",
        "Boo Project Files (*.booproj);*.booproj",
        "booproj",
        "booproj",
        @".\NullPath",
        LanguageVsTemplate = "Boo",
        NewProjectRequireNewFolderVsTemplate = false
        )]
    [ProvideProjectItem(typeof(BooLangProjectFactory),
        "Boo",
        @".\\NullPath",
        32)]
    [SingleFileGeneratorSupportRegistrationAttribute(typeof(BooLangProjectFactory))]
    [RegisterMsBuildTargets("Boo_0.8.1",
        @".\Boo.Microsoft.Build.targets"
        )]
    [ProvideMenuResource(1000, 1)]
    [Guid(GuidList.guidBooLangStudioPkgString)]
    public sealed class BooLangStudioPackage : ProjectPackage, IVsInstalledProduct
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

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                //CommandID menuCommandOrganizeMembers = new CommandID(GuidList.guidBooLangServiceCmdSet, (int)PkgCmdIDList.cmdidExample);
                //MenuCommand menuItemOrganizeMembers = new MenuCommand(OrganizeMembersCallback, menuCommandOrganizeMembers);
                //mcs.AddCommand(menuItemOrganizeMembers);
            }
            
            _service = new Boo.BooLangService.BooLangService();
            _service.SetSite(this);
            IServiceContainer container = (IServiceContainer)this;
            container.AddService(typeof(Boo.BooLangService.BooLangService), _service, true);
        }
        #endregion

        #region MenuItem callbacks
        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void OrganizeMembersCallback(object sender, EventArgs e)
        {
        }
        #endregion

        #region IVsInstalledProduct Members
        /// <summary>
        /// This method is called during Devenv /Setup to get the bitmap to
        /// display on the splash screen for this package.
        /// </summary>
        /// <param name="pIdBmp">The resource id corresponding to the bitmap to display on the splash screen</param>
        /// <returns>HRESULT, indicating success or failure</returns>
        public int IdBmpSplash(out uint pIdBmp)
        {
            pIdBmp = 200;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// This method is called to get the icon that will be displayed in the
        /// Help About dialog when this package is selected.
        /// </summary>
        /// <param name="pIdIco">The resource id corresponding to the icon to display on the Help About dialog</param>
        /// <returns>HRESULT, indicating success or failure</returns>
        public int IdIcoLogoForAboutbox(out uint pIdIco)
        {
            pIdIco = 400;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// This methods provides the product official name, it will be
        /// displayed in the help about dialog.
        /// </summary>
        /// <param name="pbstrName">Out parameter to which to assign the product name</param>
        /// <returns>HRESULT, indicating success or failure</returns>
        public int OfficialName(out string pbstrName)
        {
            pbstrName = GetResourceString("110");
            return VSConstants.S_OK;
        }

        /// <summary>
        /// This methods provides the product description, it will be
        /// displayed in the help about dialog.
        /// </summary>
        /// <param name="pbstrProductDetails">Out parameter to which to assign the description of the package</param>
        /// <returns>HRESULT, indicating success or failure</returns>
        public int ProductDetails(out string pbstrProductDetails)
        {
            pbstrProductDetails = GetResourceString("112");
            return VSConstants.S_OK;
        }

        /// <summary>
        /// This methods provides the product version, it will be
        /// displayed in the help about dialog.
        /// </summary>
        /// <param name="pbstrPID">Out parameter to which to assign the version number</param>
        /// <returns>HRESULT, indicating success or failure</returns>
        public int ProductID(out string pbstrPID)
        {
            pbstrPID = GetResourceString("111");
            return VSConstants.S_OK;
        }

        /// <summary>
        /// This method loads a localized string based on the specified resource.
        /// </summary>
        /// <param name="resourceName">Resource to load</param>
        /// <returns>String loaded for the specified resource</returns>
        public string GetResourceString(string resourceName)
        {
            string resourceValue;
            IVsResourceManager resourceManager = (IVsResourceManager)GetService(typeof(SVsResourceManager));
            if (resourceManager == null)
            {
                throw new InvalidOperationException("Could not get SVsResourceManager service. Make sure the package is Sited before calling this method");
            }
            Guid packageGuid = this.GetType().GUID;
            int hr = resourceManager.LoadResourceString(ref packageGuid, -1, resourceName, out resourceValue);
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(hr);
            return resourceValue;
        }
        #endregion
    }
}
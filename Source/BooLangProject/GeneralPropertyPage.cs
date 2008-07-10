using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Package;


namespace Boo.BooLangProject
{
    internal enum GeneralPropertyPageTag
    {
        AssemblyName,
        OutputType,
        RootNamespace,
        StartupObject,
        ApplicationIcon,
        TargetPlatform,
        TargetPlatformLocation
    }

    [ComVisible(true), Guid(GuidList.guidBooProjectPropertyPageClassString)]
    public class GeneralPropertyPage : SettingsPage, EnvDTE80.IInternalExtenderProvider
    {
        #region fields
        private string assemblyName;
        private OutputType outputType;
        private string defaultNamespace;
        private string startupObject;
        private string applicationIcon;
        private PlatformType targetPlatform = PlatformType.v2;
        private string targetPlatformLocation;
        #endregion

        #region ctor
        
        public GeneralPropertyPage()
        {
            this.Name = "General";
        }

        #endregion

        #region overriden methods
        public override string GetClassName()
        {
            return this.GetType().FullName;
        }

        protected override void BindProperties()
        {
            if (this.ProjectMgr == null)
              return;			

            this.assemblyName = GetProjectProperty(GeneralPropertyPageTag.AssemblyName, true);
            string outputType = GetProjectProperty(GeneralPropertyPageTag.OutputType);

            if (outputType != null && outputType.Length > 0)
            {
              this.outputType = (OutputType)Enum.Parse(typeof(OutputType), outputType);				
            }

            this.defaultNamespace = GetProjectProperty(GeneralPropertyPageTag.RootNamespace);
            this.startupObject = GetProjectProperty(GeneralPropertyPageTag.StartupObject);
            this.applicationIcon = GetProjectProperty(GeneralPropertyPageTag.ApplicationIcon);

            string targetPlatform = GetProjectProperty(GeneralPropertyPageTag.TargetPlatform);

            if (targetPlatform != null && targetPlatform.Length > 0)
            {
              this.targetPlatform = (PlatformType)Enum.Parse(typeof(PlatformType), targetPlatform);				
            }

            this.targetPlatformLocation = GetProjectProperty(GeneralPropertyPageTag.TargetPlatformLocation);		
        }

        protected override int ApplyChanges()
        {
            if (!IsDirty)
              return VSConstants.S_OK;

            if (this.ProjectMgr == null)
              return VSConstants.E_INVALIDARG;

            ValidateProperties();

            this.ProjectMgr.SetProjectProperty(GeneralPropertyPageTag.AssemblyName.ToString(), this.assemblyName);			
            this.ProjectMgr.SetProjectProperty(GeneralPropertyPageTag.RootNamespace.ToString(), this.defaultNamespace);
            this.IsDirty = false;

            return VSConstants.S_OK;
        }

        #endregion

        #region exposed properties

        [SRCategoryAttribute(SR.Application)]
        [LocDisplayName(SR.AssemblyName)]
        [SRDescriptionAttribute(SR.AssemblyNameDescription)]
        public string AssemblyName
        {
            get { return this.assemblyName; }
            set { this.assemblyName = value; this.IsDirty = true; }
        }
        
        [SRCategoryAttribute(SR.Application)]
        [LocDisplayName(SR.DefaultNamespace)]
        [SRDescriptionAttribute(SR.DefaultNamespaceDescription)]
        public string DefaultNamespace
        {
            get { return this.defaultNamespace; }
            set { this.defaultNamespace = value; this.IsDirty = true; }
        }
        
        #endregion

        #region IInternalExtenderProvider Members

        bool EnvDTE80.IInternalExtenderProvider.CanExtend(string extenderCATID, string extenderName, object extendeeObject)
        {
            IVsHierarchy outerHierarchy = HierarchyNode.GetOuterHierarchy(this.ProjectMgr);
            if (outerHierarchy is EnvDTE80.IInternalExtenderProvider)
              return ((EnvDTE80.IInternalExtenderProvider)outerHierarchy).CanExtend(extenderCATID, extenderName, extendeeObject);
            return false;
        }

        object EnvDTE80.IInternalExtenderProvider.GetExtender(string extenderCATID, string extenderName, object extendeeObject, EnvDTE.IExtenderSite extenderSite, int cookie)
        {
            IVsHierarchy outerHierarchy = HierarchyNode.GetOuterHierarchy(this.ProjectMgr);
            if (outerHierarchy is EnvDTE80.IInternalExtenderProvider)
              return ((EnvDTE80.IInternalExtenderProvider)outerHierarchy).GetExtender(extenderCATID, extenderName, extendeeObject, extenderSite, cookie);
            return null;
        }

        object EnvDTE80.IInternalExtenderProvider.GetExtenderNames(string extenderCATID, object extendeeObject)
        {
            IVsHierarchy outerHierarchy = HierarchyNode.GetOuterHierarchy(this.ProjectMgr);
            if (outerHierarchy is EnvDTE80.IInternalExtenderProvider)
              return ((EnvDTE80.IInternalExtenderProvider)outerHierarchy).GetExtenderNames(extenderCATID, extendeeObject);
            return null;
        }

        #endregion

        #region helper methods

        private string GetProjectProperty(GeneralPropertyPageTag tag)
        {
            return GetProjectProperty(tag, false);
        }

        private string GetProjectProperty(GeneralPropertyPageTag tag, bool resetCache)
        {
            return this.ProjectMgr.GetProjectProperty(tag.ToString(), resetCache);
        }

        private void ValidateProperties()
        {
            ValidateRootNamespace();
        }

        private void ValidateRootNamespace()
        {
            String invalidChars = @"([/?:&\\*<>|#%!" + '\"' + "])";
            Regex invalidCharactersRegex = new Regex(invalidChars, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            if (invalidCharactersRegex.IsMatch(this.defaultNamespace))
            {
              throw new ArgumentException("Default Namespace:\nThe string for the default namespace must be a valid identifier");
            }
        }

        #endregion
    }
}

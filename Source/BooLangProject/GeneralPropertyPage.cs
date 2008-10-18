using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using SettingsPage=Microsoft.VisualStudio.Project.SettingsPage;


namespace Boo.BooLangProject
{
    internal enum GeneralPropertyPageTag
    {
        AssemblyName,        
        RootNamespace,        
        Ducky,
        WhiteSpaceAgnostic,
        OutputType,
    }

    [ComVisible(true), Guid(GuidList.guidBooProjectPropertyPageClassString)]
    public class GeneralPropertyPage : SettingsPage, EnvDTE80.IInternalExtenderProvider
    {
        #region fields
        private string assemblyName;        
        private string defaultNamespace;        
        private bool ducky;
        private bool whiteSpaceAgnostic;
        private Microsoft.VisualStudio.Project.OutputType outputType;
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
            this.defaultNamespace = GetProjectProperty(GeneralPropertyPageTag.RootNamespace);
            string outputType = GetProjectProperty(GeneralPropertyPageTag.OutputType);
            
            if (!string.IsNullOrEmpty(outputType))
            {
                try
                {
                    this.outputType = (Microsoft.VisualStudio.Project.OutputType)Enum.Parse(typeof(OutputType), outputType);
                }
                catch { }
            }
            
            bool.TryParse(GetProjectProperty(GeneralPropertyPageTag.Ducky), out this.ducky);
            bool.TryParse(GetProjectProperty(GeneralPropertyPageTag.WhiteSpaceAgnostic), out this.whiteSpaceAgnostic);
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
            
            this.ProjectMgr.SetProjectProperty(GeneralPropertyPageTag.Ducky.ToString(), this.ducky.ToString());
            this.ProjectMgr.SetProjectProperty(GeneralPropertyPageTag.WhiteSpaceAgnostic.ToString(), this.whiteSpaceAgnostic.ToString());
            this.ProjectMgr.SetProjectProperty(GeneralPropertyPageTag.OutputType.ToString(), this.outputType.ToString());
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

        [SRCategoryAttribute(SR.Application)]
        [LocDisplayName(SR.Ducky)]
        [SRDescriptionAttribute(SR.DuckyDescription)]
        public bool Ducky
        {
            get { return this.ducky; }
            set { this.ducky = value; this.IsDirty = true; }
        }

        [SRCategoryAttribute(SR.Application)]
        [LocDisplayName(SR.WhiteSpaceAgnostic)]
        [SRDescriptionAttribute(SR.WhiteSpaceAgnosticDescription)]
        public bool WhiteSpaceAgnostic
        {
            get { return this.whiteSpaceAgnostic; }
            set { this.whiteSpaceAgnostic = value; this.IsDirty = true; }
        }

        [SRCategoryAttribute(SR.Application)]
        [LocDisplayName(SR.OutputType)]
        [SRDescriptionAttribute(SR.OutputTypeDescription)]
        public Microsoft.VisualStudio.Project.OutputType OutputType
        {
            get { return this.outputType; }
            set { this.outputType = value; this.IsDirty = true; }
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

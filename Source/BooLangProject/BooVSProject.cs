using System;
using EnvDTE;
using Microsoft.VisualStudio.Package;
using VSLangProj;
using VSLangProj80;

namespace Boo.BooLangProject
{
    public class BooVSProject : VSProject2
    {
        private readonly BooProjectNode project;

        public BooVSProject(BooProjectNode project)
        {
            this.project = project;
        }

        public ProjectItem CreateWebReferencesFolder()
        {
            throw new NotImplementedException();
        }

        public ProjectItem AddWebReference(string bstrUrl)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void CopyProject(string bstrDestFolder, string bstrDestUNCPath, prjCopyProjectOption copyProjectOption,
                                string bstrUsername, string bstrPassword)
        {
            throw new NotImplementedException();
        }

        public void Exec(prjExecCommand command, int bSuppressUI, object varIn, out object pVarOut)
        {
            throw new NotImplementedException();
        }

        public void GenerateKeyPairFiles(string strPublicPrivateFile, string strPublicOnlyFile)
        {
            throw new NotImplementedException();
        }

        public string GetUniqueFilename(object pDispatch, string bstrRoot, string bstrDesiredExt)
        {
            throw new NotImplementedException();
        }

        public References References
        {
            get
            {
                var referenceContainer = project.GetReferenceContainer() as ReferenceContainerNode;

                if (referenceContainer != null)
                    return referenceContainer.Object as References;

                return null;
            }
        }

        public BuildManager BuildManager
        {
            get { throw new NotImplementedException(); }
        }

        public DTE DTE
        {
            get { throw new NotImplementedException(); }
        }

        public Project Project
        {
            get { throw new NotImplementedException(); }
        }

        public ProjectItem WebReferencesFolder
        {
            get { throw new NotImplementedException(); }
        }

        public string TemplatePath
        {
            get { throw new NotImplementedException(); }
        }

        public bool WorkOffline
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Imports Imports
        {
            get { throw new NotImplementedException(); }
        }

        public VSProjectEvents Events
        {
            get { throw new NotImplementedException(); }
        }

        public object PublishManager
        {
            get { throw new NotImplementedException(); }
        }

        public VSProjectEvents2 Events2
        {
            get { throw new NotImplementedException(); }
        }
    }
}
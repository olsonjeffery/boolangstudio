using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.Package;
using VSLangProj;

namespace Boo.BooLangService.VSInterop
{
    public class ProjectHierarchy
    {
        private readonly LanguageService language;

        public ProjectHierarchy(LanguageService language)
        {
            this.language = language;
        }

        public VSProject GetContainingProject(string filename)
        {
            DTE dte = (DTE)language.GetService(typeof(DTE));
            Project p = dte.Solution.FindProjectItem(filename).ContainingProject;
            VSProject proj = (VSProject)p.Object;

            return proj;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Boo.BooLangService.VSInterop;
using BooLangService;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Drawing;
using VSLangProj;

namespace Boo.BooLangService
{
    [ComVisible(true)]
    [Guid(GuidList.guidBooLangServiceClassString)]
    public class BooLangService : LanguageService
    {
        private readonly DocumentParser docParser = new DocumentParser();
        private const string ImportKeyword = "import";

        #region ctor
        public BooLangService()
            : base()
        {
            DefineColorableItems();
        }
        #endregion

        #region config crap

        public const string LanguageName = "Boo";
        public const string LanguageExtension = ".boo";

        private IScanner _scanner;
        private LanguagePreferences _languagePreferences;

        public override string GetFormatFilterList()
        {
            //throw new NotImplementedException();
            return "Boo File (*.boo) *.boo";
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            if (_languagePreferences == null)
            {
                _languagePreferences = new LanguagePreferences(this.Site, typeof(BooLangService).GUID, BooLangService.LanguageName);
                _languagePreferences.Init();
            }
            
            return _languagePreferences;
        }

        public override string Name
        {
            get { return LanguageName; }
        }

        #endregion

        #region code parsing stuff

        public override IScanner GetScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer)
        {
            if (_scanner == null)
            {
                //_scanner = new RegularExpressionScanner();
                _scanner = new BooScanner();
            }
            return _scanner;
        }

        public override Source CreateSource(IVsTextLines buffer)
        {
            return new BooSource(this, buffer, new Colorizer(this, buffer, GetScanner(buffer)));
        }

        /// <summary>
        /// EPIC!!!!
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override AuthoringScope ParseSource(ParseRequest req)
        {
            // parses the document, then returns the authoring scope that
            // provides the intellisense info.
            // I think this should be implemented elsewhere. Just have the
            // parsing happen here, then the scope should handle creating the
            // declarations.
            BooSource source = (BooSource)GetSource(req.View);
            string line = source.GetLineUptoPosition(req.Line, req.Col);

            if (line.TrimStart().StartsWith(ImportKeyword))
                return GetNamespaces(req, line);
            
            //if (IsMemberSelect(req))
            //    return docParser.GetMemberSelectScope(req, source);

            return null;
        }

        private AuthoringScope GetNamespaces(ParseRequest req, string line)
        {
            // get any namespace already written (i.e. "Boo.Lang.")
            string namespaceContinuation = line.Trim();
            namespaceContinuation = namespaceContinuation.Remove(0, ImportKeyword.Length).Trim();

            // get project references for the project that the current file is in
            ProjectHierarchy projects = new ProjectHierarchy(this);
            VSProject project = projects.GetContainingProject(req.FileName);
            IList<ProjectReference> references = projects.GetReferences(project);

            return docParser.GetNamespaceSelect(references, namespaceContinuation);
        }

        #endregion

        #region color stuff

        private Dictionary<int,Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem> _colorableItems = 
            new Dictionary<int,Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem>();

        public override int GetItemCount(out int count)
        {
            count = _colorableItems.Count;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public override int GetColorableItem(int index, out Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem item)
        {
            item = _colorableItems[index];
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        private void DefineColorableItems()
        {
            _colorableItems.Add((int)TokenColor.Comment,
                new Boo.BooLangService.ColorableItem("Comment",
                    COLORINDEX.CI_DARKGREEN,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Identifier,
                new Boo.BooLangService.ColorableItem("Identifier",
                    COLORINDEX.CI_SYSPLAINTEXT_FG,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Keyword,
                new Boo.BooLangService.ColorableItem("Keyword",
                    COLORINDEX.CI_MAGENTA,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Number,
                new Boo.BooLangService.ColorableItem("Number",
                    COLORINDEX.CI_BLUE,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.String,
                new Boo.BooLangService.ColorableItem("String",
                    COLORINDEX.CI_DARKGREEN,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Text,
                new Boo.BooLangService.ColorableItem("Text",
                    COLORINDEX.CI_SYSPLAINTEXT_FG,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
        }

       #endregion

    }
}

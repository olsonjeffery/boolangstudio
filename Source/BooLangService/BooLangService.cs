using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Boo.BooLangService.Document.Nodes;
using BooLangService;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using VSLangProj;
using BooPegLexer;

namespace Boo.BooLangService
{
    [ComVisible(true)]
    [Guid(GuidList.guidBooLangServiceClassString)]
    public class BooLangService : LanguageService
    {
        #region ctor
        public BooLangService()
            : base()
        {
            requestProcessor = new ParseRequestProcessor(this);
            DefineColorableItems();
        }

        #endregion

        #region config crap

        public const string LanguageName = "Boo";
        public const string LanguageExtension = ".boo";

        private IScanner _scanner;
        private LanguagePreferences _languagePreferences;
        private readonly ParseRequestProcessor requestProcessor;

        public override string GetFormatFilterList()
        {
            //throw new NotImplementedException();
            return "Boo File (*.boo) *.boo";
        }

        public override ViewFilter CreateViewFilter(CodeWindowManager mgr, IVsTextView newView)
        {
            return new BooViewFilter(mgr, newView);
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            if (_languagePreferences == null)
            {
                _languagePreferences = new LanguagePreferences(this.Site, typeof(BooLangService).GUID, BooLangService.LanguageName);
                _languagePreferences.Init();
                _languagePreferences.IndentStyle = IndentingStyle.Smart;
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
                _scanner = new BooScanner(new PegLexer());
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
            return requestProcessor.GetAuthoringScopeForRequest(req);
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

        #region debugging support

        public override int ValidateBreakpointLocation(IVsTextBuffer buffer, int line, int col, TextSpan[] pCodeSpan)
        {
            if (pCodeSpan != null)
            {
                pCodeSpan[0].iStartLine = line;
                pCodeSpan[0].iStartIndex = col;
                pCodeSpan[0].iEndLine = line;
                pCodeSpan[0].iEndIndex = col;
                if (buffer != null)
                {
                    int length;
                    buffer.GetLengthOfLine(line, out length);
                    pCodeSpan[0].iStartIndex = 0;
                    pCodeSpan[0].iEndIndex = length;
                }
                return Microsoft.VisualStudio.VSConstants.S_OK;
            }
            else
            {
                return Microsoft.VisualStudio.VSConstants.S_FALSE;
            }
        }

        #endregion
    }
}

using Boo.BooLangProject;
using Boo.BooLangService.Intellisense;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace BooLangService
{
    public class BooSource : Source, ISource
    {
        public BooSource(LanguageService service, IVsTextLines textLines, Colorizer colorizer)
            : base(service, textLines, colorizer)
        {}

        /// <summary>
        /// Gets all text on the specified line upto the column specified.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetTextUptoPosition(int line, int col)
        {
            TextSpan span = new TextSpan();

            span.iStartLine = line;
            span.iStartIndex = 0;
            span.iEndLine = line;
            span.iEndIndex = col;

            return GetText(span);
        }
        
        public int GetIndexOfNextNonWhitespaceChar(int line)
        {
            return ScanToNonWhitespaceChar(line);
        }

        public bool UseTabs
        {
            get { return LanguageService.Preferences.InsertTabs; }
        }

        public void SetText(int line, int endColumn, string newText)
        {
            SetText(line, 0, line, endColumn, newText);
        }
    }
}

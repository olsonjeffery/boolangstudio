using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;

namespace BooLangService
{
    class BooSource : Source
    {

        public BooSource(LanguageService service, IVsTextLines textLines, Colorizer colorizer)
            : base(service, textLines, colorizer)
        {
        }

        public override void OnCommand(IVsTextView textView, Microsoft.VisualStudio.VSConstants.VSStd2KCmdID command, char ch)
        {
            base.OnCommand(textView, command, ch);

            // need to handle a press of the enter key
            // when the last non-whitespace/comment character
            // on the line was a colon, so we can do indentation
            
        }

        /// <summary>
        /// Gets all text on the specified line upto the column specified.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string GetLineUptoPosition(int line, int col)
        {
            TextSpan span = new TextSpan();

            span.iStartLine = line;
            span.iStartIndex = 0;
            span.iEndLine = line;
            span.iEndIndex = col;

            return GetText(span);
        }
    }
}

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Boo.BooLangService.Intellisense;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace BooLangService
{
    public class BooSource : Source, ILineView
    {
        public BooSource(LanguageService service, IVsTextLines textLines, Colorizer colorizer)
            : base(service, textLines, colorizer)
        {}

        /// <summary>
        /// Occurs when the changes are committed to the document, typically on save, but more
        /// importantly on enter.
        /// </summary>
        /// <param name="reason">Reason why the changes were committed, i.e. Save.</param>
        /// <param name="changedArea">TextSpan of where the changes occurred.</param>
        public override void OnChangesCommitted(uint reason, TextSpan[] changedArea)
        {
            base.OnChangesCommitted(reason, changedArea);

            if (((ChangeCommitGestureFlags)reason & ChangeCommitGestureFlags.CCG_CARET_ON_NEW_BUFFER_LINE) == ChangeCommitGestureFlags.CCG_CARET_ON_NEW_BUFFER_LINE)
            {
                // there's been a newline put in by the user
                if (changedArea.Length > 0)
                {
                    // and its actually changed the doc
                    LineIndenter indenter = new LineIndenter(this);

                    // change the indentation, if necessary
                    indenter.ChangeIndentation(changedArea[0]);
                }
            }
        }

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
    }
}

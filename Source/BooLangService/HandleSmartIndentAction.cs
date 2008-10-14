using BooLangService;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangService
{
    public class HandleSmartIndentAction
    {
        private readonly IVsTextView view;
        private readonly ILineIndenter indenter;

        public HandleSmartIndentAction(ISource source, IVsTextView view)
        {
            this.view = view;
            this.indenter = new LineIndenter(source, view);
        }

        public HandleSmartIndentAction(IVsTextView view, ILineIndenter indenter)
        {
            this.view = view;
            this.indenter = indenter;
        }

        public bool Execute()
        {
            int line, col;

            view.GetCaretPos(out line, out col);

            indenter.SetIndentationForNextLine(line);

            return false;
        }
    }
}
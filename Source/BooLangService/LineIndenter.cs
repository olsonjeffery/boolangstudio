using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TextManager.Interop;

namespace BooLangService
{
    /// <summary>
    /// Checks the current users line and indents or dedents the next one.
    /// </summary>
    public class LineIndenter
    {
        // matches comments on the end of a line
        private readonly Regex removeCommentsRegExp = new Regex(@".*((?:#|\/\/|\/\*).*)$", RegexOptions.Compiled);
        // matches a dedenting code line (pass, return, return xxx, return if x = x, return xxx unless x = x, etc...)
        private readonly Regex dedentRegExp = new Regex(@"(?:return|pass)(?:[\t ]?[\w]*[\t ]?(?<exp>(?:if|unless)))?", RegexOptions.Compiled);
        private const string Indent = "  ";
        private readonly BooSource source;

        public LineIndenter(BooSource source)
        {
            this.source = source;
        }

        /// <summary>
        /// Changes the indentation of the current line based on the last
        /// bit of code typed.
        /// </summary>
        /// <param name="changedArea">Area of changed code</param>
        public void ChangeIndentation(TextSpan changedArea)
        {
            TextSpan area = changedArea;
            string text = source.GetText(area);
            string line = source.GetTextUptoPosition(area.iStartLine, area.iStartIndex);

            if (!line.EndsWith(text))
            {
                // if commit includes last bit of text typed, the line won't have it yet
                // so stick it on for our parsing sanity
                // ** probably not the best thing to be doing here, but we can't just
                // get the whole line, because we may be pressing return in the centre
                // of it
                line += text;
            }

            if (RequiresIndent(line))
            {
                source.SetText(area, text + Indent);    // add an indent to the end of the changed text
                MoveCaret(Indent.Length);               // move the caret to the new end of line
            }
            else if (RequiresDedent(line))
            {
                source.SetText(area, text.Remove(text.Length - Indent.Length)); // remove an indent
                MoveCaret(-Indent.Length);                                      // move caret in a bit
            }
        }

        /// <summary>
        /// Moves the caret a set number of columns.
        /// </summary>
        /// <remarks>
        /// Use negative numbers to move left.
        /// </remarks>
        /// <param name="amount">Number of columns to move.</param>
        private void MoveCaret(int amount)
        {
            IVsTextView view = source.LanguageService.GetPrimaryViewForSource(source);

            int caretLine = 0;
            int caretCol = 0;

            view.GetCaretPos(out caretLine, out caretCol);
            view.SetCaretPos(caretLine, caretCol + amount);
        }

        /// <summary>
        /// Determines whether the current line should have the newline indented.
        /// </summary>
        /// <param name="line">Current line</param>
        /// <returns>Whether newline line should be indented</returns>
        private bool RequiresIndent(string line)
        {
            line = PrepareLine(line); // cheat and make parsing easier

            return line.EndsWith(":");
        }

        private bool RequiresDedent(string line)
        {
            line = PrepareLine(line); // cheat and make parsing easier
            Match dedentMatch = dedentRegExp.Match(line);

            // my regex foo is not strong enough to do this in one, so match
            // a dedent line (i.e. return) but then check if it ends with an
            // expression (i.e. return "value" if x = 10). Only indent if
            // there isn't an expression
            if (dedentMatch.Success && !dedentMatch.Groups["exp"].Success)
                return true;

            return false;
        }

        private string PrepareLine(string line)
        {
            // remove comments, makes parsing easier
            Match commentMatch = removeCommentsRegExp.Match(line);

            if (commentMatch != null && commentMatch.Groups.Count >= 2)
                line = line.Remove(line.IndexOf(commentMatch.Groups[1].Value));

            // get rid of trailing whitespace
            line = line.TrimEnd();

            return line;
        }
    }
}
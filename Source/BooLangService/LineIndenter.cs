using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TextManager.Interop;

namespace BooLangService
{
    /// <summary>
    /// Checks the current users line and indents or dedents the next one.
    /// </summary>
    public class LineIndenter : ILineIndenter
    {
        // matches comments on the end of a line
        private readonly Regex removeCommentsRegExp = new Regex(@".*((?:#|\/\/|\/\*).*)$", RegexOptions.Compiled);
        // matches a dedenting code line (pass, return, return xxx, return if x = x, return xxx unless x = x, etc...)
        private readonly Regex dedentRegExp = new Regex(@"(?:return|pass)(?:[\t ]?[\w]*[\t ]?(?<exp>(?:if|unless)))?", RegexOptions.Compiled);
        private readonly ISource source;
        private readonly IVsTextView view;
        private readonly char IndentChar;

        public LineIndenter(ISource source, IVsTextView view)
        {
            this.source = source;
            this.view = view;

            IndentChar = source.UseTabs ? '\t' : ' ';
        }

        /// <summary>
        /// Sets the indentation for the next lineNumber.
        /// </summary>
        public void SetIndentationForNextLine(int lineNumber)
        {
            string previousLine = GetPreviousNonWhitespaceLine(lineNumber);
            int indentLevel = GetIndentLevel(previousLine);

            if (RequiresIndent(previousLine))
                indentLevel++;
            else if (RequiresDedent(previousLine) && indentLevel > 0)
                indentLevel--;

            var nextLine = "".PadRight(indentLevel, IndentChar);
            var firstChar = source.ScanToNonWhitespaceChar(lineNumber);

            source.SetText(lineNumber, firstChar, nextLine);
            view.PositionCaretForEditing(lineNumber, indentLevel);
        }

        private int GetIndentLevel(string line)
        {
            var count = 0;

            foreach (var c in line)
            {
                if (c != IndentChar)
                    break; // got to the text on the line

                count++;
            }

            return count;
        }

        private string GetPreviousNonWhitespaceLine(int startLine)
        {
            int prevLineIndex = startLine - 1;
            var lineText = source.GetLine(prevLineIndex);

            //searchig for not blank line
            while (prevLineIndex > 0 && string.IsNullOrEmpty(lineText))
            {
                prevLineIndex--;
                lineText = source.GetLine(prevLineIndex);
            }

            return lineText;
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
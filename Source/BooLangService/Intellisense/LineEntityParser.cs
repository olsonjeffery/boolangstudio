using System.Collections.Generic;

namespace Boo.BooLangService.Intellisense
{
    public class LineEntityParser
    {
        private const string InsideString = "in string";
        private const string InsideParentheses = "in paren";

        public Invocation[] GetEntityNames(string line)
        {
            var mode = new Stack<string>();
            var entities = new Queue<Invocation>();
            var startIndex = 0;

            line = line.Trim();

            for (var currentIndex = 0; currentIndex < line.Length; currentIndex++)
            {
                var currentChar = line[currentIndex];

                if (currentChar == ' ' && !Currently(InsideString, mode) && !Currently(InsideParentheses, mode))
                {
                    // have a space that isn't inside a string, which probably means the line
                    // started with a keyword (import or return). So we'll remove everything
                    // before the space and start again
                    return GetEntityNames(line.Substring(currentIndex));
                }

                if (currentChar == '(')
                {
                    mode.Push(InsideParentheses);
                    continue;
                }

                if (currentChar == ')' && Currently(InsideParentheses, mode))
                {
                    mode.Pop();
                    continue;
                }

                if (currentChar == '"' && Currently(InsideString, mode))
                {
                    mode.Pop();
                    continue;
                }

                if (currentChar == '"')
                {
                    mode.Push(InsideString);
                    continue;
                }

                // escaped character
                if (currentChar == '\\' && Currently(InsideString, mode))
                {
                    currentIndex += 1; // skip next (escaped) char
                    continue;
                }

                if (currentChar == '.' && mode.Count == 0)
                {
                    Enqueue(entities, line.Substring(startIndex, currentIndex - startIndex));
                    startIndex = currentIndex + 1;
                }
            }

            Enqueue(entities, line.Substring(startIndex));

            return entities.ToArray();
        }

        private bool Currently(string state, Stack<string> mode)
        {
            if (mode.Count == 0) return false;

            return mode.Peek() == state;
        }

        private void Enqueue(Queue<Invocation> entities, string line)
        {
            var invocation = new Invocation();

            if (line.Contains("("))
            {
                line = line.Substring(0, line.IndexOf('('));
                invocation.HadParentheses = true;
            }

            invocation.Name = line;

            entities.Enqueue(invocation);
        }
    }
}
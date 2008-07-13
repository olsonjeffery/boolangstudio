using System.Collections.Generic;

namespace Boo.BooLangService.Intellisense
{
    public class LineEntityParser
    {
        public Invocation[] GetEntityNames(string line)
        {
            var mode = new Stack<string>();
            var entities = new Queue<Invocation>();
            var startIndex = 0;

            line = line.Trim();

            for (var currentIndex = 0; currentIndex < line.Length; currentIndex++)
            {
                var currentChar = line[currentIndex];

                if (currentChar == '(')
                {
                    mode.Push("in paren");
                    continue;
                }

                if (currentChar == ')' && mode.Peek() == "in paren")
                {
                    mode.Pop();
                    continue;
                }

                if (currentChar == '"' && mode.Peek() == "in string")
                {
                    mode.Pop();
                    continue;
                }

                if (currentChar == '"')
                {
                    mode.Push("in string");
                    continue;
                }

                if (currentChar == '\\' && mode.Peek() == "in string")
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
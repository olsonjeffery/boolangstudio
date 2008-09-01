using System.Collections.Generic;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.StringParsing
{
    public class LineEntityParser
    {
        private const string InsideString = "in string";
        private const string InsideParentheses = "in paren";

        public Invocation[] GetEntityNames(string line)
        {
            var entities = new Queue<Invocation>();
            var stringWalker = new StringWalker();
            var startIndex = 0;

            line = line.Trim();

            foreach (var currentPosition in stringWalker.Iterate(line))
            {
                if (currentPosition.Character == ' ' && !stringWalker.StateIs(StringWalkerState.InsideString) && !stringWalker.StateIs(StringWalkerState.InsideParentheses))
                {
                    stringWalker.Abort();
                    return GetEntityNames(line.Substring(currentPosition.Index));
                }

                if (currentPosition.Character == '.' && stringWalker.HasNoState)
                {
                    Enqueue(entities, line.Substring(startIndex, currentPosition.Index - startIndex));
                    startIndex = currentPosition.Index + 1;
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
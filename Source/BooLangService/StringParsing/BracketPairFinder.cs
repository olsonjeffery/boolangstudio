using System.Collections.Generic;

namespace Boo.BooLangService.StringParsing
{
    public class BracketPairFinder
    {
        private readonly BracketPairType type;

        public BracketPairFinder(BracketPairType type)
        {
            this.type = type;
        }

        public BracketPairCollection FindPairs(string value)
        {
            var pairs = new BracketPairCollection();
            var stack = new Stack<BracketPair>();
            var walker = new ExcludingStringLiteralsStringWalker();

            foreach (var position in walker.Iterate(value))
            {
                if (position.Character == type.Left)
                {
                    // starting new pair
                    stack.Push(new BracketPair { LeftIndex = position.Index });
                }

                if (position.Character == type.Right)
                {
                    // ending pair
                    var pair = stack.Pop();

                    pair.RightIndex = position.Index;

                    pairs.Add(pair);
                }
            }

            return pairs;
        }
    }
}
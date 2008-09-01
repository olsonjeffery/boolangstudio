using System;

namespace Boo.BooLangService.StringParsing
{
    public class BracketPairType
    {
        public static readonly BracketPairType Round = new BracketPairType('(', ')');
        public static readonly BracketPairType Square = new BracketPairType('[', ']');
        public static readonly BracketPairType Curly = new BracketPairType('{', '}');

        public static BracketPairType FromChar(char @char)
        {
            if (@char == '(' || @char == ')') return Round;
            if (@char == '[' || @char == ']') return Square;
            if (@char == '{' || @char == '}') return Curly;

            throw new ArgumentException("Invalid character, cannot resolve to bracket type.", "char");
        }

        private readonly char left;
        private readonly char right;

        private BracketPairType(char left, char right)
        {
            this.left = left;
            this.right = right;
        }

        public char Left
        {
            get { return left; }
        }

        public char Right
        {
            get { return right; }
        }
    }
}
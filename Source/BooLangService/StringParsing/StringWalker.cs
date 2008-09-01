using System;
using System.Collections.Generic;

namespace Boo.BooLangService.StringParsing
{
    public class StringWalker
    {
        private const char LeftParenthesis = '(';
        private const char RightParenthesis = ')';
        private const char DoubleQuote = '"';
        private const char BackSlash = '\\';

        private bool abort;
        private Stack<StringWalkerState> state;

        public IEnumerable<StringPosition> Iterate(string value)
        {
            state = new Stack<StringWalkerState>();

            for (var currentIndex = 0; currentIndex < value.Length; currentIndex++)
            {
                if (abort) break;

                var currentChar = value[currentIndex];

                if (currentChar == LeftParenthesis)
                    state.Push(StringWalkerState.InsideParentheses);
                else if (currentChar == RightParenthesis && StateIs(StringWalkerState.InsideParentheses))
                    state.Pop();
                else if (currentChar == DoubleQuote && StateIs(StringWalkerState.InsideString))
                    state.Pop();
                else if (currentChar == DoubleQuote)
                    state.Push(StringWalkerState.InsideString);
                else if (currentChar == BackSlash && StateIs(StringWalkerState.InsideString))
                    currentIndex += 1; // skip next (escaped) char

                if (ShouldYield(currentChar))
                    yield return new StringPosition(currentChar, currentIndex);
            }
        }

        protected virtual bool ShouldYield(char currentChar)
        {
            return true;
        }

        public bool StateIs(StringWalkerState expectedState)
        {
            if (state.Count == 0) return false;

            return state.Peek() == expectedState;
        }

        public void Abort()
        {
            abort = true;
        }

        public bool HasNoState
        {
            get { return state == null || state.Count == 0; }
        }
    }
}
namespace Boo.BooLangService.StringParsing
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// What does this name even mean?! I walk a string, looking for a match to what you asked for that ISN'T inside a string declaration.
    /// 
    /// e.g. Find the next closing parenthesis, starting from the first opening paren.
    /// 
    /// MyFun(x, "some string ()");
    /// 
    /// This would find the last paren, NOT the one inside the string.
    /// </remarks>
    public class ExcludeStringMatcher
    {
        private readonly string source;
        private int startIndex;

        public ExcludeStringMatcher(string source)
        {
            this.source = source;
        }

        public int? FindNextIndex(char value)
        {
            var stringWalker = new StringWalker();
            var startingSource = source.Substring(startIndex);

            foreach (var position in stringWalker.Iterate(startingSource))
            {
                if (position.Character == value && !stringWalker.StateIs(StringWalkerState.InsideString))
                    return position.Index + startIndex;
            }

            return null;
        }

        public void SetStartIndex(int index)
        {
            startIndex = index;
        }
    }
}

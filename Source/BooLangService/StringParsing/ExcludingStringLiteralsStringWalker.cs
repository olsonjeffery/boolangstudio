namespace Boo.BooLangService.StringParsing
{
    public class ExcludingStringLiteralsStringWalker : StringWalker
    {
        protected override bool ShouldYield(char currentChar)
        {
            if (StateIs(StringWalkerState.InsideString)) return false;

            return base.ShouldYield(currentChar);
        }
    }
}
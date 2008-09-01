namespace Boo.BooLangService.StringParsing
{
    public class StringWalkerState
    {
        public static readonly StringWalkerState InsideString = new StringWalkerState("in string");
        public static readonly StringWalkerState InsideParentheses = new StringWalkerState("in paren");

        private readonly string state;

        private StringWalkerState(string state)
        {
            this.state = state;
        }

        public override bool Equals(object obj)
        {
            if (obj is StringWalkerState) return Equals((StringWalkerState)obj);

            return false;
        }

        public bool Equals(StringWalkerState otherState)
        {
            return state == otherState.state;
        }

        public override int GetHashCode()
        {
            return (state != null ? state.GetHashCode() : 0);
        }
    }
}
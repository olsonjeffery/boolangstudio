namespace Boo.BooLangService.StringParsing
{
    public class StringPosition
    {
        private readonly char character;
        private readonly int index;

        public StringPosition(char character, int index)
        {
            this.character = character;
            this.index = index;
        }

        public char Character
        {
            get { return character; }
        }

        public int Index
        {
            get { return index; }
        }
    }
}
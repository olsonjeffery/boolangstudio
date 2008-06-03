using Boo.BooLangService.Intellisense;

namespace Boo.BooLangStudioSpecs.Intellisense.Stubs
{
    internal class SimpleStubLineView : ILineView
    {
        private readonly string lineToReturn;

        public SimpleStubLineView(string lineToReturn)
        {
            this.lineToReturn = lineToReturn;
        }

        public string GetTextUptoPosition(int line, int col)
        {
            return lineToReturn;
        }
    }
}
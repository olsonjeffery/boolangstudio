using Boo.BooLangService.Intellisense;
using BooLangService;

namespace Boo.BooLangStudioSpecs.Intellisense.Stubs
{
    internal class StubSource : ISource
    {
        private readonly string lineToReturn;

        public StubSource(string lineToReturn)
        {
            this.lineToReturn = lineToReturn;
        }

        public void SetText(int line, int endColumn, string newText)
        {
            throw new System.NotImplementedException();
        }

        public string GetTextUptoPosition(int line, int col)
        {
            return lineToReturn;
        }

        public bool UseTabs
        {
            get { throw new System.NotImplementedException(); }
        }

        public string GetLine(int index)
        {
            return lineToReturn;
        }

        public int ScanToNonWhitespaceChar(int line)
        {
            throw new System.NotImplementedException();
        }
    }
}
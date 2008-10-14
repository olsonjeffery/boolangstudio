using Microsoft.VisualStudio.Package;

namespace BooLangService
{
    public interface ISource
    {
        bool UseTabs { get; }
        string GetLine(int index);
        int ScanToNonWhitespaceChar(int line);
        void SetText(int line, int endColumn, string newText);
        string GetTextUptoPosition(int line, int col);
    }
}
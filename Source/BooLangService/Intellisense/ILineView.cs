namespace Boo.BooLangService.Intellisense
{
    public interface ILineView
    {
        string GetTextUptoPosition(int line, int col);
    }
}
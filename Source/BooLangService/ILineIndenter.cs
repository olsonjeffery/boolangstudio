namespace BooLangService
{
    public interface ILineIndenter
    {
        /// <summary>
        /// Sets the indentation for the next lineNumber.
        /// </summary>
        void SetIndentationForNextLine(int lineNumber);
    }
}
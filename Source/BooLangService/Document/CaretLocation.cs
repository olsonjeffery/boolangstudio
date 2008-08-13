namespace Boo.BooLangStudioSpecs.Document
{
    public class CaretLocation
    {
        private readonly int? column;
        private readonly int? line;
        private readonly string fileName;
        private readonly string lineSource;

        public CaretLocation(int? column, int? line, string fileName, string lineSource)
        {
            this.column = column;
            this.lineSource = lineSource;
            this.line = line;
            this.fileName = fileName;
        }

        public int? Column
        {
            get { return column; }
        }

        public int? Line
        {
            get { return line; }
        }

        public string FileName
        {
            get { return fileName; }
        }

        public string LineSource
        {
            get { return lineSource; }
        }

        public bool IsValid
        {
            get { return !(LineSource == null || Line == null || Column == null); }
        }
    }
}
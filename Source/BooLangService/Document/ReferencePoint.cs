using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// Represents an entity in the parse tree that can trigger member select requests. Basically
    /// types and variables.
    /// </summary>
    public class ReferencePoint
    {
        public IEntity Entity { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string FileName { get; set; }
        public ReferenceType ReferenceType { get; set; }

        public bool WithinBounds(int line, int column)
        {
            return line == Line && (column >= Column && column <= Column + Entity.FullName.Length);
        }
    }
}
namespace Boo.BooLangService.Document.Nodes
{
    /// <summary>
    /// Defines a node that returns a value: a property, field, or method.
    /// </summary>
    public interface IReturnableNode
    {
        string ReturnType { get; }
    }
}
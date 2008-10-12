namespace Boo.BooLangService.Document.Nodes
{
    public class MethodParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public string GetIntellisenseDescription()
        {
            return Type + " " + Name;
        }
    }
}
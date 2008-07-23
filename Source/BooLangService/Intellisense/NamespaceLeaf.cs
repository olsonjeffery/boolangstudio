using System.Collections.Generic;

namespace Boo.BooLangService.Intellisense
{
    public class NamespaceLeaf
    {
        private readonly List<NamespaceLeaf> children = new List<NamespaceLeaf>();
        private readonly string name;

        public NamespaceLeaf(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }

        public List<NamespaceLeaf> Children
        {
            get { return children; }
        }
    }
}
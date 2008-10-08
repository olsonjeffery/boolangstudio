using System;
using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Document.Origins
{
    public class TypeSourceOrigin : ISourceOrigin
    {
        private readonly Type type;

        public TypeSourceOrigin(Type type)
        {
            this.type = type;
        }

        public string Name
        {
            get { return type.Name; }
        }

        public List<ISourceOrigin> GetMembers(bool isConstructor)
        {
            return null;
        }

        public IBooParseTreeNode ToTreeNode()
        {
            return new ClassTreeNode(this, type.FullName);
        }
    }
}
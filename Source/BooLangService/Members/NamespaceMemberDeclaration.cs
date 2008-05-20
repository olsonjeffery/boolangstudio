namespace Boo.BooLangService.Members
{
    public class NamespaceMemberDeclaration : IMemberDeclaration
    {
        private readonly string namespaceName;

        public NamespaceMemberDeclaration(string namespaceName)
        {
            this.namespaceName = namespaceName;
        }

        public string Name
        {
            get { return namespaceName; }
        }

        public string FullName
        {
            get { return Name; }
        }

        public string Description
        {
            get { return Name; }
        }

        public IntellisenseIcon Icon
        {
            get { return IntellisenseIcon.Namespace; }
        }
    }
}
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Intellisense;

namespace Boo.BooLangService.Intellisense
{
    public class Keywords
    {
        public void InjectIntoScope(IBooParseTreeNode scope)
        {
            if (scope == null) return;

            TypeKeywordResolver keywords = new TypeKeywordResolver();

            string[] words = keywords.GetForScope(scope);

            foreach (IBooParseTreeNode child in scope.Children)
            {
                keywords.GetForScope(child);
            }

            foreach (string word in words)
            {
                KeywordTreeNode treeNode = new KeywordTreeNode();

                treeNode.Name = word;

                scope.Children.Add(treeNode);
            }
        }
    }
}
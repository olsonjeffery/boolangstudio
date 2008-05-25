using Boo.BooLangService.Document;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Intellisense
{
    public class IntellisenseNodeList : BooParseTreeNodeList
    {
        public override void Add(IBooParseTreeNode item)
        {
            bool intellisenseVisible = AttributeHelper.Has<IntellisenseVisibleAttribute>(item.GetType());
            
            if (intellisenseVisible)
                base.Add(item);
        }
    }
}
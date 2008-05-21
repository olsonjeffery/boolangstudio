using System.Collections;
using System.Collections.Generic;

namespace Boo.BooLangService.Document.Nodes
{
    public class ParseTreeNodeSet : IList<IBooParseTreeNode>
    {
        private readonly List<IBooParseTreeNode> innerList = new List<IBooParseTreeNode>();
        private readonly IBooParseTreeNode parent;

        public ParseTreeNodeSet(IBooParseTreeNode parent)
        {
            this.parent = parent;
        }

        public int IndexOf(IBooParseTreeNode item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, IBooParseTreeNode item)
        {
            innerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            innerList.RemoveAt(index);
        }

        public IBooParseTreeNode this[int index]
        {
            get { return innerList[index]; }
            set { innerList[index] = value; }
        }

        public void Add(IBooParseTreeNode item)
        {
            item.Parent = parent;
            innerList.Add(item);
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public bool Contains(IBooParseTreeNode item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(IBooParseTreeNode[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public bool Remove(IBooParseTreeNode item)
        {
            return innerList.Remove(item);
        }

        public int Count
        {
            get { return innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        IEnumerator<IBooParseTreeNode> IEnumerable<IBooParseTreeNode>.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<IBooParseTreeNode>)this).GetEnumerator();
        }
    }
}
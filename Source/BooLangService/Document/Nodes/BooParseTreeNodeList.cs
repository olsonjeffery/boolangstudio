using System;
using System.Collections;
using System.Collections.Generic;

namespace Boo.BooLangService.Document.Nodes
{
    public class BooParseTreeNodeList : IList<IBooParseTreeNode>
    {
        private readonly List<IBooParseTreeNode> inner = new List<IBooParseTreeNode>();

        private bool IsValidForAdding(IBooParseTreeNode item)
        {
            if (item.Name.Contains("$")) return false;
            if (Contains(item)) return false;

            return true;
        }

        public int IndexOf(IBooParseTreeNode item)
        {
            return inner.IndexOf(item);
        }

        public IBooParseTreeNode Find(Predicate<IBooParseTreeNode> match)
        {
            return inner.Find(match);
        }

        public void Insert(int index, IBooParseTreeNode item)
        {
            if (IsValidForAdding(item))
                inner.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            inner.RemoveAt(index);
        }

        public IBooParseTreeNode this[int index]
        {
            get { return inner[index]; }
            set { inner[index] = value; }
        }

        public void AddRange(IList<IBooParseTreeNode> list)
        {
            foreach (IBooParseTreeNode node in list)
            {
                Add(node);
            }
        }

        public virtual void Add(IBooParseTreeNode item)
        {
            if (IsValidForAdding(item))
                inner.Add(item);
        }

        public void Clear()
        {
            inner.Clear();
        }

        public bool Contains(IBooParseTreeNode item)
        {
            // this is sub-optimal because we might have two items that
            // have the same name, class and namespace for example.
            return inner.Exists(delegate(IBooParseTreeNode x) {
                return x.Name == item.Name;
            });
        }

        public void CopyTo(IBooParseTreeNode[] array, int arrayIndex)
        {
            inner.CopyTo(array, arrayIndex);
        }

        public bool Remove(IBooParseTreeNode item)
        {
            return inner.Remove(item);
        }

        public int Count
        {
            get { return inner.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        IEnumerator<IBooParseTreeNode> IEnumerable<IBooParseTreeNode>.GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<IBooParseTreeNode>)this).GetEnumerator();
        }

        public void Sort()
        {
            inner.Sort((x, y) => x.Name.CompareTo(y.Name));
        }
    }
}
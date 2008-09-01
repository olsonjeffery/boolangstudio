using System.Collections;
using System.Collections.Generic;

namespace Boo.BooLangService.StringParsing
{
    public class BracketPairCollection : ICollection<BracketPair>
    {
        private readonly List<BracketPair> pairs = new List<BracketPair>();

        public IEnumerator<BracketPair> GetEnumerator()
        {
            return pairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(BracketPair item)
        {
            pairs.Add(item);
        }

        public void Clear()
        {
            pairs.Clear();
        }

        public bool Contains(BracketPair item)
        {
            return pairs.Contains(item);
        }

        public void CopyTo(BracketPair[] array, int arrayIndex)
        {
            pairs.CopyTo(array, arrayIndex);
        }

        public bool Remove(BracketPair item)
        {
            return pairs.Remove(item);
        }

        public int Count
        {
            get { return pairs.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public BracketPair FindLeftByIndex(int index)
        {
            foreach (var pair in pairs)
            {
                if (pair.LeftIndex == index)
                    return pair;
            }

            return null;
        }

        public BracketPair FindRightByIndex(int index)
        {
            foreach (var pair in pairs)
            {
                if (pair.RightIndex == index)
                    return pair;
            }

            return null;
        }

        public int? FindPartnerIndex(int index)
        {
            foreach (var pair in pairs)
            {
                if (pair.LeftIndex == index) return pair.RightIndex;
                if (pair.RightIndex == index) return pair.LeftIndex;
            }

            return null;
        }
    }
}
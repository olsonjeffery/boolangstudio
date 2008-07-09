using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Boo.BooLangProject
{
    internal class HierarchyListener : IVsHierarchyEvents
    {
        // friendlier named constants
        private const uint NullItem = VSConstants.VSITEMID_NIL;
        private const uint RootItem = VSConstants.VSITEMID_ROOT;
        private const int NextSibling = (int)__VSHPROPID.VSHPROPID_NextSibling;
        private const int FirstChild = (int)__VSHPROPID.VSHPROPID_FirstChild;
        private const int TypeGuid = (int)__VSHPROPID.VSHPROPID_TypeGuid;
        private const int HasEnumerationSideEffects = (int)__VSHPROPID.VSHPROPID_HasEnumerationSideEffects;
        private readonly Guid PhysicalFile = VSConstants.GUID_ItemType_PhysicalFile;
        private const int Ok = VSConstants.S_OK;

        private readonly IVsHierarchy hierarchy;

        public HierarchyListener(IVsHierarchy hierarchy)
        {
            this.hierarchy = hierarchy;
        }

        public void StartListening()
        {
            Scan(RootItem);
        }

        /// <summary>
        /// Scans an existing hierarchy, raising events for each file it encounters.
        /// </summary>
        /// <param name="id">Item to start the scan from.</param>
        private void Scan(uint id)
        {
            uint currentItem = id;

            while (currentItem != NullItem)
            {
                RaiseItemAddedEvent(currentItem);

                if (CanRecurseInto(currentItem))
                {
                    object child = GetFirstChild(currentItem);

                    if (child != null)
                        Scan(GetItemID(child));
                }

                currentItem = MoveNext(currentItem);
            }
        }

        /// <summary>
        /// Raises the ItemAdded event for the specified item, if it's appropriate to do so.
        /// </summary>
        /// <param name="item">Item to raise the event for.</param>
        private void RaiseItemAddedEvent(uint item)
        {
            string itemName;

            if (ItemAdded == null || !IsBooFile(item, out itemName)) return;

            ItemAdded(hierarchy, new HierarchyEventArgs(item, itemName));
        }

        /// <summary>
        /// Determines whether an item's children can be recursed into.
        /// </summary>
        /// <param name="item">Item whose children are to be recursed.</param>
        /// <returns></returns>
        private bool CanRecurseInto(uint item)
        {
            object propertyValue;
            int result = hierarchy.GetProperty(item, HasEnumerationSideEffects, out propertyValue);

            if (result == Ok && propertyValue is bool)
                return !(bool)propertyValue;
            
            return true;
        }

        /// <summary>
        /// Gets the first child of an item.
        /// </summary>
        /// <param name="item">Item to get the first child of.</param>
        /// <returns></returns>
        private object GetFirstChild(uint item)
        {
            object child;
            int result = hierarchy.GetProperty(item, FirstChild, out child);

            return result == Ok ? child : null;
        }

        /// <summary>
        /// Moves to the next sibling in the tree.
        /// </summary>
        /// <param name="currentItem">Current item.</param>
        /// <returns>Next item.</returns>
        private uint MoveNext(uint currentItem)
        {
            object sibling;
            int result = hierarchy.GetProperty(currentItem, NextSibling, out sibling);

            return result != Ok ? NullItem : GetItemID(sibling);
        }

        /// <summary>
        /// Checks if a file is a valid Boo document.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <param name="canonicalName">Name of the file.</param>
        /// <returns>If a file is a valid Boo document.</returns>
        private bool IsBooFile(uint item, out string canonicalName)
        {
            canonicalName = null;

            if (!IsFile(item)) return false;

            int result = hierarchy.GetCanonicalName(item, out canonicalName);

            return !ErrorHandler.Failed(result) && canonicalName.EndsWith(".boo"); // todo: use a file extension list
        }

        /// <summary>
        /// Checks if an item is a file.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>If item is a file.</returns>
        private bool IsFile(uint item)
        {
            Guid typeGuid;
            int result = hierarchy.GetGuidProperty(item, TypeGuid, out typeGuid);

            return !ErrorHandler.Failed(result) && typeGuid == PhysicalFile;
        }

        /// <summary>
        /// Gets the item ID of a variant object.
        /// </summary>
        /// <param name="variant">VARIANT holding an itemid.</param>
        /// <returns>Item Id of the concerned node</returns>
        static uint GetItemID(object variant)
        {
            if (variant == null) return NullItem;

            if (variant is int) return (uint)(int)variant;
            if (variant is uint) return (uint)variant;
            if (variant is short) return (uint)(short)variant;
            if (variant is ushort) return (ushort)variant;
            if (variant is long) return (uint)(long)variant;

            return NullItem;
        }


        #region IVsHierarchyEvents Members

        public int OnItemAdded(uint itemidParent, uint itemidSiblingPrev, uint itemidAdded)
        {
            RaiseItemAddedEvent(itemidAdded);

            return Ok;
        }

        public int OnItemsAppended(uint itemidParent)
        {
            throw new NotImplementedException();
        }

        public int OnItemDeleted(uint itemid)
        {
            throw new NotImplementedException();
        }

        public int OnPropertyChanged(uint itemid, int propid, uint flags)
        {
            throw new NotImplementedException();
        }

        public int OnInvalidateItems(uint itemidParent)
        {
            throw new NotImplementedException();
        }

        public int OnInvalidateIcon(IntPtr hicon)
        {
            throw new NotImplementedException();
        }

        #endregion

        public event EventHandler<HierarchyEventArgs> ItemAdded;
    }
}
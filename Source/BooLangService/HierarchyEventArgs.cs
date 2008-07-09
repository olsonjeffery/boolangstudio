using System;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangProject
{
    public class HierarchyEventArgs : EventArgs
    {
        private readonly string fileName;
        private readonly uint itemID;

        public HierarchyEventArgs(uint itemID, string fileName)
        {
            this.itemID = itemID;
            this.fileName = fileName;
        }

        public string FileName
        {
            get { return fileName; }
        }

        public uint ItemID
        {
            get { return itemID; }
        }

        public IVsTextLines TextBuffer { get; set; }
    }
}
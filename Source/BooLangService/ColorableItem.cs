/* Cribbed from ManagedBabel. Thank you very much, Microsoft */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangService
{
    public class ColorableItem : Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem
    {
        private string displayName;
        private COLORINDEX background;
        private COLORINDEX foreground;
        private uint fontFlags = (uint)FONTFLAGS.FF_DEFAULT;

        public ColorableItem(string displayName, COLORINDEX foreground, COLORINDEX background, bool bold, bool strikethrough)
        {
            this.displayName = displayName;
            this.background = background;
            this.foreground = foreground;

            if (bold)
                this.fontFlags = this.fontFlags | (uint)FONTFLAGS.FF_BOLD;
            if (strikethrough)
                this.fontFlags = this.fontFlags | (uint)FONTFLAGS.FF_STRIKETHROUGH;
        }

        #region IVsColorableItem Members
        public int GetDefaultColors(COLORINDEX[] piForeground, COLORINDEX[] piBackground)
        {
            if (null == piForeground)
            {
                throw new ArgumentNullException("piForeground");
            }
            if (0 == piForeground.Length)
            {
                throw new ArgumentOutOfRangeException("piForeground");
            }
            piForeground[0] = foreground;

            if (null == piBackground)
            {
                throw new ArgumentNullException("piBackground");
            }
            if (0 == piBackground.Length)
            {
                throw new ArgumentOutOfRangeException("piBackground");
            }
            piBackground[0] = background;

            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int GetDefaultFontFlags(out uint pdwFontFlags)
        {
            pdwFontFlags = this.fontFlags;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public int GetDisplayName(out string pbstrName)
        {
            pbstrName = displayName;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }
        #endregion
    }
}

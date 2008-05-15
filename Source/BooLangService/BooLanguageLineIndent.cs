using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextManager.Interop;

namespace BooLangService
{
    class BooLanguageLineIndent : IVsLanguageLineIndent
    {
        #region IVsLanguageLineIndent Members

        public int GetIndentPosition(IVsTextLayer pBaseLayer, int BaseBufferLineIndex, out int pIndentPosition)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

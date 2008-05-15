using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell.Interop;

namespace Boo.BooLangProject
{
    public class BooFileNode : FileNode
    {

        public BooFileNode(ProjectNode project, ProjectElement element, int imageIndex)
            : base(project, element)
        {
            _imageIndex = imageIndex;
        }

        internal static int _imageIndex;
        public override int ImageIndex
        {
            get
            {
                return _imageIndex + 0;
            }
        }

    }
}

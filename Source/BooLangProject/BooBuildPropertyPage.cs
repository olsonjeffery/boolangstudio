using System;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Package;
using Microsoft.Win32;

namespace Boo.BooLangProject
{
    [ComVisible(true), Guid(GuidList.guidBooProjectBuildPropertyPageClassString)]
    public class BooBuildPropertyPage : BuildPropertyPage
    {

    }
}

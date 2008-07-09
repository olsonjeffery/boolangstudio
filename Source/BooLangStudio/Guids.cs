// Guids.cs
// MUST match guids.h
using System;

namespace Boo.BooLangStudio
{
    static class GuidList
    {
        public const string guidBooLangStudioPkgString = "55663be2-a969-4279-82c5-a6f27936f4f7";
        public const string guidBooLangStudioCmdSetString = "0DE0ACD7-158C-4932-BC73-A6342B326BDB";
        
        public static readonly Guid guidBooLangServiceCmdSet = new Guid(guidBooLangStudioCmdSetString);
    };
}
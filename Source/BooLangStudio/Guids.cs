// Guids.cs
// MUST match guids.h
using System;

namespace Boo.BooLangStudio
{
    static class GuidList
    {
        public const string guidBooLangStudioPkgString = "55663be2-a969-4279-82c5-a6f27936f4f7";
        public const string guidBooLangStudioCmdSetString = "f8148522-4227-4c68-ab72-2abbcad0cfc0";
        
        public static readonly Guid guidBooLangServiceCmdSet = new Guid(guidBooLangStudioCmdSetString);
    };
}
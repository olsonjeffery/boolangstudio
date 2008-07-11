using System;
using System.Reflection;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.ComponentModel;
using System.Security.Permissions;

namespace Boo.BooLangProject
{
    internal sealed class SR
    {
        internal const string Application = "Application";
        internal const string AssemblyName = "AssemblyName";
        internal const string AssemblyNameDescription = "AssemblyNameDescription";
        internal const string DefaultNamespace = "DefaultNamespace";
        internal const string DefaultNamespaceDescription = "DefaultNamespaceDescription";
        internal const string ApplicationIcon = "ApplicationIcon";
        internal const string ApplicationIconDescription = "ApplicationIconDescription";
        internal const string StartupObject = "StartupObject";
        internal const string StartupObjectDescription = "StartupObjectDescription";
        internal const string Ducky = "Ducky";
        internal const string DuckyDescription = "DuckyDescription";
        internal const string WhiteSpaceAgnostic = "WhiteSpaceAgnostic";
        internal const string WhiteSpaceAgnosticDescription = "WhiteSpaceAgnosticDescription";
        
        static SR loader = null;
        ResourceManager resources;

        internal SR()
        {
            resources = new ResourceManager("Boo.BooLangProject.Resources", typeof(Resources).Assembly);
        }

        private static Object syncObject;
        private static Object SyncObject
        {
            get
            {
                if (syncObject == null)
                {
                    object obj = new object();
                    Interlocked.CompareExchange(ref syncObject, obj, null);
                }
                return syncObject;
            }
        }

        public static ResourceManager Resources
        {
            get
            {
                if (loader == null)
                {
                    lock (SyncObject)
                    {
	                    if (loader == null)
	                    {
		                    loader = new SR();
	                    }
                    }
                }

                return loader.resources;
            }
        }

        public static string GetString(string name, params object[] args)
        {
            string res = Resources.GetString(name, null);

            if (args != null && args.Length > 0)
            {
                return String.Format(CultureInfo.CurrentCulture, res, args);
            }
            else
            {
                return res;
            }
        }

        public static string GetString(string name)
        {
            return Resources.GetString(name, null);
        }

        public static object GetObject(string name)
        {
            return Resources.GetObject(name, null);
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class SRDescriptionAttribute : DescriptionAttribute
    {
        public SRDescriptionAttribute(string description) : base(description)
        {
        }

        public override string Description
        {
	        get	{ return SR.GetString(base.Description); }				
        }		
    }

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class SRCategoryAttribute : CategoryAttribute
    {
        public SRCategoryAttribute(string category)	: base(category)
        {
        }

        protected override string GetLocalizedString(string value)
        {
            return SR.GetString(value);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class LocDisplayNameAttribute : DisplayNameAttribute
    {
        private string name;

        public LocDisplayNameAttribute(string name)
        {
            this.name = name;
        }

        public override string DisplayName
        {
            get
            {
	            return SR.GetString(this.name);
            }
        }
    }
}

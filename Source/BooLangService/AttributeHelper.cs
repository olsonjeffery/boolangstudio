using System;

namespace Boo.BooLangService
{
    public class AttributeHelper
    {
        public static bool Has<T>(Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof (T), true).Length > 0;
        }
    }
}
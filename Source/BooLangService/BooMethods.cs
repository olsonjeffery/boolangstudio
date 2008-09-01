using System.Collections.Generic;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangService
{
    public class BooMethods : Methods
    {
        public BooMethods()
        {

        }

        public override string GetName(int index)
        {
            return "MyFakeName";
        }

        public override int GetCount()
        {
            return 1;
        }

        public override string GetDescription(int index)
        {
            return "MyFakeDescription";
        }

        public override string GetType(int index)
        {
            return "MyFakeType";
        }

        public override int GetParameterCount(int index)
        {
            return 1;
        }

        public override void GetParameterInfo(int index, int parameter, out string name, out string display, out string description)
        {
            name = "MyFakeParameterName";
            display = "MyFakeParameterDisplay";
            description = "MyFakeParameterDescription";
        }

        public TextSpan StartName { get; set; }
        public TextSpan StartParameters { get; set; }
        public IList<TextSpan> NextParameters { get; set; }
        public TextSpan EndParameters { get; set; }
    }
}
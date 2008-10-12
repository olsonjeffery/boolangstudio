using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Boo.BooLangService
{
    public class BooMethods : Methods
    {
        private readonly MethodTreeNode method;

        public BooMethods(MethodTreeNode method)
        {
            this.method = method;
        }

        public override string GetName(int index)
        {
            return GetMethodByIndex(index).Name;
        }

        public override int GetCount()
        {
            return method.Overloads.Count + 1; // overloads + default
        }

        public override string GetDescription(int index)
        {
            return GetMethodByIndex(index).GetIntellisenseDescription();
        }

        public override string GetType(int index)
        {
            return GetMethodByIndex(index).ReturnType;
        }

        public override int GetParameterCount(int index)
        {
            return GetMethodByIndex(index).Parameters.Count;
        }

        public override void GetParameterInfo(int index, int parameter, out string name, out string display, out string description)
        {
            var foundParam = GetMethodByIndex(index).Parameters[parameter];

            name = foundParam.Name;
            display = foundParam.GetIntellisenseDescription();
            description = foundParam.Name; // not sure what to show here yet
        }

        /// <summary>
        /// Get the method for an index. This covers up the way we structure methods, in that the first method is
        /// the default and it has a collection of overloads. In VS this is flat, so index 0 is actually our default
        /// and index 1 is our first overload.
        /// </summary>
        /// <param name="index"></param>
        private MethodTreeNode GetMethodByIndex(int index)
        {
            return index == 0 ? method : method.Overloads[index - 1];
        }

        public TextSpan StartName { get; set; }
        public TextSpan StartParameters { get; set; }
        public IList<TextSpan> NextParameters { get; set; }
        public TextSpan EndParameters { get; set; }
    }
}
using System.Collections.Generic;
using System.Text;
using Boo.BooLangService.Document.Origins;
using Boo.BooLangService.Intellisense;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document.Nodes
{
    [Scopable]
    public class MethodTreeNode : AbstractTreeNode, IReturnableNode
    {
        public MethodTreeNode(ISourceOrigin sourceOrigin, string returnType, string containingClass) : base(sourceOrigin)
        {
            ReturnType = returnType;
            ContainingClass = containingClass;
            Overloads = new List<MethodTreeNode>();
            Parameters = new List<MethodParameter>();
        }

        public string ReturnType { get; private set; }
        public string ContainingClass { get; private set; }
        public IList<MethodParameter> Parameters { get; set; }
        public IList<MethodTreeNode> Overloads { get; private set; }

        public override bool IntellisenseVisible
        {
            get { return true; }
        }

        public override string GetIntellisenseDescription()
        {
            return ReturnType + " " + ContainingClass + "." + Name + GetParametersIntellisenseDescription();
        }

        private string GetParametersIntellisenseDescription()
        {
            if (Parameters.Count == 0) return "()";

            var builder = new StringBuilder();

            builder.Append("(");

            foreach (var parameter in Parameters)
            {
                builder.Append(parameter.GetIntellisenseDescription());
                builder.Append(", ");
            }

            builder.Length -= 2;
            builder.Append(")");

            return builder.ToString();
        }
    }
}
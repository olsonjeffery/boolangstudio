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
        private readonly string returnType;
        private readonly string containingClass;
        private IList<MethodParameter> parameters = new List<MethodParameter>();

        public MethodTreeNode(ISourceOrigin sourceOrigin, string returnType, string containingClass) : base(sourceOrigin)
        {
            this.returnType = returnType;
            this.containingClass = containingClass;
        }

        public string ReturnType
        {
            get { return returnType; }
        }

        public string ContainingClass
        {
            get { return containingClass; }
        }

        public IList<MethodParameter> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public override string GetIntellisenseDescription()
        {
            return ReturnType + " " + ContainingClass + "." + Name + GetParametersIntellisenseDescription();
        }

        public override bool IntellisenseVisible
        {
            get { return true; }
        }

        private string GetParametersIntellisenseDescription()
        {
            if (Parameters.Count == 0) return "()";

            var builder = new StringBuilder();

            builder.Append("(");

            foreach (var parameter in Parameters)
            {
                builder.Append(parameter.Type);
                builder.Append(" ");
                builder.Append(parameter.Name);
                builder.Append(", ");
            }

            builder.Length -= 2;
            builder.Append(")");

            return builder.ToString();
        }
    }
}
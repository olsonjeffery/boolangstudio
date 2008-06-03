using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document.Nodes;
using MbUnit.Framework;
using Context = MbUnit.Framework.TestFixtureAttribute;
using Spec = MbUnit.Framework.TestAttribute;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [Context]
    public class WhenDisplayingDescriptionsForIntellisense
    {
        [Spec]
        public void ClassesArePrefixedWithClassAndUseFullNamespaceName()
        {
            var declarations = CreateDeclarations(new ClassTreeNode {Name = "MyClass", FullName = "Some.Namespace.MyClass"});

            string description = declarations.GetDescription(0);

            Assert.IsTrue(description == "Class Some.Namespace.MyClass",
                "Expected: 'Class Some.Namespace.MyClass'\r\nFound: '" + description + "'.");
        }

        [Spec]
        public void FieldsStartWithTypeFollowedByDeclaredName()
        {
            var declarations = CreateDeclarations(new LocalTreeNode { Name = "variable", ReturnType = "int" });

            string description = declarations.GetDescription(0);

            Assert.IsTrue(description == "int variable",
                "Expected: 'int variable'\r\nFound: '" + description + "'.");
        }

        private BooDeclarations CreateDeclarations(params IBooParseTreeNode[] nodes)
        {
            var declarations = new BooDeclarations();

            declarations.Add(new List<IBooParseTreeNode>(nodes));

            return declarations;
        }
    }
}
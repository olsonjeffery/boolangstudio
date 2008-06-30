using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document.Nodes;
using MbUnit.Framework;
using Microsoft.VisualStudio.Package;
using Context = MbUnit.Framework.TestFixtureAttribute;
using Spec = MbUnit.Framework.TestAttribute;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    [Context]
    public class WhenDisplayingDescriptionsForIntellisense : BaseDisplayIntellisenseContext
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

        [Spec]
        public void MethodsIncludeParameters()
        {
            var declarations = GetDeclarations(@"
class MyClass:
  def MyMethod(firstParameter as string, secondParameter as int):
    ~
    pass
");
            string description = GetDescriptionMatchingName(declarations, "MyMethod");

            Assert.IsTrue(description == "void MyClass.MyMethod(string firstParameter, int secondParameter)",
                "Expected: 'void MyClass.MyMethod(string firstParameter, int secondParameter)'\r\nFound: '" + description + "'.");
        }

        private string GetDescriptionMatchingName(Declarations declarations, string name)
        {
            int index;
            bool uniqueMatch;

            declarations.GetBestMatch(name, out index, out uniqueMatch);

            return declarations.GetDescription(index);
        }

        private IntellisenseDeclarations CreateDeclarations(params IBooParseTreeNode[] nodes)
        {
            var declarations = new IntellisenseDeclarations();

            declarations.Add(new List<IBooParseTreeNode>(nodes));

            return declarations;
        }
    }
}
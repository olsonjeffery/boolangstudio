using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document.Nodes;
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenDisplayingDescriptionsForIntellisense : BaseDisplayIntellisenseContext
    {
        [Fact]
        public void ClassesArePrefixedWithClassAndUseFullNamespaceName()
        {
            var declarations = CreateDeclarations(new ClassTreeNode {Name = "MyClass", FullName = "Some.Namespace.MyClass"});

            string description = declarations.GetDescription(0);

            Assert.True(description == "Class Some.Namespace.MyClass",
                "Expected: 'Class Some.Namespace.MyClass'\r\nFound: '" + description + "'.");
        }

        [Fact]
        public void FieldsStartWithTypeFollowedByDeclaredName()
        {
            var declarations = CreateDeclarations(new LocalTreeNode { Name = "variable", ReturnType = "int" });

            string description = declarations.GetDescription(0);

            Assert.True(description == "int variable",
                "Expected: 'int variable'\r\nFound: '" + description + "'.");
        }

        [Fact]
        public void MethodsIncludeParameters()
        {
            var declarations = GetDeclarations(@"
class MyClass:
  def MyMethod(firstParameter as string, secondParameter as int):
    ~
    pass
");
            string description = GetDescriptionMatchingName(declarations, "MyMethod");

            Assert.True(description == "void MyClass.MyMethod(string firstParameter, int secondParameter)",
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
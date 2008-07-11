using System;
using System.Collections.Generic;
using Boo.BooLangService;
using Boo.BooLangService.Document.Nodes;
using Microsoft.VisualStudio.Package;
using Xunit;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class WhenDisplayingDescriptionsForIntellisense : BaseCompilerContext
    {
        [Fact]
        public void ClassesArePrefixedWithClassAndUseFullNamespaceName()
        {
            var declarations = CompiledFixtures.GetDeclarations();
            var description = declarations.GetDescriptionMatchingName("DummyClass");

            Assert.Equal("Class Test.Project.DummyClass", description);
        }

        [Fact]
        public void InterfacesArePrefixedWithInterfaceAndUseFullNamespaceName()
        {
            var declarations = CompiledFixtures.GetDeclarations();
            var description = declarations.GetDescriptionMatchingName("IDummyInterface");

            Assert.Equal("Interface Test.Project.IDummyInterface", description);
        }

        [Fact]
        public void FieldsStartWithTypeFollowedByDeclaredName()
        {
            var declarations = CompiledFixtures.GetDeclarations();
            var description = declarations.GetDescriptionMatchingName("variable");

            Assert.Equal("int variable", description);
        }

        [Fact]
        public void MethodsIncludeParameters()
        {
            var declarations = CompiledFixtures.GetDeclarations();
            
            string description = declarations.GetDescriptionMatchingName("MyMethod");

            Assert.True(description == "void MyClass.MyMethod(string firstParameter, int secondParameter)",
                "Expected: 'void MyClass.MyMethod(string firstParameter, int secondParameter)'\r\nFound: '" + description + "'.");
        }

        private IntellisenseDeclarations CreateDeclarations(params IBooParseTreeNode[] nodes)
        {
            var declarations = new IntellisenseDeclarations();

            declarations.Add(new List<IBooParseTreeNode>(nodes));

            return declarations;
        }
    }
}
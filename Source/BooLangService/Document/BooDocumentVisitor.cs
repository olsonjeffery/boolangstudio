using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Boo.BooLangService.Document.Nodes;
using Boo.BooLangService.Document.Origins;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;
using Boo.Lang.Compiler.TypeSystem;
using Module=Boo.Lang.Compiler.Ast.Module;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// Visitor for building a tree of the source for use with intellisense.
    /// </summary>
    public class BooDocumentVisitor : AbstractTransformerCompilerStep
    {
        private readonly ProjectTreeNode project = new ProjectTreeNode();
        private IBooParseTreeNode currentScope;
        private DocumentTreeNode currentDocument;

        public IBooParseTreeNode Project
        {
            get { return project; }
        }

        public override void Run()
        {
            currentScope = project;

            Visit(CompileUnit);

            VisitReferences();

        }

        private void VisitReferences()
        {
            // get references and their types
            string[] topLevelNamespaces = GetTopLevelNamespacesFromReferences();

            // get INamespaces
            foreach (var ns in topLevelNamespaces)
            {
                INamespace namespaceEntity = NameResolutionService.GetNamespace(ns);

                project.ReferencedNamespaces[ns] = new ReferencedNamespaceTreeNode(new EntitySourceOrigin((IEntity)namespaceEntity));
            }
        }

        private string[] GetTopLevelNamespacesFromReferences()
        {
            var namespaces = new List<string>();

            foreach (Assembly reference in Context.References)
            {
                foreach (var type in reference.GetExportedTypes())
                {
                    if (type.Namespace == null) continue;

                    var ns = type.Namespace;

                    if (ns.Contains(".")) ns = ns.Substring(0, ns.IndexOf("."));

                    if (!namespaces.Contains(ns))
                        namespaces.Add(ns);
                }
            }

            return namespaces.ToArray();
        }


        public override bool EnterModule(Module node)
        {
            var document = new DocumentTreeNode(new EntitySourceOrigin(GetEntity(node)), node.LexicalInfo.FileName);

            Push(document, 0);
            currentDocument = document;

            return base.EnterModule(node);
        }

        public override void LeaveModule(Module node)
        {
            base.LeaveModule(node);

            // todo: de-nasty this
            var linesInFile = -1;

            if (node.LexicalInfo.FileName != null)
                linesInFile = File.ReadAllLines(node.LexicalInfo.FileName).Length;

            Pop(linesInFile);
            currentDocument = null;
        }

        public override void OnImport(Import node)
        {
            // this is a bit nasty - get all the members of the referenced namespace
            // then push them on the tree, so they're referencable
            var ns = (INamespace)TypeSystemServices.GetEntity(node);

            currentDocument.Imports[node.Namespace] = new ImportedNamespaceTreeNode(new EntitySourceOrigin((IEntity)ns));

            base.OnImport(node);
        }

        public override bool EnterInterfaceDefinition(InterfaceDefinition node)
        {
            Push(new InterfaceTreeNode(new EntitySourceOrigin(GetEntity(node)), node.FullName), node.LexicalInfo.Line);

            return base.EnterInterfaceDefinition(node);
        }

        public override void LeaveInterfaceDefinition(InterfaceDefinition node)
        {
            base.LeaveInterfaceDefinition(node);

            Pop(node.EndSourceLocation.Line);
        }

        public override bool EnterClassDefinition(ClassDefinition node)
        {
            Push(new ClassTreeNode(new EntitySourceOrigin(GetEntity(node)), node.FullName), node.LexicalInfo.Line);

            return base.EnterClassDefinition(node);
        }

        public override void OnField(Field node)
        {
            Push(new FieldTreeNode(new EntitySourceOrigin(GetEntity(node))), node.LexicalInfo.Line);

            base.OnField(node);

            Pop(node.LexicalInfo.Line);
        }

        public override void OnProperty(Property node)
        {
            Push(new PropertyTreeNode(new EntitySourceOrigin(GetEntity(node))), node.LexicalInfo.Line);

            base.OnProperty(node);

            Pop(node.EndSourceLocation.Line);
        }

        public override bool EnterConstructor(Constructor node)
        {
            base.EnterConstructor(node);

            return EnterMethod(node);
        }

        public override bool EnterMethod(Method node)
        {
            var parameters = new List<MethodParameter>();

            // add parameters... 
            // TODO: Clean Me
            foreach (var parameter in node.Parameters)
            {
                parameters.Add(new MethodParameter
                {
                    Name = parameter.Name,
                    Type = parameter.Type.ToString()
                });
            }

            //           method in class                   ?? method in property, in class
            var parent = node.ParentNode as TypeDefinition ?? node.ParentNode.ParentNode as TypeDefinition;
            MethodTreeNode method = null;

            // find if the method has already been declared (if so, we're an overload)
            foreach (var child in currentScope.Children)
            {
                if (child.Name == node.Name)
                {
                    method = (MethodTreeNode)child;
                }
            }

            if (method == null)
            {
                // new method
                method = new MethodTreeNode(new EntitySourceOrigin(GetEntity(node)), node.ReturnType != null ? node.ReturnType.ToString() : "void", parent != null ? parent.Name : "");
                method.Parameters = parameters;

                Push(method, node.LexicalInfo.Line);
            }
            else
            {
                // method overload
                var overload = new MethodTreeNode(new EntitySourceOrigin(GetEntity(node)), node.ReturnType != null ? node.ReturnType.ToString() : "void", parent != null ? parent.Name : "");
                overload.Parameters = parameters;

                method.Overloads.Add(overload);

                Push(overload, node.LexicalInfo.Line);
            }

            return base.EnterMethod(node);
        }

        public override bool EnterTryStatement(TryStatement node)
        {
            Push(new TryTreeNode(new EntitySourceOrigin(GetEntity(node))), node.LexicalInfo.Line);

            return base.EnterTryStatement(node);
        }

        public override void LeaveTryStatement(TryStatement node)
        {
            base.LeaveTryStatement(node);

            Pop(node.ProtectedBlock.EndSourceLocation.Line);
        }

        public override void OnLocal(Local node)
        {
            var local = (ITypedEntity)TypeSystemServices.GetEntity(node);

            Push(new LocalTreeNode(new EntitySourceOrigin(GetEntity(node))) { ReturnType = local.Type.ToString() }, node.LexicalInfo.Line);

            base.OnLocal(node);

            Pop(node.LexicalInfo.Line);
        }

        public override void LeaveMethod(Method node)
        {
            base.LeaveMethod(node);

            Pop(node.Body.EndSourceLocation.Line);
        }

        public override void LeaveConstructor(Constructor node)
        {
            base.LeaveConstructor(node);

            LeaveMethod(node);
        }

        public override void LeaveClassDefinition(ClassDefinition node)
        {
            base.LeaveClassDefinition(node);

            Pop(node.EndSourceLocation.Line);
        }

        public override void OnReferenceExpression(ReferenceExpression node)
        {
            base.OnReferenceExpression(node);
        }

        public override void OnNamespaceDeclaration(NamespaceDeclaration node)
        {
            base.OnNamespaceDeclaration(node);
        }

        private void Push(IBooParseTreeNode node, int line)
        {
            node.Parent = currentScope;
            node.StartLine = line;

            currentScope.Children.Add(node);
            currentScope = node;
        }

        private void Pop(int endLine)
        {
            currentScope.EndLine = endLine;
            currentScope = currentScope.Parent;
        }

        public override Node VisitNode(Node node)
        {
            // handy for breaking into
            return base.VisitNode(node);
        }
    }
}
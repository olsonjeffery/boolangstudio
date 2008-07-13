using System;
using System.Collections.Generic;
using System.IO;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// Visitor for building a tree of the source for use with intellisense.
    /// </summary>
    public class BooDocumentVisitor : AbstractTransformerCompilerStep
    {
        private readonly IBooParseTreeNode project = new ProjectTreeNode();
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
        }

        public override bool EnterModule(Module node)
        {
            var document = new DocumentTreeNode(GetEntity(node));

            Push(document, node.LexicalInfo.FileName, 0);
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
            IEntity[] entites = ns.GetMembers();

            currentDocument.Imports[node.Namespace] = new List<IBooParseTreeNode>();

            foreach (var entity in entites)
            {
                if (entity is IType)
                {
                    if (((IType)entity).IsInterface)
                        currentDocument.Imports[node.Namespace].Add(new InterfaceTreeNode(entity, entity.FullName) { Name = entity.Name });
                    else
                        currentDocument.Imports[node.Namespace].Add(new ClassTreeNode(entity, entity.FullName) { Name = entity.Name });
                }
                else if (entity is INamespace)
                    currentDocument.Imports[node.Namespace].Add(new ImportedNamespaceTreeNode { Name = entity.Name });
            }

            base.OnImport(node);
        }

        public override bool EnterInterfaceDefinition(InterfaceDefinition node)
        {
            Push(new InterfaceTreeNode(GetEntity(node), node.FullName), node.Name, node.LexicalInfo.Line);

            return base.EnterInterfaceDefinition(node);
        }

        public override void LeaveInterfaceDefinition(InterfaceDefinition node)
        {
            base.LeaveInterfaceDefinition(node);

            Pop(node.EndSourceLocation.Line);
        }

        public override bool EnterClassDefinition(ClassDefinition node)
        {
            Push(new ClassTreeNode(GetEntity(node), node.FullName), node.Name, node.LexicalInfo.Line);

            return base.EnterClassDefinition(node);
        }

        public override void OnField(Field node)
        {
            Push(new FieldTreeNode(GetEntity(node)), node.Name, node.LexicalInfo.Line);

            base.OnField(node);

            Pop(node.LexicalInfo.Line);
        }

        public override void OnProperty(Property node)
        {
            Push(new PropertyTreeNode(GetEntity(node)), node.Name, node.LexicalInfo.Line);

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
            var method = new MethodTreeNode(GetEntity(node), node.ReturnType != null ? node.ReturnType.ToString() : "void", parent != null ? parent.Name : "");
            method.Parameters = parameters;

            Push(method, node.Name, node.LexicalInfo.Line);

            return base.EnterMethod(node);
        }

        public override bool EnterTryStatement(TryStatement node)
        {
            Push(new TryTreeNode(GetEntity(node)), "", node.LexicalInfo.Line);

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

            Push(new LocalTreeNode(GetEntity(node)) { ReturnType = local.Type.ToString() }, node.Name, node.LexicalInfo.Line);

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

        private void Push(IBooParseTreeNode node, string name, int line)
        {
            node.Parent = currentScope;
            node.Name = name;
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

    [Flags]
    public enum ReferenceType
    {
        Method,
        Local,
        Property,
        Field
    }
}
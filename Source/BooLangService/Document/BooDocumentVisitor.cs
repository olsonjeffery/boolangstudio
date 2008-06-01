using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.BooLangService.Document
{
    public class BooDocumentVisitor : AbstractTransformerCompilerStep
    {
        private readonly IBooParseTreeNode document = new DocumentTreeNode();
        private readonly IDictionary<string, IList<IBooParseTreeNode>> importedNamespaces = new Dictionary<string, IList<IBooParseTreeNode>>();
        private IBooParseTreeNode currentScope;

        public IBooParseTreeNode Document
        {
            get { return document; }
        }

        public IDictionary<string, IList<IBooParseTreeNode>> ImportedNamespaces
        {
            get { return importedNamespaces; }
        }

        public override void Run()
        {
            currentScope = document;

            Visit(CompileUnit);
        }

        public override void OnImport(Import node)
        {
            // this is a bit nasty - get all the members of the referenced namespace
            // then push them on the tree, so they're referencable
            INamespace ns = (INamespace)TypeSystemServices.GetEntity(node);
            IEntity[] entites = ns.GetMembers();

            importedNamespaces[node.Namespace] = new List<IBooParseTreeNode>();

            foreach (IEntity entity in entites)
            {
                if (entity is IType)
                    importedNamespaces[node.Namespace].Add(new ClassTreeNode { Name = entity.Name, FullName = entity.FullName });
                else if (entity is INamespace)
                    importedNamespaces[node.Namespace].Add(new ImportedNamespaceTreeNode { Name = entity.Name });
            }

            base.OnImport(node);
        }

        public override bool EnterClassDefinition(ClassDefinition node)
        {
            Push(new ClassTreeNode(), node.Name, node.LexicalInfo.Line);

            return base.EnterClassDefinition(node);
        }

        public override void OnField(Field node)
        {
            Push(new LocalTreeNode(), node.Name, node.LexicalInfo.Line);

            base.OnField(node);

            Pop(node.LexicalInfo.Line);
        }

        public override bool EnterMethod(Method node)
        {
            Push(new MethodTreeNode(), node.Name, node.LexicalInfo.Line);

            return base.EnterMethod(node);
        }

        public override bool EnterTryStatement(TryStatement node)
        {
            Push(new TryTreeNode(), "", node.LexicalInfo.Line);

            return base.EnterTryStatement(node);
        }

        public override void LeaveTryStatement(TryStatement node)
        {
            base.LeaveTryStatement(node);

            Pop(node.ProtectedBlock.EndSourceLocation.Line);
        }

        public override void OnLocal(Local node)
        {
            Push(new LocalTreeNode(), node.Name, node.LexicalInfo.Line);

            base.OnLocal(node);

            Pop(node.LexicalInfo.Line);
        }

        public override void LeaveMethod(Method node)
        {
            base.LeaveMethod(node);

            Pop(node.Body.EndSourceLocation.Line);
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
            // if the scope is incomplete, then there won't be an end so just use the start
            currentScope.EndLine = endLine;
            currentScope = currentScope.Parent;
        }

        private void PushAndPop(IBooParseTreeNode node, string name, int line)
        {
            Push(node, name, line);
            Pop(line);
        }
    }
}
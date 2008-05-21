using Boo.BooLangService.Document.Nodes;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.Steps;

namespace Boo.BooLangService.Document
{
    public class BooDocumentVisitor : AbstractTransformerCompilerStep
    {
        private readonly IBooParseTreeNode root = new RootTreeNode();
        private IBooParseTreeNode currentScope;

        public IBooParseTreeNode Root
        {
            get { return root; }
        }

        public override void Run()
        {
            currentScope = root;

            Visit(CompileUnit);
        }

        public override bool EnterClassDefinition(ClassDefinition node)
        {
            Push(new ClassTreeNode(), node.Name, node.LexicalInfo.Line);

            return base.EnterClassDefinition(node);
        }

        public override bool EnterMethod(Method node)
        {
            Push(new MethodTreeNode(), node.Name, node.LexicalInfo.Line);

            return base.EnterMethod(node);
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
            currentScope.EndLine = endLine;
            currentScope = currentScope.Parent;
        }
    }
}
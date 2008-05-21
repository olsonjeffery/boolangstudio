using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Boo.BooLangService.VSInterop;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Pipelines;
using Boo.Lang.Compiler.Steps;
using BooLangService;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Drawing;
using VSLangProj;

namespace Boo.BooLangService
{
    [ComVisible(true)]
    [Guid(GuidList.guidBooLangServiceClassString)]
    public class BooLangService : LanguageService
    {
        private readonly DocumentParser docParser = new DocumentParser();
        private const string ImportKeyword = "import";

        #region ctor
        public BooLangService()
            : base()
        {
            DefineColorableItems();
        }
        #endregion

        #region config crap

        public const string LanguageName = "Boo";
        public const string LanguageExtension = ".boo";

        private IScanner _scanner;
        private LanguagePreferences _languagePreferences;

        public override string GetFormatFilterList()
        {
            //throw new NotImplementedException();
            return "Boo File (*.boo) *.boo";
        }

        public override LanguagePreferences GetLanguagePreferences()
        {
            if (_languagePreferences == null)
            {
                _languagePreferences = new LanguagePreferences(this.Site, typeof(BooLangService).GUID, BooLangService.LanguageName);
                _languagePreferences.Init();
            }
            
            return _languagePreferences;
        }

        public override string Name
        {
            get { return LanguageName; }
        }

        #endregion

        #region code parsing stuff

        public override IScanner GetScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer)
        {
            if (_scanner == null)
            {
                //_scanner = new RegularExpressionScanner();
                _scanner = new BooScanner();
            }
            return _scanner;
        }

        public override Source CreateSource(IVsTextLines buffer)
        {
            return new BooSource(this, buffer, new Colorizer(this, buffer, GetScanner(buffer)));
        }

        public class BooScopeTree
        {
            
        }

        public class ParseTreeNodeSet : IList<IBooParseTreeNode>
        {
            private readonly List<IBooParseTreeNode> innerList = new List<IBooParseTreeNode>();
            private readonly IBooParseTreeNode parent;

            public ParseTreeNodeSet(IBooParseTreeNode parent)
            {
                this.parent = parent;
            }

            public int IndexOf(IBooParseTreeNode item)
            {
                return innerList.IndexOf(item);
            }

            public void Insert(int index, IBooParseTreeNode item)
            {
                innerList.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                innerList.RemoveAt(index);
            }

            public IBooParseTreeNode this[int index]
            {
                get { return innerList[index]; }
                set { innerList[index] = value; }
            }

            public void Add(IBooParseTreeNode item)
            {
                item.Parent = parent;
                innerList.Add(item);
            }

            public void Clear()
            {
                innerList.Clear();
            }

            public bool Contains(IBooParseTreeNode item)
            {
                return innerList.Contains(item);
            }

            public void CopyTo(IBooParseTreeNode[] array, int arrayIndex)
            {
                innerList.CopyTo(array, arrayIndex);
            }

            public bool Remove(IBooParseTreeNode item)
            {
                return innerList.Remove(item);
            }

            public int Count
            {
                get { return innerList.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            IEnumerator<IBooParseTreeNode> IEnumerable<IBooParseTreeNode>.GetEnumerator()
            {
                return innerList.GetEnumerator();
            }

            public IEnumerator GetEnumerator()
            {
                return ((IEnumerable<IBooParseTreeNode>)this).GetEnumerator();
            }
        }

        public abstract class AbstractTreeNode : IBooParseTreeNode
        {
            private IBooParseTreeNode parent;
            private readonly IList<IBooParseTreeNode> children;
            private string name;
            private int startLine;
            private int endLine;

            public AbstractTreeNode()
            {
                children = new ParseTreeNodeSet(this);
            }

            public IBooParseTreeNode Parent
            {
                get { return parent; }
                set { parent = value; }
            }

            public IList<IBooParseTreeNode> Children
            {
                get { return children; }
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public int StartLine
            {
                get { return startLine; }
                set { startLine = value; }
            }

            public int EndLine
            {
                get { return endLine; }
                set { endLine = value; }
            }
        }

        public class NamespaceTreeNode : AbstractTreeNode
        {}

        public class ClassTreeNode : AbstractTreeNode
        {}

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

        public class DocumentVisualiser
        {
            public static string Output(IBooParseTreeNode node)
            {
                return Output(node, 0);
            }

            private static string Output(IBooParseTreeNode node, int indentLevel)
            {
                string indent = "";

                for (int i = 0; i < indentLevel; i++)
                {
                    indent += "  ";
                }

                string output = indent +
                    node.GetType().Name + ": " +
                    node.Name +
                    "(" + node.StartLine + "," + node.EndLine + ")" +
                    Environment.NewLine;

                foreach (IBooParseTreeNode child in node.Children)
                {
                    output += Output(child, indentLevel + 1);
                }

                return output;
            }
        }

        public class BooDocumentCompiler
        {
            private readonly BooCompiler compiler = new BooCompiler();
            private readonly BooDocumentVisitor visitor = new BooDocumentVisitor();

            public BooDocumentCompiler()
            {
                compiler.Parameters.OutputWriter = new StringWriter();
                compiler.Parameters.Pipeline = new ResolveExpressions();
                compiler.Parameters.Pipeline.BreakOnErrors = false;
                compiler.Parameters.Pipeline.Add(visitor);
            }

            public IBooParseTreeNode Compile(string filename, string source)
            {
                compiler.Parameters.Input.Add(new StringInput(filename, source));
                compiler.Run();

                return visitor.Root;
            }
        }

        // scopes for ctrl+space in whitespace (non-assignments)
        //
        // namespace
        // {
        //   keywords
        //
        //   class
        //   {
        //     [parent scope]
        //     types
        //     methods
        //     class member declarations
        //
        //     method
        //     {
        //       [parent scope]
        //       local declarations
        //
        //       inner scope
        //       {
        //         [parent scope]
        //         local declarations
        //       }
        //     }
        //   }
        // }
        /// <summary>
        /// EPIC!!!!
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public override AuthoringScope ParseSource(ParseRequest req)
        {
            BooDocumentCompiler compiler = new BooDocumentCompiler();
            IBooParseTreeNode compiledTree = compiler.Compile(req.FileName, req.Text);

            return new BooScope(compiledTree);
        }

        private AuthoringScope GetNamespaces(ParseRequest req, string line)
        {
            // get any namespace already written (i.e. "Boo.Lang.")
            string namespaceContinuation = line.Trim();
            namespaceContinuation = namespaceContinuation.Remove(0, ImportKeyword.Length).Trim();

            // get project references for the project that the current file is in
            ProjectHierarchy projects = new ProjectHierarchy(this);
            VSProject project = projects.GetContainingProject(req.FileName);
            IList<ProjectReference> references = projects.GetReferences(project);

            return docParser.GetNamespaceSelect(references, namespaceContinuation);
        }

        #endregion

        #region color stuff

        private Dictionary<int,Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem> _colorableItems = 
            new Dictionary<int,Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem>();

        public override int GetItemCount(out int count)
        {
            count = _colorableItems.Count;
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        public override int GetColorableItem(int index, out Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem item)
        {
            item = _colorableItems[index];
            return Microsoft.VisualStudio.VSConstants.S_OK;
        }

        private void DefineColorableItems()
        {
            _colorableItems.Add((int)TokenColor.Comment,
                new Boo.BooLangService.ColorableItem("Comment",
                    COLORINDEX.CI_DARKGREEN,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Identifier,
                new Boo.BooLangService.ColorableItem("Identifier",
                    COLORINDEX.CI_SYSPLAINTEXT_FG,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Keyword,
                new Boo.BooLangService.ColorableItem("Keyword",
                    COLORINDEX.CI_MAGENTA,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Number,
                new Boo.BooLangService.ColorableItem("Number",
                    COLORINDEX.CI_BLUE,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.String,
                new Boo.BooLangService.ColorableItem("String",
                    COLORINDEX.CI_DARKGREEN,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
            _colorableItems.Add((int)TokenColor.Text,
                new Boo.BooLangService.ColorableItem("Text",
                    COLORINDEX.CI_SYSPLAINTEXT_FG,
                    COLORINDEX.CI_USERTEXT_BK,
                    false, false));
        }

       #endregion

    }

    public class RootTreeNode : BooLangService.AbstractTreeNode
    {}

    public class LocalTreeNode : BooLangService.AbstractTreeNode
    {}

    public class MethodTreeNode : BooLangService.AbstractTreeNode
    {}

    public interface IBooParseTreeNode
    {
        IBooParseTreeNode Parent { get; set; }
        IList<IBooParseTreeNode> Children { get; }
        string Name { get; set; }
        int StartLine { get; set; }
        int EndLine { get; set; }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Boo.BooLangService.Document;
using Boo.BooLangStudioSpecs.Document;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public class FixtureCompiler
    {
        private static BooDocumentCompiler _compiler;
        private const string Caret = "~";

        private BooDocumentCompiler GetCompiler()
        {
            _compiler = _compiler ?? new BooDocumentCompiler();
            _compiler.Reset();

            return _compiler;
        }

        private string GetSource(string file, ref int? caretColumn, ref int? caretLine, ref string caretLineSource, ref string caretFileName)
        {
            var source = File.ReadAllText(file);

            if (source.Contains(Caret))
            {
                GetCaretLocation(source, out caretColumn, out caretLine, out caretLineSource);

                source = source.Replace(Caret, "");
                caretFileName = file;
            }

            return source;
        }

        private void GetCaretLocation(string source, out int? column, out int? line, out string lineSource)
        {
            var lines = source.Split(new []{Environment.NewLine}, StringSplitOptions.None);

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var caretIndex = lines[lineIndex].IndexOf(Caret);

                if (caretIndex >= 0)
                {
                    column = caretIndex;
                    line = lineIndex;
                    lineSource = lines[lineIndex].Replace(Caret, "");
                    return;
                }
            }

            column = null;
            line = null;
            lineSource = null;
        }

        private string GetFixturesDirectory()
        {
            var stackTrace = new StackTrace();
            var callingStackFrame = stackTrace.GetFrame(3); // this is flaky
            var method = callingStackFrame.GetMethod();

            var methodName = method.Name;
            var className = method.DeclaringType.Name;

            // ..\..\Namespace\Fixtures\ClassName\MethodName
            return new StringBuilder()
                .Append("..\\..\\")
                .Append(method.DeclaringType.Namespace.Replace("Boo.BooLangStudioSpecs.", "").Replace(".", "\\"))
                .Append("\\Fixtures\\")
                .Append(className)
                .Append("\\")
                .Append(methodName)
                .ToString();
        }

        public CompilationOutput CompileForCurrentMethod()
        {
            var fixturesPath = GetFixturesDirectory();
            var compiler = GetCompiler();

            int? caretColumn = null;
            int? caretLine = null;
            string caretLineSource = null;
            string caretFileName = null;

            if (File.Exists(fixturesPath + ".boo"))
            {
                string source = GetSource(fixturesPath + ".boo", ref caretColumn, ref caretLine, ref caretLineSource, ref caretFileName);

                compiler.AddSource(fixturesPath + ".boo", source);
            }
            else
            {
                foreach (var file in Directory.GetFiles(fixturesPath))
                {
                    string source = GetSource(file, ref caretColumn, ref caretLine, ref caretLineSource, ref caretFileName);

                    compiler.AddSource(file, source);
                }
            }

            var project = compiler.Compile();

            return new CompilationOutput(project, new CaretLocation(caretColumn, caretLine, caretFileName, caretLineSource));
        }
    }
}
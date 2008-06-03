using System;
using Boo.BooLangService.Document;

namespace Boo.BooLangStudioSpecs.Intellisense
{
    public abstract class BaseCompilerContext
    {
        protected const string NoFileName = "";

        protected CompiledDocument Compile(string text)
        {
            // kill off the first newline because of the style of text we're doing
            if (text.StartsWith(Environment.NewLine))
                text = text.Remove(0, Environment.NewLine.Length);

            var compiler = new BooDocumentCompiler();

            return compiler.Compile(NoFileName, text);
        }
    }
}
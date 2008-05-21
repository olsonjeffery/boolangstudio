using System;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Document
{
    /// <summary>
    /// IGNORE ME, I'm a developer aid!
    /// 
    /// Outputs a IBooParseTreeNode hierarchy as a text tree.
    /// </summary>
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
}
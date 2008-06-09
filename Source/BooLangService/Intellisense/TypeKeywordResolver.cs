using System;
using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Intellisense
{
    public class TypeKeywordResolver
    {
        private readonly IDictionary<Type, string[]> scopeKeywordMap = new Dictionary<Type, string[]>();

        public TypeKeywordResolver()
        {
            Add<DocumentTreeNode>(
                "abstract",
                "class",
                "constructor",
                "def",
                "destructor",
                "enum",
                "event",
                "final",
                "interface",
                "internal",
                "override",
                "public",
                "protected",
                "private",
                "static",
                "struct",
                "virtual",
                "cast",
                "def",
                "do",
                "elif",
                "else",
                "from",
                "for",
                "goto",
                "isa",
                "if",
                "null",
                "pass",
                "raise",
                "return",
                "typeof",
                "unless",
                "while",
                "yield"
            );
            Add<ClassTreeNode>(
                "abstract",
                "class",
                "constructor",
                "def",
                "destructor",
                "enum",
                "event",
                "final",
                "interface",
                "internal",
                "override",
                "public",
                "protected",
                "private",
                "static",
                "struct",
                "virtual"
            );
            Add<MethodTreeNode>(
                "cast",
                "def",
                "do",
                "elif",
                "else",
                "from",
                "for",
                "goto",
                "isa",
                "if",
                "null",
                "pass",
                "raise",
                "return",
                "typeof",
                "unless",
                "while",
                "yield"
            );
            Add<TryTreeNode, MethodTreeNode>(
                "ensure",
                "except",
                "failure"
            );
        }

        public string[] GetForScope(IBooParseTreeNode scope)
        {
            if (scope == null) return new string[] { };

            Type type = scope.GetType();

            if (scopeKeywordMap.ContainsKey(type))
                return scopeKeywordMap[type];

            return new string[] {};
        }

        private void Add<T>(params string[] keywords) where T : IBooParseTreeNode
        {
            scopeKeywordMap.Add(typeof(T), keywords);
        }

        private void Add<TTarget, TSource>(params string[] additionalKeywords)
            where TTarget : IBooParseTreeNode
            where TSource : IBooParseTreeNode
        {
            string[] sourceKeywords = new string[] {};

            scopeKeywordMap.TryGetValue(typeof (TSource), out sourceKeywords);

            List<string> keywords = new List<string>(sourceKeywords);

            keywords.AddRange(additionalKeywords);

            Add<TTarget>(keywords.ToArray());
        }
    }
}
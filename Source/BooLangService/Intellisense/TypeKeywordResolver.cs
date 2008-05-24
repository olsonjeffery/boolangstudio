using System;
using System.Collections.Generic;
using Boo.BooLangService.Document.Nodes;

namespace Boo.BooLangService.Intellisense
{
    /*
		public const int ABSTRACT = 10;
		public const int AND = 11;
		public const int AS = 12;
		public const int BREAK = 13;
		public const int CONTINUE = 14;
		public const int CALLABLE = 15;
		public const int CAST = 16;
		public const int CHAR = 17;
		public const int CLASS = 18;
		public const int CONSTRUCTOR = 19;
		public const int DEF = 20;
		public const int DESTRUCTOR = 21;
		public const int DO = 22;
		public const int ELIF = 23;
		public const int ELSE = 24;
		public const int ENSURE = 25;
		public const int ENUM = 26;
		public const int EVENT = 27;
		public const int EXCEPT = 28;
		public const int FAILURE = 29;
		public const int FINAL = 30;
		public const int FROM = 31;
		public const int FOR = 32;
		public const int FALSE = 33;
		public const int GET = 34;
		public const int GOTO = 35;
		public const int IMPORT = 36;
		public const int INTERFACE = 37;
		public const int INTERNAL = 38;
		public const int IS = 39;
		public const int ISA = 40;
		public const int IF = 41;
		public const int IN = 42;
		public const int NOT = 43;
		public const int NULL = 44;
		public const int OF = 45;
		public const int OR = 46;
		public const int OVERRIDE = 47;
		public const int PASS = 48;
		public const int NAMESPACE = 49;
		public const int PARTIAL = 50;
		public const int PUBLIC = 51;
		public const int PROTECTED = 52;
		public const int PRIVATE = 53;
		public const int RAISE = 54;
		public const int REF = 55;
		public const int RETURN = 56;
		public const int SET = 57;
		public const int SELF = 58;
		public const int SUPER = 59;
		public const int STATIC = 60;
		public const int STRUCT = 61;
		public const int THEN = 62;
		public const int TRY = 63;
		public const int TRUE = 65;
		public const int TYPEOF = 66;
		public const int UNLESS = 67;
		public const int VIRTUAL = 68;
		public const int WHILE = 69;
		public const int YIELD = 70;
     */
    public class TypeKeywordResolver
    {
        private readonly IDictionary<Type, string[]> scopeKeywordMap = new Dictionary<Type, string[]>();

        public TypeKeywordResolver()
        {
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
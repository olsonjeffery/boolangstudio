using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Package;
using Boo.Lang.Parser;
using Microsoft.VisualStudio.TextManager.Interop;
using BooPegLexer;

namespace Boo.BooLangService
{
    public partial class BooScanner : IScanner
    {
        #region properties and members
        private PegLexer _lexer = null;
		public PegLexer Lexer
		{
			get
			{
				return _lexer;
			}
		}
        #endregion
		
        /// <summary>
        /// default ctor... news up a lexer
        /// </summary>
        public BooScanner()
        {
        	_lexer = new PegLexer();
        	_lexer.InitializeAndBindPegs(GetKeywords(),new string[] { });
        }
		
        /// <summary>
        /// ctor that takes a lexer as an argument.. assume the lexer has
        /// already had it's PEGs bound...
        /// </summary>
        /// <param name="buffer"></param>
        public BooScanner(PegLexer lexer)
        {
        	_lexer = lexer;
        	_lexer.InitializeAndBindPegs(GetKeywords(), new string[] { });
        }
        
        public BooScanner(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines buffer) 
            : this()
        {
            //
        }

        #region lexer/colorizing-related

        PegToken pegToken = null;
        public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {
        	if(_lexer.NextToken(tokenInfo,pegToken,ref state) == false)
        		return false;

        	return true;
        }

        public  void SetSource(string source, int offset)
        {
        	_lexer.SetSource(source.Substring(offset));
        }
        

	
        protected string[] GetKeywords()
       {
	       	return new string[] {
	       		// declarations
	       		"def",
	       		"class",
	       		"interface",
	       		"get",
	       		"set",
	       		"namespace",
	       		// modifiers
	       		"public",
	       		"private",
	       		"protected",
	       		"internal",
	       		"virtual",
	       		"override",
	       		"abstract",
	       		"static",
	       		"final",
	       		"partial",
	       		"transient",
	       		// conditionals
	       		"if",
	       		"elif",
	       		"else",
	       		// exception stuff
	       		"raise",
	       		"except",
	       		"ensure",
	       		"try",
	       		// loops
	       		"for",
	       		"while",
	       		// literals
	       		"null",
	       		"true",
	       		"false",
	       		// logical operators
	       		"and",
	       		"or",
	       		"is",
	       		"isa",
	       		"not",
	       		"in",
	       		// misc
	       		"as",
	       		"do",
	       		"break",
	       		"continue",
	       		"cast",
	       		"import",
	       		"from",
	       		"goto",
	       		"of",
	       		"ref",
	       		"self",
	       		"super",
	       		"typeof",
	       		// return-type-stuff
	       		"yield",       	
	       		"pass",
	       		"return",
	       		// primitives
	       		"char",
	       		"string",
	       		"int",
	       		"callable",
	       		"enum",
	       		"struct",
	       		"event",
	       		// special methods
	       		"constructor",
	       		"destructor"
	       		
	       	};
       }
        
        #endregion

    }

}

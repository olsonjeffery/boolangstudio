using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooColorizerParser;
using Microsoft.VisualStudio.Package;
using System.IO;
using Xunit;
using Spec = Xunit.FactAttribute;
using Rhino.Mocks;
using Rhino.Testing.AutoMocking;

using Boo.BooLangService;

namespace Boo.BooLangStudioSpecs
{
	/// <summary>
	/// Description of LexerPartialMockFixture.
	/// </summary>
	public class LexerPartialMockFixture
	{
		protected BooTokenLexer lexer = null;		
		public LexerPartialMockFixture()
			: base()
		{
		  lexer = MockRepository.GenerateMock<BooTokenLexer>();
		}
	}
}

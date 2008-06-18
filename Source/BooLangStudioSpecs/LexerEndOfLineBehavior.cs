using System;
using Xunit;
using Microsoft.VisualStudio.Package;
using BooLangService;
using BooPegLexer;
using Rhino.Mocks;
using Boo.Pegs;

namespace Boo.BooLangStudioSpecs
{
	/// <summary>
	/// Description of LexerEndOfLineBehavior.
	/// </summary>
	public class WhenParsingTheLastTokenInALine : LexerPartialMockFixture
	{
		protected int state = 0;
		protected PegToken pegToken = new PegToken();
		protected string line = string.Empty;
		public WhenParsingTheLastTokenInALine()
			: base()
		{
			line = "singleIdentifierToken";
			lexer.SetSource(line);
		}
	}
}

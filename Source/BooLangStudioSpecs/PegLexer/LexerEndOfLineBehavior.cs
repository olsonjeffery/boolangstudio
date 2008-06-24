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
            // if you pass in an empty line, the lexer
            // will act as though it's at the EOL
			line = "";
		}

        [Fact]
        public void NextTokenMethodShouldntBranchOut()
        {
            using (Mocks.Record())
            {
                DoNotExpect
                    .Call(lexer.InGeneralLexingCase(pegToken, ref state));
                DoNotExpect
                    .Call(lexer.InMultiLineComment(pegToken, ref  state));
                DoNotExpect
                    .Call(lexer.InTrippleQuoteString(pegToken, ref state));
            }
            using (Mocks.Playback())
            {
                lexer.NextToken(pegToken, ref state);
            }
        }

        [Fact]
        public void NextTokenMethodShouldReturnFalse()
        {
            lexer = new PegLexer();
            lexer.SetSource(line);
            // consume the token
            bool retVal = true;

            retVal = lexer.NextToken(pegToken, ref state);
            

            Assert.False(retVal);
        }

	}
}

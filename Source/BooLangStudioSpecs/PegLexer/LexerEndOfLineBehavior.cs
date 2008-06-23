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
			line = "singleToken";
		}

        [Fact]
        public void NextTokenMethodShouldntBranchOut()
        {
            bool retVal = true;
            using (Mocks.Record())
            {
                Expect
                    .Call(lexer.InGeneralLexingCase(pegToken, ref state))
                    .Return(true);
            }
            using (Mocks.Playback())
            {
                lexer.SetSource(line);
                lexer.NextToken(pegToken, ref state);
            }

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
                retVal = lexer.NextToken(pegToken, ref state);
            }
            Assert.False(retVal);
        }

        [Fact]
        public void NextTokenMethodShouldReturnFalse()
        {
            lexer.SetSource(line);
            // consume the token
            lexer.NextToken(pegToken, ref state);
            bool retVal = lexer.NextToken(pegToken, ref state);
            Assert.False(retVal);
        }

	}
}

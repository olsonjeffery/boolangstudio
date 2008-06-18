/*
 * Created by SharpDevelop.
 * User: jeff_2
 * Date: 6/8/2008
 * Time: 8:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

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
	/// Description of WhenAttemptingToGetATokenFromThePegLexer.
	/// </summary>
	public abstract class VerifyingNextTokenStateBranchingBehavior : LexerPartialMockFixture
	{
		protected int state = 0;
		protected string line = string.Empty;
		protected TokenInfo token = null;
		protected PegToken pegToken = null;
		public VerifyingNextTokenStateBranchingBehavior()
			: base()
		{
			token = new TokenInfo();
			pegToken = new PegToken();
			
			line = "doesn't matter because it's in the ml_comment zone";
		}
	}	
	

	
	public class AndStateIsThirteen : VerifyingNextTokenStateBranchingBehavior
	{
		public AndStateIsThirteen()
			: base()
		{
			state = 13;
		}
		
		[Fact]
		public void InMultiLineCommentMethodShouldBeInvoked()
		{
			using (Mocks.Record())
			{
				Expect
				  .Call(lexer.InMultiLineComment(pegToken,ref state))
					.Return(false);
			}
			using (Mocks.Playback())
			{
				lexer.SetSource(line);
				lexer.NextToken(pegToken,ref state);
			}
		}
	}
	
	public class AndStateIsFourteen : VerifyingNextTokenStateBranchingBehavior
	{
		public AndStateIsFourteen()
			: base()
		{
			state = 14;
		}
		
		[Fact]
		public void InTrippleQuoteStringMethodShouldBeInvoked()
		{
			using (Mocks.Record())
			{
				Expect
				  .Call(lexer.InTrippleQuoteString(pegToken,ref state))
					.Return(false);
			}
			using (Mocks.Playback())
			{
				lexer.SetSource(line);
				lexer.NextToken(pegToken,ref state);
			}
		}
	}
	
	public class AndStateIsZero : VerifyingNextTokenStateBranchingBehavior
	{
		public AndStateIsZero()
			: base()
		{
			state = 0;
		}
		
		[Fact]
		public void InGeneralLexingCaseMethodShouldBeInvoked()
		{
			using (Mocks.Record())
			{
				Expect
				  .Call(lexer.InGeneralLexingCase(pegToken,ref state))
					.Return(false);
			}
			using (Mocks.Playback())
			{
				lexer.SetSource(line);
				lexer.NextToken(pegToken,ref state);
			}
		}
	}
}
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

namespace Boo.BooLangStudioSpecs
{
	public abstract class AndPassingInDifferentStates : WhenAttemptingToGetATokenFromThePegLexer
	{
		protected int startIndex = 0;
		protected int endIndex = 0;
		
		public AndPassingInDifferentStates()
			: base()
		{
			line = "doesn't matter because it's in the ml_comment zone";
			endIndex = line.Length-1;
		}
		

	}
	
	public class AndStateIsThirteen : AndPassingInDifferentStates
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
					.Call(lexer.InMultiLineComment(token,pegToken,ref state))
					.Return(false);
			}
			using (Mocks.Playback())
			{
				lexer.NextToken(token,pegToken,ref state);
			}
		}
	}
	
	public class AndStateIsFourteen : AndPassingInDifferentStates
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
					.Call(lexer.InTrippleQuoteString(token,pegToken,ref state))
					.Return(false);
			}
			using (Mocks.Playback())
			{
				lexer.NextToken(token,pegToken,ref state);
			}
		}
	}
	
	public class AndStateIsZero : AndPassingInDifferentStates
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
					.Call(lexer.InGeneralLexingCase(token,pegToken,ref state))
					.Return(false);
			}
			using (Mocks.Playback())
			{
				lexer.NextToken(token,pegToken,ref state);
			}
		}
	}
}
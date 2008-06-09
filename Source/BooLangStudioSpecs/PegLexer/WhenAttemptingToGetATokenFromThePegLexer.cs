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
	/// <summary>
	/// Description of WhenAttemptingToGetATokenFromThePegLexer.
	/// </summary>
	public abstract class WhenAttemptingToGetATokenFromThePegLexer : AutoMockingTestFixture
	{
		protected BooPegLexer.BooPegLexer lexer = null;
		protected int state = 0;
		protected string line = string.Empty;
		protected TokenInfo token = null;
		public WhenAttemptingToGetATokenFromThePegLexer()
			: base()
		{
			lexer = Create<BooPegLexer.BooPegLexer>();
			token = new TokenInfo();
		}
	}
	
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
					//.Call(Mock<BooPegLexer.BooPegLexer>().InMultiLineComment(token,line,ref state))
					.Call(Mock<BooPegLexer.BooPegLexer>().InMultiLineComment())
					.Return(false);
			}
			using (Mocks.Playback())
			{
				lexer.NextToken(token,line,ref state);
			}
		}
	}
	
}

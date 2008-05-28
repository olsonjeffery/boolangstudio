using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Microsoft.VisualStudio.Package;
using System.IO;
using Context = MbUnit.Framework.TestFixtureAttribute;
using Spec = MbUnit.Framework.TestAttribute;

namespace Boo.BooLangStudioSpecs
{

    [Context]
    public class WhenParsingMalformedStringTokensWithNoClosingQuotes : LexingBaseFixture
    {
        public override void SetUp()
        {
            base.SetUp();
            //               0         1         2         3         4
            //               012345678901234567890123456789012345678901
            rawCodeString = "print 'hello this should be a string token";
            BuildTokens(rawCodeString);
        }

        [Spec]
        public void MalformedTokenShouldBeTreatedAsAStringToken()
        {
            List<TokenInfo> stringTokens = new List<TokenInfo>(from i in tokens
                                                                where i.Type == TokenType.String
                                                                select i);
            Assert.IsTrue(stringTokens.Count == 1, "Not expected string token count! Expected 1, Actual: " + stringTokens.Count.ToString());
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boo.BooLangService.StringParsing;
using Xunit;

namespace BooLangStudioSpecs.StringParsing
{
    public class ExcludeStringMatcherTests
    {
        [Fact]
        public void DoesntFindAMatchIfThereIsntOne()
        {
            string source = "SomeString";
            var matcher = new ExcludeStringMatcher(source);

            Assert.Null(matcher.FindNextIndex('!'));
        }

        [Fact]
        public void FindsAMatchWhenThereIsOne()
        {
            string source = "Some!String";
            var matcher = new ExcludeStringMatcher(source);

            Assert.Equal(4, matcher.FindNextIndex('!'));
        }

        [Fact]
        public void MatchIndexIsCorrectlyOffsetWhenUsingStartIndex()
        {
            string source = "Some St!ring";
            var matcher = new ExcludeStringMatcher(source);

            matcher.SetStartIndex(5);

            Assert.Equal(7, matcher.FindNextIndex('!'));
        }

        [Fact]
        public void DoesntFindAMatchWhenMatchIsBeforeStartIndex()
        {
            string source = "Some!String";
            var matcher = new ExcludeStringMatcher(source);

            matcher.SetStartIndex(5);

            Assert.Null(matcher.FindNextIndex('!'));
        }

        [Fact]
        public void DoesntFindMatchIfMatchIsInsideStringLiteral()
        {
            string source = "Some \"!\" String";
            var matcher = new ExcludeStringMatcher(source);

            Assert.Null(matcher.FindNextIndex('!'));
        }
    }
}

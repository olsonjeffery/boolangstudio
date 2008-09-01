using Boo.BooLangService.StringParsing;
using Xunit;

namespace BooLangStudioSpecs.StringParsing
{
    public class BracketPairFinderTests
    {
        [Fact]
        public void FindsSinglePair()
        {
            string value = "()";
            var finder = new BracketPairFinder(BracketPairType.Round);

            BracketPairCollection pairs = finder.FindPairs(value);

            Assert.Equal(1, pairs.Count);

            var pair = pairs.FindLeftByIndex(0);

            Assert.NotNull(pair);
            Assert.Equal(1, pair.RightIndex);
        }

        [Fact]
        public void FindsTwoPairsWhenAdjacent()
        {
            string value = "()()";
            var finder = new BracketPairFinder(BracketPairType.Round);

            BracketPairCollection pairs = finder.FindPairs(value);

            Assert.Equal(2, pairs.Count);

            var pair = pairs.FindLeftByIndex(0);

            Assert.NotNull(pair);
            Assert.Equal(1, pair.RightIndex);
            
            pair = pairs.FindLeftByIndex(2);

            Assert.NotNull(pair);
            Assert.Equal(3, pair.RightIndex);
        }

        [Fact]
        public void FindsTwoPairsWhenNested()
        {
            string value = "(())";
            var finder = new BracketPairFinder(BracketPairType.Round);

            BracketPairCollection pairs = finder.FindPairs(value);

            Assert.Equal(2, pairs.Count);

            var pair = pairs.FindLeftByIndex(0);

            Assert.NotNull(pair);
            Assert.Equal(3, pair.RightIndex);

            pair = pairs.FindLeftByIndex(1);

            Assert.NotNull(pair);
            Assert.Equal(2, pair.RightIndex);
        }
    }
}
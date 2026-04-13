using System.Collections.Generic;
using StockFishPortApp_5._0;
using Xunit;

namespace StockFish.Tests
{
    /// <summary>
    /// Tests for the Misc utility class and the BitSet helper.
    /// </summary>
    public class MiscTests
    {
        // ── engine_info ───────────────────────────────────────────────────────

        [Fact]
        public void EngineInfo_ContainsVersionString()
        {
            string info = Misc.engine_info();
            Assert.Contains("StockFishPort", info);
        }

        [Fact]
        public void EngineInfo_ContainsAuthor()
        {
            string info = Misc.engine_info();
            Assert.Contains("Mauricio Cortes", info);
        }

        // ── isdigit ───────────────────────────────────────────────────────────

        [Theory]
        [InlineData('0', true)]
        [InlineData('9', true)]
        [InlineData('5', true)]
        [InlineData('a', false)]
        [InlineData(' ', false)]
        [InlineData('-', false)]
        public void IsDigit_ReturnsExpected(char c, bool expected)
        {
            Assert.Equal(expected, Misc.isdigit(c));
        }

        // ── islower ───────────────────────────────────────────────────────────

        [Theory]
        [InlineData('a', true)]
        [InlineData('z', true)]
        [InlineData('A', false)]
        [InlineData('Z', false)]
        [InlineData('1', true)]   // digits are unchanged by ToLower
        [InlineData(' ', true)]   // space is unchanged by ToLower
        public void IsLower_ReturnsExpected(char c, bool expected)
        {
            Assert.Equal(expected, Misc.islower(c));
        }

        // ── toupper / tolower ─────────────────────────────────────────────────

        [Theory]
        [InlineData('a', 'A')]
        [InlineData('z', 'Z')]
        [InlineData('A', 'A')]
        public void ToUpper_ReturnsUpperCase(char input, char expected)
        {
            Assert.Equal(expected, Misc.toupper(input));
        }

        [Theory]
        [InlineData('A', 'a')]
        [InlineData('Z', 'z')]
        [InlineData('a', 'a')]
        public void ToLower_ReturnsLowerCase(char input, char expected)
        {
            Assert.Equal(expected, Misc.tolower(input));
        }

        // ── cpu_count ─────────────────────────────────────────────────────────

        [Fact]
        public void CpuCount_ReturnsPositiveNumber()
        {
            Assert.True(Misc.cpu_count() > 0);
        }

        // ── CreateStack ───────────────────────────────────────────────────────

        [Fact]
        public void CreateStack_SingleToken_ContainsThatToken()
        {
            var stack = Misc.CreateStack("hello");
            Assert.Single(stack);
            Assert.Equal("hello", stack.Pop());
        }

        [Fact]
        public void CreateStack_MultipleTokens_PreservesOrder()
        {
            // Stack is LIFO but CreateStack pushes in reverse so first word is on top
            var stack = Misc.CreateStack("one two three");
            Assert.Equal("one",   stack.Pop());
            Assert.Equal("two",   stack.Pop());
            Assert.Equal("three", stack.Pop());
        }

        [Fact]
        public void CreateStack_ExtraSpaces_AreIgnored()
        {
            var stack = Misc.CreateStack("  a   b  ");
            Assert.Equal(2, stack.Count);
        }

        [Fact]
        public void CreateStack_EmptyExtraTokens_NotAdded()
        {
            var stack = Misc.CreateStack("x");
            Assert.Single(stack);
        }

        // ── existSearchMove ───────────────────────────────────────────────────

        [Fact]
        public void ExistSearchMove_EmptyList_ReturnsFalse()
        {
            var moves = new List<int>();
            Assert.False(Misc.existSearchMove(moves, 42));
        }

        [Fact]
        public void ExistSearchMove_MovePresent_ReturnsTrue()
        {
            int move = Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4);
            var moves = new List<int> { move };
            Assert.True(Misc.existSearchMove(moves, move));
        }

        [Fact]
        public void ExistSearchMove_MoveAbsent_ReturnsFalse()
        {
            int move1 = Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4);
            int move2 = Types.make_move(SquareS.SQ_D2, SquareS.SQ_D4);
            var moves = new List<int> { move1 };
            Assert.False(Misc.existSearchMove(moves, move2));
        }

        [Fact]
        public void ExistSearchMove_MultipleEntries_FindsCorrectOne()
        {
            int m1 = Types.make_move(SquareS.SQ_A1, SquareS.SQ_A2);
            int m2 = Types.make_move(SquareS.SQ_B1, SquareS.SQ_B3);
            int m3 = Types.make_move(SquareS.SQ_C2, SquareS.SQ_C4);
            var moves = new List<int> { m1, m2, m3 };

            Assert.True(Misc.existSearchMove(moves, m2));
            Assert.False(Misc.existSearchMove(moves, Types.make_move(SquareS.SQ_D2, SquareS.SQ_D4)));
        }
    }

    /// <summary>
    /// Tests for the BitSet helper class.
    /// </summary>
    public class BitSetTests
    {
        [Fact]
        public void None_AllFalse_ReturnsTrue()
        {
            var bs = new BitSet(10);
            Assert.True(bs.none());
        }

        [Fact]
        public void None_OneBitSet_ReturnsFalse()
        {
            var bs = new BitSet(10);
            bs[5] = true;
            Assert.False(bs.none());
        }

        [Fact]
        public void SetAll_True_AllBitsAreTrue()
        {
            var bs = new BitSet(8);
            bs.SetAll(true);
            for (int i = 0; i < 8; i++)
                Assert.True(bs[i]);
        }

        [Fact]
        public void SetAll_False_ClearsBits()
        {
            var bs = new BitSet(8);
            bs.SetAll(true);
            bs.SetAll(false);
            Assert.True(bs.none());
        }

        [Fact]
        public void Indexer_GetSet_RoundTrip()
        {
            var bs = new BitSet(16);
            bs[7]  = true;
            bs[15] = true;
            Assert.True(bs[7]);
            Assert.True(bs[15]);
            Assert.False(bs[0]);
            Assert.False(bs[8]);
        }
    }
}

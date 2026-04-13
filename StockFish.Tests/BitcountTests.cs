using StockFishPortApp_5._0;
using Xunit;

namespace StockFish.Tests
{
    /// <summary>
    /// Tests for the Bitcount popcount implementations.
    /// </summary>
    public class BitcountTests
    {
        // ── popcount ──────────────────────────────────────────────────────────

        [Fact]
        public void Popcount_Zero_ReturnsZero()
        {
            Assert.Equal(0, Bitcount.popcount(0UL));
        }

        [Fact]
        public void Popcount_AllOnes_Returns64()
        {
            Assert.Equal(64, Bitcount.popcount(ulong.MaxValue));
        }

        [Fact]
        public void Popcount_OneBit_ReturnsOne()
        {
            Assert.Equal(1, Bitcount.popcount(1UL));
            Assert.Equal(1, Bitcount.popcount(0x8000000000000000UL)); // MSB
            Assert.Equal(1, Bitcount.popcount(0x0000000100000000UL)); // bit 32
        }

        [Theory]
        [InlineData(0x5555555555555555UL, 32)]  // every other bit
        [InlineData(0xAAAAAAAAAAAAAAAAUL, 32)]  // every other bit (shifted)
        [InlineData(0xFF00FF00FF00FF00UL, 32)]
        [InlineData(0x00FF00FF00FF00FFUL, 32)]
        [InlineData(0x0000000000000003UL, 2)]
        [InlineData(0xFUL, 4)]
        [InlineData(0xFFUL, 8)]
        [InlineData(0xFFFFUL, 16)]
        public void Popcount_VariousPatterns_ReturnsCorrectCount(ulong value, int expected)
        {
            Assert.Equal(expected, Bitcount.popcount(value));
        }

        [Fact]
        public void Popcount_SingleBitAtEachPosition_ReturnsOne()
        {
            for (int i = 0; i < 64; i++)
            {
                ulong bit = 1UL << i;
                Assert.Equal(1, Bitcount.popcount(bit));
            }
        }

        [Fact]
        public void Popcount_AgreesWithDotNetBitOperations()
        {
            ulong[] testValues = {
                0UL, 1UL, 0xDEADBEEFUL, 0xCAFEBABEDEADBEEFUL,
                0xF0F0F0F0F0F0F0F0UL, ulong.MaxValue
            };
            foreach (ulong v in testValues)
            {
                int expected = System.Numerics.BitOperations.PopCount(v);
                Assert.Equal(expected, Bitcount.popcount(v));
            }
        }

        // ── popcount_Max15 ────────────────────────────────────────────────────

        [Fact]
        public void PopcountMax15_Zero_ReturnsZero()
        {
            Assert.Equal(0, Bitcount.popcount_Max15(0UL));
        }

        [Fact]
        public void PopcountMax15_OneBit_ReturnsOne()
        {
            Assert.Equal(1, Bitcount.popcount_Max15(1UL));
        }

        [Theory]
        [InlineData(0x1UL,    1)]
        [InlineData(0x3UL,    2)]
        [InlineData(0x7UL,    3)]
        [InlineData(0xFFUL,   8)]
        [InlineData(0x7FFFUL, 15)]
        public void PopcountMax15_SmallValues_ReturnsCorrectCount(ulong value, int expected)
        {
            Assert.Equal(expected, Bitcount.popcount_Max15(value));
        }

        [Fact]
        public void PopcountMax15_AgreesWithPopcountForSmallCounts()
        {
            // popcount_Max15 is only valid for values with at most 15 bits set
            ulong[] testValues = {
                0UL, 1UL, 0xFUL, 0x55UL, 0x7FFFUL,
                0x100000001UL
            };
            foreach (ulong v in testValues)
                Assert.Equal(Bitcount.popcount(v), Bitcount.popcount_Max15(v));
        }
    }
}

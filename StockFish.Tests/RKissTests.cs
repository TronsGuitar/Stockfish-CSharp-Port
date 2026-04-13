using StockFishPortApp_5._0;
using Xunit;

namespace StockFish.Tests
{
    /// <summary>
    /// Tests for the RKISS pseudo-random number generator.
    /// </summary>
    public class RKissTests
    {
        [Fact]
        public void Constructor_DefaultSeed_DoesNotThrow()
        {
            var rng = new RKISS();
            Assert.NotNull(rng);
        }

        [Fact]
        public void Rand64_ReturnsDifferentValues()
        {
            var rng = new RKISS(1);
            ulong first  = rng.rand64();
            ulong second = rng.rand64();
            Assert.NotEqual(first, second);
        }

        [Fact]
        public void Rand64_SameSeed_ProducesSameSequence()
        {
            var rng1 = new RKISS(42);
            var rng2 = new RKISS(42);

            for (int i = 0; i < 100; i++)
                Assert.Equal(rng1.rand64(), rng2.rand64());
        }

        [Fact]
        public void Rand64_DifferentSeeds_ProduceDifferentSequences()
        {
            var rng1 = new RKISS(10);
            var rng2 = new RKISS(20);

            bool anyDifferent = false;
            for (int i = 0; i < 10; i++)
            {
                if (rng1.rand64() != rng2.rand64())
                {
                    anyDifferent = true;
                    break;
                }
            }
            Assert.True(anyDifferent);
        }

        [Fact]
        public void Rand32_ReturnsDifferentValues()
        {
            var rng = new RKISS(1);
            uint first  = rng.rand32();
            uint second = rng.rand32();
            Assert.NotEqual(first, second);
        }

        [Fact]
        public void Rand32_SameSeed_ProducesSameSequence()
        {
            var rng1 = new RKISS(7);
            var rng2 = new RKISS(7);

            for (int i = 0; i < 100; i++)
                Assert.Equal(rng1.rand32(), rng2.rand32());
        }

        [Fact]
        public void MagicRand_SameSeed_ProducesSameSequence()
        {
            var rng1 = new RKISS(5);
            var rng2 = new RKISS(5);

            for (int i = 0; i < 20; i++)
                Assert.Equal(rng1.magic_rand(i), rng2.magic_rand(i));
        }

        [Fact]
        public void MagicRand_ProducesValues_WithFewerBitsSet()
        {
            // magic_rand is designed to return sparse bit patterns (few bits set)
            var rng = new RKISS(73);
            int totalBits = 0;
            int samples = 64;
            for (int i = 0; i < samples; i++)
                totalBits += System.Numerics.BitOperations.PopCount(rng.magic_rand(i));

            double averageBits = (double)totalBits / samples;
            // magic_rand ands together three random values, so expected ~8 bits
            Assert.True(averageBits < 20, $"Expected sparse magic_rand output, got average {averageBits} bits set");
        }

        [Fact]
        public void Rand64_ProducesAllBitPositions_OverManyDraws()
        {
            // After many draws every bit should have been set at least once
            var rng = new RKISS(73);
            ulong seen = 0UL;
            for (int i = 0; i < 1000; i++)
                seen |= rng.rand64();

            Assert.Equal(ulong.MaxValue, seen);
        }

        [Fact]
        public void SeedZero_DoesNotThrow()
        {
            var rng = new RKISS(0);
            ulong v = rng.rand64();
            Assert.True(v != 0 || v == 0); // just ensure no exception
        }

        [Fact]
        public void LargeSeed_DoesNotThrow()
        {
            var rng = new RKISS(200);
            ulong v = rng.rand64();
            Assert.True(v != 0 || v == 0);
        }
    }
}

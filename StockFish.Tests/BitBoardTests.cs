using StockFishPortApp_5._0;
using Xunit;

namespace StockFish.Tests
{
    /// <summary>
    /// xUnit collection fixture that initialises BitBoard lookup tables once
    /// per test run.  All test classes that use BitBoard functions should
    /// belong to the "BitBoardInit" collection.
    /// </summary>
    [CollectionDefinition("BitBoardInit")]
    public class BitBoardCollection : ICollectionFixture<BitBoardFixture> { }

    public class BitBoardFixture
    {
        public BitBoardFixture()
        {
            BitBoard.init();
        }
    }

    /// <summary>
    /// Tests for BitBoard static helpers (msb, lsb, pop_lsb, more_than_one,
    /// shift_bb, file/rank/distance, and various bitboard lookups).
    /// All tests require BitBoard.init() to have been called first.
    /// </summary>
    [Collection("BitBoardInit")]
    public class BitBoardTests
    {
        // ── msb ───────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(1UL, 0)]           // only bit 0 set
        [InlineData(2UL, 1)]           // only bit 1 set
        [InlineData(0x80UL, 7)]        // only bit 7 set
        [InlineData(0x8000000000000000UL, 63)] // only MSB set
        [InlineData(0xFFUL, 7)]        // bits 0-7, MSB is 7
        [InlineData(0x100UL, 8)]
        public void Msb_ReturnsIndexOfHighestSetBit(ulong value, int expected)
        {
            Assert.Equal(expected, BitBoard.msb(value));
        }

        // ── lsb ───────────────────────────────────────────────────────────────

        [Theory]
        [InlineData(1UL,  0)]
        [InlineData(2UL,  1)]
        [InlineData(4UL,  2)]
        [InlineData(0x8000000000000000UL, 63)]
        [InlineData(0xFFFFFFFFFFFFFFFFUL, 0)]
        [InlineData(0x100UL, 8)]
        public void Lsb_ReturnsIndexOfLowestSetBit(ulong value, int expected)
        {
            Assert.Equal(expected, BitBoard.lsb(value));
        }

        // ── pop_lsb ───────────────────────────────────────────────────────────

        [Fact]
        public void PopLsb_RemovesLowestBit()
        {
            ulong bb = 0b1010UL; // bits 1 and 3 set
            int lsb = BitBoard.pop_lsb(ref bb);
            Assert.Equal(1, lsb);       // lowest was bit 1
            Assert.Equal(0b1000UL, bb); // bit 1 cleared
        }

        [Fact]
        public void PopLsb_SingleBit_LeavesZero()
        {
            ulong bb = 1UL << 5;
            int lsb = BitBoard.pop_lsb(ref bb);
            Assert.Equal(5, lsb);
            Assert.Equal(0UL, bb);
        }

        [Fact]
        public void PopLsb_IterateOverAllBits()
        {
            ulong bb = 0b10110101UL; // bits 0, 2, 4, 5, 7
            int[] expected = { 0, 2, 4, 5, 7 };
            int i = 0;
            while (bb != 0)
            {
                int s = BitBoard.pop_lsb(ref bb);
                Assert.Equal(expected[i++], s);
            }
            Assert.Equal(5, i);
        }

        // ── more_than_one ─────────────────────────────────────────────────────

        [Theory]
        [InlineData(0UL, false)]
        [InlineData(1UL, false)]
        [InlineData(2UL, false)]
        [InlineData(3UL, true)]
        [InlineData(0xFFFFFFFFFFFFFFFFUL, true)]
        public void MoreThanOne_ReturnsExpected(ulong value, bool expected)
        {
            Assert.Equal(expected, BitBoard.more_than_one(value));
        }

        // ── SquareBB ──────────────────────────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_A1, 0)]
        [InlineData(SquareS.SQ_H8, 63)]
        [InlineData(SquareS.SQ_E4, 28)]
        public void SquareBB_ContainsExactlyOneBit(int square, int expectedBitPos)
        {
            ulong bb = BitBoard.SquareBB[square];
            Assert.Equal(1UL << expectedBitPos, bb);
        }

        // ── FileBB / RankBB ───────────────────────────────────────────────────

        [Fact]
        public void FileBB_FileA_HasEightSquares()
        {
            ulong bb = BitBoard.FileBB[FileS.FILE_A];
            Assert.Equal(8, Bitcount.popcount(bb));
        }

        [Fact]
        public void RankBB_Rank1_HasEightSquares()
        {
            ulong bb = BitBoard.RankBB[RankS.RANK_1];
            Assert.Equal(8, Bitcount.popcount(bb));
        }

        [Fact]
        public void RankBB_AllRanks_Cover64Squares()
        {
            ulong all = 0UL;
            for (int r = RankS.RANK_1; r <= RankS.RANK_8; r++)
                all |= BitBoard.RankBB[r];
            Assert.Equal(ulong.MaxValue, all);
        }

        // ── shift_bb ──────────────────────────────────────────────────────────

        [Fact]
        public void ShiftBb_DeltaN_MovesNorthOneRank()
        {
            ulong rank1 = BitBoard.RankBB[RankS.RANK_1];
            ulong rank2 = BitBoard.RankBB[RankS.RANK_2];
            Assert.Equal(rank2, BitBoard.shift_bb(rank1, SquareS.DELTA_N));
        }

        [Fact]
        public void ShiftBb_DeltaS_MovesSouthOneRank()
        {
            ulong rank2 = BitBoard.RankBB[RankS.RANK_2];
            ulong rank1 = BitBoard.RankBB[RankS.RANK_1];
            Assert.Equal(rank1, BitBoard.shift_bb(rank2, SquareS.DELTA_S));
        }

        // ── rank_bb / file_bb helpers ─────────────────────────────────────────

        [Fact]
        public void RankBbSquare_ReturnsRankOfSquare()
        {
            ulong expected = BitBoard.RankBB[RankS.RANK_4];
            Assert.Equal(expected, BitBoard.rank_bb_square(SquareS.SQ_E4));
        }

        [Fact]
        public void FileBbSquare_ReturnsFileOfSquare()
        {
            ulong expected = BitBoard.FileBB[FileS.FILE_A];
            Assert.Equal(expected, BitBoard.file_bb_square(SquareS.SQ_A1));
        }

        // ── BitboardAndSquare / BitboardOrSquare / BitboardXorSquare ──────────

        [Fact]
        public void BitboardAndSquare_SquarePresent_ReturnsNonZero()
        {
            ulong fullBoard = ulong.MaxValue;
            ulong result = BitBoard.BitboardAndSquare(fullBoard, SquareS.SQ_E4);
            Assert.NotEqual(0UL, result);
        }

        [Fact]
        public void BitboardAndSquare_SquareAbsent_ReturnsZero()
        {
            ulong empty = 0UL;
            ulong result = BitBoard.BitboardAndSquare(empty, SquareS.SQ_E4);
            Assert.Equal(0UL, result);
        }

        [Fact]
        public void BitboardOrSquare_AddsSquareBit()
        {
            ulong bb = 0UL;
            ulong result = BitBoard.BitboardOrSquare(bb, SquareS.SQ_A1);
            Assert.Equal(BitBoard.SquareBB[SquareS.SQ_A1], result);
        }

        [Fact]
        public void BitboardXorSquare_TogglesSquareBit()
        {
            ulong bb = ulong.MaxValue;
            ulong result = BitBoard.BitboardXorSquare(bb, SquareS.SQ_A1);
            Assert.Equal(ulong.MaxValue ^ BitBoard.SquareBB[SquareS.SQ_A1], result);
        }

        // ── file_distance / rank_distance ─────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_H1, 7)]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_A1, 0)]
        [InlineData(SquareS.SQ_D4, SquareS.SQ_G4, 3)]
        public void FileDistance_ReturnsAbsoluteFileDiff(int s1, int s2, int expected)
        {
            Assert.Equal(expected, BitBoard.file_distance(s1, s2));
        }

        [Theory]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_A8, 7)]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_A1, 0)]
        [InlineData(SquareS.SQ_E4, SquareS.SQ_E6, 2)]
        public void RankDistance_ReturnsAbsoluteRankDiff(int s1, int s2, int expected)
        {
            Assert.Equal(expected, BitBoard.rank_distance(s1, s2));
        }

        // ── SquareDistance ────────────────────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_A1, 0)]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_B2, 1)]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_H8, 7)]
        public void SquareDistance_ReturnsChebychevDistance(int s1, int s2, int expected)
        {
            Assert.Equal(expected, BitBoard.square_distance(s1, s2));
        }

        // ── pretty ────────────────────────────────────────────────────────────

        [Fact]
        public void Pretty_EmptyBoard_ContainsDashes()
        {
            string result = BitBoard.pretty(0UL);
            Assert.Contains("---", result);
            Assert.DoesNotContain("X", result);
        }

        [Fact]
        public void Pretty_FullBoard_ContainsX()
        {
            string result = BitBoard.pretty(ulong.MaxValue);
            Assert.Contains("X", result);
        }
    }

    /// <summary>
    /// TranspositionTable tests that rely on correctly initialised BitBoard
    /// tables (msb/lsb needed by TranspositionTable.resize).
    /// </summary>
    [Collection("BitBoardInit")]
    public class TranspositionTableWithBitBoardTests
    {
        private TranspositionTable CreateTable(ulong mbSize = 1)
        {
            var tt = new TranspositionTable();
            tt.resize(mbSize);
            return tt;
        }

        [Fact]
        public void Resize_1MB_AllocatesCorrectSizedTable()
        {
            var tt = CreateTable(1);
            // With correct msb the table should be a non-trivial size
            Assert.True(tt.table.Length >= (int)TranspositionTable.ClusterSize);
        }

        [Fact]
        public void StoreAndProbe_ReturnsCorrectDepth()
        {
            var tt = CreateTable(1);
            ulong key  = 0xFEDCBA9876543210UL;
            int   depth = DepthS.ONE_PLY * 10;
            tt.store(key, 300, BoundS.BOUND_EXACT, depth,
                     Types.make_move(SquareS.SQ_C3, SquareS.SQ_E4), 250);

            var entry = tt.probe(key);
            Assert.NotNull(entry);
            Assert.Equal(depth, entry.depth());
            Assert.Equal(300, entry.value());
        }

        [Fact]
        public void NewSearch_DoesNotInvalidateExistingEntries()
        {
            var tt = CreateTable(1);
            ulong key = 0x1234567890ABCDEFUL;
            tt.store(key, 50, BoundS.BOUND_LOWER, DepthS.ONE_PLY * 3,
                     Types.make_move(SquareS.SQ_A2, SquareS.SQ_A4), 30);

            tt.new_search();

            // Entry should still be found (generation8 is updated in probe)
            var entry = tt.probe(key);
            Assert.NotNull(entry);
        }

        [Fact]
        public void Clear_AfterStore_ProbeReturnsNull()
        {
            var tt = CreateTable(1);
            ulong key = 0xCAFEBABEDEADBEEFUL;
            tt.store(key, 10, BoundS.BOUND_UPPER, DepthS.ONE_PLY,
                     Types.make_move(SquareS.SQ_H7, SquareS.SQ_H8), 5);

            tt.clear();

            Assert.Null(tt.probe(key));
        }
    }
}

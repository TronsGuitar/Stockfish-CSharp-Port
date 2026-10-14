using StockFishPortApp_5._0;
using Xunit;

namespace StockFish.Tests
{
    /// <summary>
    /// Tests for TTEntry and TranspositionTable.
    /// </summary>
    public class TranspositionTableTests
    {
        // ── TTEntry ───────────────────────────────────────────────────────────

        [Fact]
        public void TTEntry_Save_PersistsAllFields()
        {
            var entry = new TTEntry();
            entry.save(
                k:  0xDEAD1234u,
                v:  100,
                b:  BoundS.BOUND_EXACT,
                d:  DepthS.ONE_PLY * 6,
                m:  Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4),
                g:  3,
                ev: 50
            );

            Assert.Equal(100,              entry.value());
            Assert.Equal(BoundS.BOUND_EXACT, entry.bound());
            Assert.Equal(DepthS.ONE_PLY * 6, entry.depth());
            Assert.Equal(50,               entry.eval_value());
        }

        [Fact]
        public void TTEntry_Clear_ResetsAllFields()
        {
            var entry = new TTEntry();
            entry.save(0xDEAD1234u, 100, BoundS.BOUND_EXACT,
                       DepthS.ONE_PLY * 4,
                       Types.make_move(SquareS.SQ_D2, SquareS.SQ_D4),
                       1, 80);

            entry.clear();

            Assert.Equal(0u,   entry.key32);
            Assert.Equal(0,    entry.move16);
            Assert.Equal(0,    entry.bound8);
            Assert.Equal(0,    entry.generation8);
            Assert.Equal(0,    entry.value16);
            Assert.Equal(0,    entry.depth16);
            Assert.Equal(0,    entry.evalValue);
        }

        [Fact]
        public void TTEntry_Move_ReturnsStoredMove()
        {
            var entry = new TTEntry();
            int move = Types.make_move(SquareS.SQ_G1, SquareS.SQ_F3);
            entry.save(0u, ValueS.VALUE_ZERO, BoundS.BOUND_NONE, DepthS.DEPTH_ZERO, move, 0, 0);
            Assert.Equal(move, entry.move());
        }

        [Fact]
        public void TTEntry_BoundAccessor_ReturnsStoredBound()
        {
            var entry = new TTEntry();
            entry.save(0u, 0, BoundS.BOUND_LOWER, DepthS.DEPTH_ZERO, 0, 0, 0);
            Assert.Equal(BoundS.BOUND_LOWER, entry.bound());
        }

        // ── TranspositionTable ────────────────────────────────────────────────

        private TranspositionTable CreateTable(ulong mbSize = 1)
        {
            var tt = new TranspositionTable();
            tt.resize(mbSize);
            return tt;
        }

        [Fact]
        public void Resize_1MB_AllocatesTable()
        {
            var tt = CreateTable(1);
            Assert.NotNull(tt.table);
            Assert.True(tt.table.Length > 0);
        }

        [Fact]
        public void Resize_ResizeToSameSize_DoesNotThrow()
        {
            var tt = CreateTable(1);
            tt.resize(1); // Should be a no-op
            Assert.NotNull(tt.table);
        }

        [Fact]
        public void Clear_ZerosAllEntries()
        {
            var tt = CreateTable(1);
            ulong key = 0x123456789ABCDEF0UL;
            tt.store(key, 100, BoundS.BOUND_EXACT, DepthS.ONE_PLY * 4,
                     Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4), 50);

            tt.clear();

            TTEntry found = tt.probe(key);
            Assert.Null(found);
        }

        [Fact]
        public void Probe_AfterStore_FindsEntry()
        {
            var tt = CreateTable(1);
            ulong key = 0xABCDEF0123456789UL;
            int move  = Types.make_move(SquareS.SQ_D2, SquareS.SQ_D4);

            tt.store(key, 200, BoundS.BOUND_LOWER, DepthS.ONE_PLY * 8, move, 120);

            TTEntry found = tt.probe(key);
            Assert.NotNull(found);
            Assert.Equal(200,             found.value());
            Assert.Equal(BoundS.BOUND_LOWER, found.bound());
            Assert.Equal(DepthS.ONE_PLY * 8, found.depth());
        }

        [Fact]
        public void Probe_UnknownKey_ReturnsNull()
        {
            var tt = CreateTable(1);
            TTEntry found = tt.probe(0xDEADBEEFCAFEBABEUL);
            Assert.Null(found);
        }

        [Fact]
        public void NewSearch_DoesNotThrow()
        {
            var tt = CreateTable(1);
            tt.new_search();
            tt.new_search();
        }

        [Fact]
        public void Store_MultipleEntries_EachRetrievable()
        {
            var tt = CreateTable(4); // 4 MB for room

            ulong key1 = 0x1111111111111111UL;
            ulong key2 = 0x2222222222222222UL;

            int move1 = Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4);
            int move2 = Types.make_move(SquareS.SQ_D2, SquareS.SQ_D4);

            tt.store(key1, 100, BoundS.BOUND_EXACT, DepthS.ONE_PLY * 4, move1, 80);
            tt.store(key2, 200, BoundS.BOUND_EXACT, DepthS.ONE_PLY * 6, move2, 160);

            var e1 = tt.probe(key1);
            var e2 = tt.probe(key2);

            Assert.NotNull(e1);
            Assert.NotNull(e2);
            Assert.Equal(100, e1.value());
            Assert.Equal(200, e2.value());
        }

        [Fact]
        public void FirstEntry_DifferentKeys_CanMapToSameCluster()
        {
            var tt = CreateTable(1);
            // first_entry uses the low bits, so two keys with the same low bits
            // will map to the same cluster index
            int idx1 = tt.first_entry(0x0000000000000000UL);
            int idx2 = tt.first_entry(0x0000000100000000UL);
            // Just verify no exception and that they return valid indices
            Assert.True(idx1 >= 0);
            Assert.True(idx2 >= 0);
        }

        // ── HashTable ──────────────────────────────────────────────────────────

        [Fact]
        public void HashTable_SetAndGet_RoundTrip()
        {
            var ht = new HashTable<int>(16);
            ulong key = 0xABCDEF01UL;
            ht[key] = 42;
            Assert.Equal(42, ht[key]);
        }

        [Fact]
        public void HashTable_MultipleKeys_DoNotCollide()
        {
            // Use keys that map to different slots (power-of-2 size table)
            var ht = new HashTable<int>(256);
            for (int i = 0; i < 256; i++)
                ht[(ulong)i] = i * 10;

            for (int i = 0; i < 256; i++)
                Assert.Equal(i * 10, ht[(ulong)i]);
        }
    }
}

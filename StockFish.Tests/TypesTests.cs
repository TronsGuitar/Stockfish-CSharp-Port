using StockFishPortApp_5._0;
using Xunit;

namespace StockFish.Tests
{
    /// <summary>
    /// Tests for the Types utility class and related structs.
    /// </summary>
    public class TypesTests
    {
        // ── make_square / file_of / rank_of ──────────────────────────────────

        [Theory]
        [InlineData(FileS.FILE_A, RankS.RANK_1, SquareS.SQ_A1)]
        [InlineData(FileS.FILE_H, RankS.RANK_1, SquareS.SQ_H1)]
        [InlineData(FileS.FILE_A, RankS.RANK_8, SquareS.SQ_A8)]
        [InlineData(FileS.FILE_H, RankS.RANK_8, SquareS.SQ_H8)]
        [InlineData(FileS.FILE_E, RankS.RANK_4, SquareS.SQ_E4)]
        [InlineData(FileS.FILE_D, RankS.RANK_5, SquareS.SQ_D5)]
        public void MakeSquare_ReturnsCorrectSquare(int file, int rank, int expectedSquare)
        {
            Assert.Equal(expectedSquare, Types.make_square(file, rank));
        }

        [Theory]
        [InlineData(SquareS.SQ_A1, FileS.FILE_A)]
        [InlineData(SquareS.SQ_H1, FileS.FILE_H)]
        [InlineData(SquareS.SQ_E4, FileS.FILE_E)]
        [InlineData(SquareS.SQ_D5, FileS.FILE_D)]
        public void FileOf_ReturnsCorrectFile(int square, int expectedFile)
        {
            Assert.Equal(expectedFile, Types.file_of(square));
        }

        [Theory]
        [InlineData(SquareS.SQ_A1, RankS.RANK_1)]
        [InlineData(SquareS.SQ_A8, RankS.RANK_8)]
        [InlineData(SquareS.SQ_E4, RankS.RANK_4)]
        [InlineData(SquareS.SQ_D5, RankS.RANK_5)]
        public void RankOf_ReturnsCorrectRank(int square, int expectedRank)
        {
            Assert.Equal(expectedRank, Types.rank_of(square));
        }

        // ── make_square round-trip ────────────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_A1)]
        [InlineData(SquareS.SQ_H8)]
        [InlineData(SquareS.SQ_E4)]
        [InlineData(SquareS.SQ_D5)]
        [InlineData(SquareS.SQ_G2)]
        public void MakeSquare_RoundTrip(int square)
        {
            int rebuilt = Types.make_square(Types.file_of(square), Types.rank_of(square));
            Assert.Equal(square, rebuilt);
        }

        // ── make_piece / type_of_piece / color_of ────────────────────────────

        [Theory]
        [InlineData(ColorS.WHITE, PieceTypeS.PAWN,   PieceS.W_PAWN)]
        [InlineData(ColorS.WHITE, PieceTypeS.KNIGHT, PieceS.W_KNIGHT)]
        [InlineData(ColorS.WHITE, PieceTypeS.BISHOP, PieceS.W_BISHOP)]
        [InlineData(ColorS.WHITE, PieceTypeS.ROOK,   PieceS.W_ROOK)]
        [InlineData(ColorS.WHITE, PieceTypeS.QUEEN,  PieceS.W_QUEEN)]
        [InlineData(ColorS.WHITE, PieceTypeS.KING,   PieceS.W_KING)]
        [InlineData(ColorS.BLACK, PieceTypeS.PAWN,   PieceS.B_PAWN)]
        [InlineData(ColorS.BLACK, PieceTypeS.KING,   PieceS.B_KING)]
        public void MakePiece_ReturnsCorrectPiece(int color, int pieceType, int expectedPiece)
        {
            Assert.Equal(expectedPiece, Types.make_piece(color, pieceType));
        }

        [Theory]
        [InlineData(PieceS.W_PAWN,   PieceTypeS.PAWN)]
        [InlineData(PieceS.W_KNIGHT, PieceTypeS.KNIGHT)]
        [InlineData(PieceS.W_BISHOP, PieceTypeS.BISHOP)]
        [InlineData(PieceS.W_ROOK,   PieceTypeS.ROOK)]
        [InlineData(PieceS.W_QUEEN,  PieceTypeS.QUEEN)]
        [InlineData(PieceS.W_KING,   PieceTypeS.KING)]
        [InlineData(PieceS.B_PAWN,   PieceTypeS.PAWN)]
        [InlineData(PieceS.B_KING,   PieceTypeS.KING)]
        public void TypeOfPiece_ReturnsCorrectType(int piece, int expectedType)
        {
            Assert.Equal(expectedType, Types.type_of_piece(piece));
        }

        [Theory]
        [InlineData(PieceS.W_PAWN,   ColorS.WHITE)]
        [InlineData(PieceS.W_KING,   ColorS.WHITE)]
        [InlineData(PieceS.B_PAWN,   ColorS.BLACK)]
        [InlineData(PieceS.B_KING,   ColorS.BLACK)]
        [InlineData(PieceS.B_QUEEN,  ColorS.BLACK)]
        public void ColorOf_ReturnsCorrectColor(int piece, int expectedColor)
        {
            Assert.Equal(expectedColor, Types.color_of(piece));
        }

        // ── is_ok_square ─────────────────────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_A1, true)]
        [InlineData(SquareS.SQ_H8, true)]
        [InlineData(SquareS.SQ_E4, true)]
        [InlineData(SquareS.SQ_NONE, false)]
        [InlineData(-1, false)]
        public void IsOkSquare_ReturnsExpected(int square, bool expected)
        {
            Assert.Equal(expected, Types.is_ok_square(square));
        }

        // ── make_move / from_sq / to_sq ───────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_E2, SquareS.SQ_E4)]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_H8)]
        [InlineData(SquareS.SQ_D5, SquareS.SQ_D6)]
        public void MakeMove_FromSq_ToSq_RoundTrip(int from, int to)
        {
            int move = Types.make_move(from, to);
            Assert.Equal(from, Types.from_sq(move));
            Assert.Equal(to,   Types.to_sq(move));
        }

        // ── is_ok_move ───────────────────────────────────────────────────────

        [Fact]
        public void IsOkMove_MoveNone_ReturnsFalse()
        {
            Assert.False(Types.is_ok_move(MoveS.MOVE_NONE));
        }

        [Fact]
        public void IsOkMove_MoveNull_ReturnsFalse()
        {
            Assert.False(Types.is_ok_move(MoveS.MOVE_NULL));
        }

        [Fact]
        public void IsOkMove_NormalMove_ReturnsTrue()
        {
            int move = Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4);
            Assert.True(Types.is_ok_move(move));
        }

        // ── type_of_move / promotion_type ────────────────────────────────────

        [Fact]
        public void TypeOfMove_NormalMove_ReturnsNormal()
        {
            int move = Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4);
            Assert.Equal(MoveTypeS.NORMAL, Types.type_of_move(move));
        }

        [Fact]
        public void TypeOfMove_PromotionMove_ReturnsPromotion()
        {
            int move = Types.make(SquareS.SQ_E7, SquareS.SQ_E8, MoveTypeS.PROMOTION, PieceTypeS.QUEEN);
            Assert.Equal(MoveTypeS.PROMOTION, Types.type_of_move(move));
        }

        [Fact]
        public void PromotionType_QueenPromotion_ReturnsQueen()
        {
            int move = Types.make(SquareS.SQ_E7, SquareS.SQ_E8, MoveTypeS.PROMOTION, PieceTypeS.QUEEN);
            Assert.Equal(PieceTypeS.QUEEN, Types.promotion_type(move));
        }

        [Theory]
        [InlineData(PieceTypeS.KNIGHT)]
        [InlineData(PieceTypeS.BISHOP)]
        [InlineData(PieceTypeS.ROOK)]
        [InlineData(PieceTypeS.QUEEN)]
        public void PromotionType_AllPromotionPieces_RoundTrip(int pt)
        {
            int move = Types.make(SquareS.SQ_A7, SquareS.SQ_A8, MoveTypeS.PROMOTION, pt);
            Assert.Equal(pt, Types.promotion_type(move));
        }

        // ── make_score / mg_value / eg_value ─────────────────────────────────

        [Theory]
        [InlineData(100, 200)]
        [InlineData(0, 0)]
        [InlineData(-50, -100)]
        [InlineData(ValueS.PawnValueMg, ValueS.PawnValueEg)]
        public void MakeScore_MgValue_EgValue_RoundTrip(int mg, int eg)
        {
            int score = Types.make_score(mg, eg);
            Assert.Equal(mg, Types.mg_value(score));
            Assert.Equal(eg, Types.eg_value(score));
        }

        [Fact]
        public void ScoreZero_BothValuesAreZero()
        {
            Assert.Equal(0, Types.mg_value(ScoreS.SCORE_ZERO));
            Assert.Equal(0, Types.eg_value(ScoreS.SCORE_ZERO));
        }

        // ── relative_square ───────────────────────────────────────────────────

        [Fact]
        public void RelativeSquare_WhiteE1_IsE1()
        {
            // From WHITE's perspective, E1 stays E1
            Assert.Equal(SquareS.SQ_E1, Types.relative_square(ColorS.WHITE, SquareS.SQ_E1));
        }

        [Fact]
        public void RelativeSquare_BlackE1_IsE8()
        {
            // From BLACK's perspective, E1 maps to E8
            Assert.Equal(SquareS.SQ_E8, Types.relative_square(ColorS.BLACK, SquareS.SQ_E1));
        }

        [Fact]
        public void RelativeSquare_BlackE8_IsE1()
        {
            Assert.Equal(SquareS.SQ_E1, Types.relative_square(ColorS.BLACK, SquareS.SQ_E8));
        }

        // ── notColor ──────────────────────────────────────────────────────────

        [Fact]
        public void NotColor_White_IsBlack()
        {
            Assert.Equal(ColorS.BLACK, Types.notColor(ColorS.WHITE));
        }

        [Fact]
        public void NotColor_Black_IsWhite()
        {
            Assert.Equal(ColorS.WHITE, Types.notColor(ColorS.BLACK));
        }

        // ── pawn_push ────────────────────────────────────────────────────────

        [Fact]
        public void PawnPush_White_IsDeltaN()
        {
            Assert.Equal(SquareS.DELTA_N, Types.pawn_push(ColorS.WHITE));
        }

        [Fact]
        public void PawnPush_Black_IsDeltaS()
        {
            Assert.Equal(SquareS.DELTA_S, Types.pawn_push(ColorS.BLACK));
        }

        // ── file_to_char / rank_to_char ───────────────────────────────────────

        [Theory]
        [InlineData(FileS.FILE_A, 'a')]
        [InlineData(FileS.FILE_H, 'h')]
        [InlineData(FileS.FILE_E, 'e')]
        public void FileToChar_LowerCase(int file, char expected)
        {
            Assert.Equal(expected, Types.file_to_char(file));
        }

        [Theory]
        [InlineData(FileS.FILE_A, 'A')]
        [InlineData(FileS.FILE_H, 'H')]
        public void FileToChar_UpperCase(int file, char expected)
        {
            Assert.Equal(expected, Types.file_to_char(file, false));
        }

        [Theory]
        [InlineData(RankS.RANK_1, '1')]
        [InlineData(RankS.RANK_8, '8')]
        [InlineData(RankS.RANK_4, '4')]
        public void RankToChar_ReturnsCorrectChar(int rank, char expected)
        {
            Assert.Equal(expected, Types.rank_to_char(rank));
        }

        // ── square_to_string ─────────────────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_A1, "a1")]
        [InlineData(SquareS.SQ_H8, "h8")]
        [InlineData(SquareS.SQ_E4, "e4")]
        [InlineData(SquareS.SQ_D5, "d5")]
        public void SquareToString_ReturnsAlgebraicNotation(int square, string expected)
        {
            Assert.Equal(expected, Types.square_to_string(square));
        }

        // ── mate_in / mated_in ────────────────────────────────────────────────

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void MateIn_ReturnsCorrectValue(int ply)
        {
            Assert.Equal(ValueS.VALUE_MATE - ply, Types.mate_in(ply));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void MatedIn_ReturnsCorrectValue(int ply)
        {
            Assert.Equal(-ValueS.VALUE_MATE + ply, Types.mated_in(ply));
        }

        // ── opposite_colors ───────────────────────────────────────────────────

        [Theory]
        [InlineData(SquareS.SQ_A1, SquareS.SQ_B1, true)]   // A1 dark, B1 light → opposite
        [InlineData(SquareS.SQ_A1, SquareS.SQ_A2, true)]   // A1 dark, A2 light → opposite
        [InlineData(SquareS.SQ_A1, SquareS.SQ_B2, false)]  // A1 dark, B2 dark  → same
        [InlineData(SquareS.SQ_A1, SquareS.SQ_A3, false)]  // A1 dark, A3 dark  → same
        [InlineData(SquareS.SQ_H1, SquareS.SQ_A8, false)]  // H1 and A8 both odd rank+file sum → same
        public void OppositeColors_ReturnsExpected(int s1, int s2, bool expected)
        {
            Assert.Equal(expected, Types.opposite_colors(s1, s2));
        }

        // ── divScore ─────────────────────────────────────────────────────────

        [Fact]
        public void DivScore_DividesByInteger()
        {
            int score = Types.make_score(100, 200);
            int half  = Types.divScore(score, 2);
            Assert.Equal(50,  Types.mg_value(half));
            Assert.Equal(100, Types.eg_value(half));
        }

        // ── minThan ───────────────────────────────────────────────────────────

        [Fact]
        public void MinThan_SmallerFirst_ReturnsTrue()
        {
            ExtMove a = new ExtMove { value = 10 };
            ExtMove b = new ExtMove { value = 20 };
            Assert.True(Types.minThan(ref a, ref b));
        }

        [Fact]
        public void MinThan_LargerFirst_ReturnsFalse()
        {
            ExtMove a = new ExtMove { value = 20 };
            ExtMove b = new ExtMove { value = 10 };
            Assert.False(Types.minThan(ref a, ref b));
        }

        // ── orCastlingRight ───────────────────────────────────────────────────

        [Fact]
        public void OrCastlingRight_WhiteKingSide()
        {
            Assert.Equal(CastlingRightS.WHITE_OO,
                Types.orCastlingRight(ColorS.WHITE, CastlingSideS.KING_SIDE));
        }

        [Fact]
        public void OrCastlingRight_BlackQueenSide()
        {
            Assert.Equal(CastlingRightS.BLACK_OOO,
                Types.orCastlingRight(ColorS.BLACK, CastlingSideS.QUEEN_SIDE));
        }

        // ── relative_rank ─────────────────────────────────────────────────────

        [Fact]
        public void RelativeRankRank_White_SameRank()
        {
            Assert.Equal(RankS.RANK_1, Types.relative_rank_rank(ColorS.WHITE, RankS.RANK_1));
        }

        [Fact]
        public void RelativeRankRank_Black_Flipped()
        {
            // Black's rank 1 is White's rank 8
            Assert.Equal(RankS.RANK_8, Types.relative_rank_rank(ColorS.BLACK, RankS.RANK_1));
        }
    }
}

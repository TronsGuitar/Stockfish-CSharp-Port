using StockFishPortApp_5._0;
using Xunit;

namespace StockFish.Tests
{
    /// <summary>
    /// Tests for Notation helpers that do not require a live Position object.
    /// </summary>
    public class NotationTests
    {
        // ── score_to_uci ───────────────────────────────────────────────────────

        [Fact]
        public void ScoreToUci_Draw_ReturnsCpZero()
        {
            string result = Notation.score_to_uci(ValueS.VALUE_DRAW);
            Assert.StartsWith("cp ", result);
            Assert.Contains("0", result);
        }

        [Fact]
        public void ScoreToUci_PawnAdvantage_ReturnsCp()
        {
            string result = Notation.score_to_uci(ValueS.PawnValueEg);
            Assert.StartsWith("cp ", result);
        }

        [Fact]
        public void ScoreToUci_MateIn1_ReturnsMate()
        {
            int mateScore = Types.mate_in(2); // 2 plies = 1 full move
            string result = Notation.score_to_uci(mateScore);
            Assert.StartsWith("mate ", result);
        }

        [Fact]
        public void ScoreToUci_ValueAboveBeta_HasLowerBound()
        {
            int value = 300;
            int beta  = 200;
            string result = Notation.score_to_uci(value, -ValueS.VALUE_INFINITE, beta);
            Assert.Contains("lowerbound", result);
        }

        [Fact]
        public void ScoreToUci_ValueBelowAlpha_HasUpperBound()
        {
            int value = 100;
            int alpha = 200;
            string result = Notation.score_to_uci(value, alpha, ValueS.VALUE_INFINITE);
            Assert.Contains("upperbound", result);
        }

        [Fact]
        public void ScoreToUci_ValueWithinWindow_HasNoBound()
        {
            int result_str_value = 150;
            string result = Notation.score_to_uci(result_str_value, 100, 200);
            Assert.DoesNotContain("bound", result);
        }

        // ── move_to_uci ────────────────────────────────────────────────────────

        [Fact]
        public void MoveToUci_MoveNone_ReturnsNone()
        {
            Assert.Equal("(none)", Notation.move_to_uci(MoveS.MOVE_NONE, false));
        }

        [Fact]
        public void MoveToUci_MoveNull_Returns0000()
        {
            Assert.Equal("0000", Notation.move_to_uci(MoveS.MOVE_NULL, false));
        }

        [Fact]
        public void MoveToUci_NormalMove_ReturnsCoordinateNotation()
        {
            int move = Types.make_move(SquareS.SQ_E2, SquareS.SQ_E4);
            Assert.Equal("e2e4", Notation.move_to_uci(move, false));
        }

        [Fact]
        public void MoveToUci_QueenPromotion_AppendsLowerCaseQ()
        {
            int move = Types.make(SquareS.SQ_E7, SquareS.SQ_E8, MoveTypeS.PROMOTION, PieceTypeS.QUEEN);
            string result = Notation.move_to_uci(move, false);
            Assert.EndsWith("q", result);
        }

        [Theory]
        [InlineData(PieceTypeS.KNIGHT, 'n')]
        [InlineData(PieceTypeS.BISHOP, 'b')]
        [InlineData(PieceTypeS.ROOK,   'r')]
        [InlineData(PieceTypeS.QUEEN,  'q')]
        public void MoveToUci_PromotionPiece_AppendedCorrectly(int pt, char expectedSuffix)
        {
            int move = Types.make(SquareS.SQ_A7, SquareS.SQ_A8, MoveTypeS.PROMOTION, pt);
            string result = Notation.move_to_uci(move, false);
            Assert.Equal(expectedSuffix, result[result.Length - 1]);
        }

        [Fact]
        public void MoveToUci_AllSquareCombinations_ProduceCorrectStrings()
        {
            // Spot-check a set of typical moves
            Assert.Equal("a1h8", Notation.move_to_uci(Types.make_move(SquareS.SQ_A1, SquareS.SQ_H8), false));
            Assert.Equal("d2d4", Notation.move_to_uci(Types.make_move(SquareS.SQ_D2, SquareS.SQ_D4), false));
            Assert.Equal("g1f3", Notation.move_to_uci(Types.make_move(SquareS.SQ_G1, SquareS.SQ_F3), false));
        }

        // ── format (time) ─────────────────────────────────────────────────────

        [Fact]
        public void FormatTime_LessThanOneMinute_ShowsZeroMinutes()
        {
            string result = Notation.format(30000L); // 30 seconds
            Assert.Equal("00:30", result);
        }

        [Fact]
        public void FormatTime_OneMinute_ShowsCorrectly()
        {
            string result = Notation.format(60000L);
            Assert.Equal("01:00", result);
        }

        [Fact]
        public void FormatTime_OneHour_ShowsHourPrefixed()
        {
            string result = Notation.format(3600000L);
            Assert.Equal("1:00:00", result);
        }

        [Fact]
        public void FormatTime_Zero_ShowsDoubleZero()
        {
            string result = Notation.format(0L);
            Assert.Equal("00:00", result);
        }

        // ── format (value) ────────────────────────────────────────────────────

        [Fact]
        public void FormatValue_Zero_ShowsPlusMinus()
        {
            string result = Notation.format(ValueS.VALUE_ZERO);
            // VALUE_ZERO / PawnValueEg = 0.00, sign may vary
            Assert.Contains("0.00", result);
        }

        [Fact]
        public void FormatValue_PositivePawnValue_ShowsPositiveSign()
        {
            string result = Notation.format(ValueS.PawnValueEg);
            Assert.StartsWith("+", result);
        }

        [Fact]
        public void FormatValue_MateScore_ShowsHashPrefix()
        {
            int mateScore = Types.mate_in(1);
            string result = Notation.format(mateScore);
            Assert.StartsWith("#", result);
        }

        [Fact]
        public void FormatValue_MatedScore_ShowsNegativeHash()
        {
            int matedScore = Types.mated_in(1);
            string result = Notation.format(matedScore);
            Assert.StartsWith("-#", result);
        }
    }
}

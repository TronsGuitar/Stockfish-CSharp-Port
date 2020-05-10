﻿using System;
using Score = System.Int32;

namespace StockFish
{
    public sealed class PsqTab
    {
        public static Score S(int mg, int eg)
        {
            return Types.Make_score(mg, eg);
        }

        public static Score[][] PSQT = new Score[7][]{
            new Score[SquareS.SQUARE_NB]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            new Score[SquareS.SQUARE_NB]{ // Pawn
                S(  0, 0), S( 0, 0), S( 0, 0), S( 0, 0), S(0,  0), S( 0, 0), S( 0, 0), S(  0, 0),
                S(-20, 0), S( 0, 0), S( 0, 0), S( 0, 0), S(0,  0), S( 0, 0), S( 0, 0), S(-20, 0),
                S(-20, 0), S( 0, 0), S(10, 0), S(20, 0), S(20, 0), S(10, 0), S( 0, 0), S(-20, 0),
                S(-20, 0), S( 0, 0), S(20, 0), S(40, 0), S(40, 0), S(20, 0), S( 0, 0), S(-20, 0),
                S(-20, 0), S( 0, 0), S(10, 0), S(20, 0), S(20, 0), S(10, 0), S( 0, 0), S(-20, 0),
                S(-20, 0), S( 0, 0), S( 0, 0), S( 0, 0), S(0,  0), S( 0, 0), S( 0, 0), S(-20, 0),
                S(-20, 0), S( 0, 0), S( 0, 0), S( 0, 0), S(0,  0), S( 0, 0), S( 0, 0), S(-20, 0),
                S(  0, 0), S( 0, 0), S( 0, 0), S( 0, 0), S(0,  0), S( 0, 0), S( 0, 0), S(  0, 0)
            },
            new Score[SquareS.SQUARE_NB]{ // Knight
                S(-144,-98), S(-109,-83), S(-85,-51), S(-73,-16), S(-73,-16), S(-85,-51), S(-109,-83), S(-144,-98),
                S( -88,-68), S( -43,-53), S(-19,-21), S( -7, 14), S( -7, 14), S(-19,-21), S( -43,-53), S( -88,-68),
                S( -69,-53), S( -24,-38), S(  0, -6), S( 12, 29), S( 12, 29), S(  0, -6), S( -24,-38), S( -69,-53),
                S( -28,-42), S(  17,-27), S( 41,  5), S( 53, 40), S( 53, 40), S( 41,  5), S(  17,-27), S( -28,-42),
                S( -30,-42), S(  15,-27), S( 39,  5), S( 51, 40), S( 51, 40), S( 39,  5), S(  15,-27), S( -30,-42),
                S( -10,-53), S(  35,-38), S( 59, -6), S( 71, 29), S( 71, 29), S( 59, -6), S(  35,-38), S( -10,-53),
                S( -64,-68), S( -19,-53), S(  5,-21), S( 17, 14), S( 17, 14), S(  5,-21), S( -19,-53), S( -64,-68),
                S(-200,-98), S( -65,-83), S(-41,-51), S(-29,-16), S(-29,-16), S(-41,-51), S( -65,-83), S(-200,-98)
            },
            new Score[SquareS.SQUARE_NB]{ // Bishop
                S(-54,-65), S(-27,-42), S(-34,-44), S(-43,-26), S(-43,-26), S(-34,-44), S(-27,-42), S(-54,-65),
                S(-29,-43), S(  8,-20), S(  1,-22), S( -8, -4), S( -8, -4), S(  1,-22), S(  8,-20), S(-29,-43),
                S(-20,-33), S( 17,-10), S( 10,-12), S(  1,  6), S(  1,  6), S( 10,-12), S( 17,-10), S(-20,-33),
                S(-19,-35), S( 18,-12), S( 11,-14), S(  2,  4), S(  2,  4), S( 11,-14), S( 18,-12), S(-19,-35),
                S(-22,-35), S( 15,-12), S(  8,-14), S( -1,  4), S( -1,  4), S(  8,-14), S( 15,-12), S(-22,-35),
                S(-28,-33), S(  9,-10), S(  2,-12), S( -7,  6), S( -7,  6), S(  2,-12), S(  9,-10), S(-28,-33),
                S(-32,-43), S(  5,-20), S( -2,-22), S(-11, -4), S(-11, -4), S( -2,-22), S(  5,-20), S(-32,-43),
                S(-49,-65), S(-22,-42), S(-29,-44), S(-38,-26), S(-38,-26), S(-29,-44), S(-22,-42), S(-49,-65)
            },
            new Score[SquareS.SQUARE_NB]{ // Rook
                S(-22, 3), S(-17, 3), S(-12, 3), S(-8, 3), S(-8, 3), S(-12, 3), S(-17, 3), S(-22, 3),
                S(-22, 3), S( -7, 3), S( -2, 3), S( 2, 3), S( 2, 3), S( -2, 3), S( -7, 3), S(-22, 3),
                S(-22, 3), S( -7, 3), S( -2, 3), S( 2, 3), S( 2, 3), S( -2, 3), S( -7, 3), S(-22, 3),
                S(-22, 3), S( -7, 3), S( -2, 3), S( 2, 3), S( 2, 3), S( -2, 3), S( -7, 3), S(-22, 3),
                S(-22, 3), S( -7, 3), S( -2, 3), S( 2, 3), S( 2, 3), S( -2, 3), S( -7, 3), S(-22, 3),
                S(-22, 3), S( -7, 3), S( -2, 3), S( 2, 3), S( 2, 3), S( -2, 3), S( -7, 3), S(-22, 3),
                S(-11, 3), S(  4, 3), S(  9, 3), S(13, 3), S(13, 3), S(  9, 3), S(  4, 3), S(-11, 3),
                S(-22, 3), S(-17, 3), S(-12, 3), S(-8, 3), S(-8, 3), S(-12, 3), S(-17, 3), S(-22, 3)
            },
            new Score[SquareS.SQUARE_NB]{ // Queen
                S(-2,-80), S(-2,-54), S(-2,-42), S(-2,-30), S(-2,-30), S(-2,-42), S(-2,-54), S(-2,-80),
                S(-2,-54), S( 8,-30), S( 8,-18), S( 8, -6), S( 8, -6), S( 8,-18), S( 8,-30), S(-2,-54),
                S(-2,-42), S( 8,-18), S( 8, -6), S( 8,  6), S( 8,  6), S( 8, -6), S( 8,-18), S(-2,-42),
                S(-2,-30), S( 8, -6), S( 8,  6), S( 8, 18), S( 8, 18), S( 8,  6), S( 8, -6), S(-2,-30),
                S(-2,-30), S( 8, -6), S( 8,  6), S( 8, 18), S( 8, 18), S( 8,  6), S( 8, -6), S(-2,-30),
                S(-2,-42), S( 8,-18), S( 8, -6), S( 8,  6), S( 8,  6), S( 8, -6), S( 8,-18), S(-2,-42),
                S(-2,-54), S( 8,-30), S( 8,-18), S( 8, -6), S( 8, -6), S( 8,-18), S( 8,-30), S(-2,-54),
                S(-2,-80), S(-2,-54), S(-2,-42), S(-2,-30), S(-2,-30), S(-2,-42), S(-2,-54), S(-2,-80)
            },
            new Score[SquareS.SQUARE_NB]{ // King
                S(298, 27), S(332, 81), S(273,108), S(225,116), S(225,116), S(273,108), S(332, 81), S(298, 27),
                S(287, 74), S(321,128), S(262,155), S(214,163), S(214,163), S(262,155), S(321,128), S(287, 74),
                S(224,111), S(258,165), S(199,192), S(151,200), S(151,200), S(199,192), S(258,165), S(224,111),
                S(196,135), S(230,189), S(171,216), S(123,224), S(123,224), S(171,216), S(230,189), S(196,135),
                S(173,135), S(207,189), S(148,216), S(100,224), S(100,224), S(148,216), S(207,189), S(173,135),
                S(146,111), S(180,165), S(121,192), S( 73,200), S( 73,200), S(121,192), S(180,165), S(146,111),
                S(119, 74), S(153,128), S( 94,155), S( 46,163), S( 46,163), S( 94,155), S(153,128), S(119, 74),
                S( 98, 27), S(132, 81), S( 73,108), S( 25,116), S( 25,116), S( 73,108), S(132, 81), S( 98, 27)
            }
        };
    }
}

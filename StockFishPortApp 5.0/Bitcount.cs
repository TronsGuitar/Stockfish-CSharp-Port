using System;
using System.Runtime.CompilerServices;

using Bitboard = System.UInt64;

namespace StockFishPortApp_5._0
{
    public sealed class Bitcount
    {
        #if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        public static int popcount(Bitboard b)
        {
            b  = b - ((b >> 1) & 0x5555555555555555UL);
            b  = (b & 0x3333333333333333UL) + ((b >> 2) & 0x3333333333333333UL);
            b  = (b + (b >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
            return (int)((b * 0x0101010101010101UL) >> 56);
        }

        #if AGGR_INLINE
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #endif
        public static int popcount_Max15(Bitboard b)
        {
            b  = b - ((b >> 1) & 0x5555555555555555UL);
            b  = (b & 0x3333333333333333UL) + ((b >> 2) & 0x3333333333333333UL);
            return (int)((b * 0x1111111111111111UL) >> 60);
        }
    }
}

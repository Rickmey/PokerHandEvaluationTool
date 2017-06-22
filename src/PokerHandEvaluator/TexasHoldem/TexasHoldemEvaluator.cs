namespace PokerHandEvaluator.TexasHoldem
{
    /// <summary>
    /// A Texas Holdem evaluation algorithm based on the pokereval C code.
    /// Pokereval here: <a href="link">https://github.com/v2k/poker-eval</a>
    /// 
    /// In comparison to the original C# port <see cref="EvaluatorOriginal"/> this code is ~18% faster (<see cref="exhaustiveTexasHoldem()"/>).
    /// Depending on the specific hand this code can evaluate ~130mio hands per second.
    /// 
    /// The increase of performance is the result of multiple optimizations.
    /// - The value of a hand was calculated and stored even though it could be returned directly.
    /// - Kicker problem:
    ///     It's easy and fast to find trips in a hand like this: TTT A92.
    ///     The problem is to find the highest remaining two cards in the hand.
    ///     The original uses lookups in pregenerated tables and some bit operations (63 system calls).
    ///     This code removes the trips from the hand and looks up the remaining two bits (26 system calls).
    ///     As a nice side effect: the code is better readable and easier to understand. 
    /// </summary>
    internal static class Evaluator
    {
        #region Const

        // Bit position offset for the suit
        const int diamondOffset = 13;
        const int heartOffset = 26;
        const int spadeOffset = 39;
        const int fullRankSet = 0x1fff;
        const int pairOrHigherShift = 13;

        // use the last 4 bits for hand type
        const uint onePairHandValue = 0x10000000;       // 00010000000000000000000000000000
        const uint twoPairHandValue = 0x20000000;       // 00100000000000000000000000000000
        const uint tripsHandValue = 0x30000000;         // 00110000000000000000000000000000
        const uint straightHandValue = 0x40000000;      // 01000000000000000000000000000000
        const uint flushHandValue = 0x50000000;         // 01010000000000000000000000000000
        const uint fullHouseHandValue = 0x60000000;     // 01100000000000000000000000000000
        const uint quadsHandValue = 0x70000000;         // 01110000000000000000000000000000
        const uint straightFlushHandValue = 0x80000000; // 10000000000000000000000000000000

        #endregion Const

        /// <summary>
        /// Evaluates a Texas Holdem hand. Supports 1-7 cards hands.
        /// </summary>
        /// <param name="cardMask">ulong where every of the first 52 bits represents a card</param>
        /// <param name="numberOfCards">For performance. Must match the number of set bits in cardMask.</param>
        /// <returns>Value of the hand</returns>
        internal static uint GetHandValue(ulong cardMask, uint numberOfCards)
        {
            uint suitClub = (uint)(cardMask & fullRankSet);
            uint suitDiamond = (uint)((cardMask >> diamondOffset) & fullRankSet);
            uint suitHeart = (uint)((cardMask >> heartOffset) & fullRankSet);
            uint suitSpade = (uint)((cardMask >> spadeOffset) & fullRankSet);

            uint ranks = suitClub | suitDiamond | suitHeart | suitSpade;
            uint numberOfRanks = Utils.nBitsTable[ranks];

            // Check for straight, flush, or straight flush
            if (numberOfRanks >= 5)
            {
                if (Utils.nBitsTable[suitClub] >= 5)
                {
                    if (LookupTables.straightTable[suitClub] > 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitClub];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitClub];
                }
                else if (Utils.nBitsTable[suitDiamond] >= 5)
                {
                    if (LookupTables.straightTable[suitDiamond] > 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitDiamond];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitDiamond];
                }
                else if (Utils.nBitsTable[suitHeart] >= 5)
                {
                    if (LookupTables.straightTable[suitHeart] > 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitHeart];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitHeart];
                }
                else if (Utils.nBitsTable[suitSpade] >= 5)
                {
                    if (LookupTables.straightTable[suitSpade] > 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitSpade];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitSpade];
                }
                else
                {
                    uint straight = LookupTables.straightTable[ranks];
                    if (straight > 0)
                        return straightHandValue + straight;
                };
            }


            uint numberOfDuplicates = numberOfCards - numberOfRanks;
            uint twoMask, threeMask, topTwoMask;
            switch (numberOfDuplicates)
            {
                case 0:
                    // highcard
                    return LookupTables.top5OrLessBitTable[ranks];

                case 1:

                    // one pair
                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    // ranks & ~twoMask = remove pair from ranks and get highest 3 remaining cards
                    return onePairHandValue + (twoMask << pairOrHigherShift) + LookupTables.top3OrLessBitTable[ranks & ~twoMask];

                case 2:

                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    if (twoMask != 0)
                    {
                        // two pair
                        return twoPairHandValue + (twoMask << pairOrHigherShift) + LookupTables.top1OrLessBitTable[ranks & ~twoMask];
                    }

                    // trips
                    threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                    return tripsHandValue + (threeMask << pairOrHigherShift) + LookupTables.top2OrLessBitTable[ranks & ~threeMask];

                default:

                    uint fourMask = suitHeart & suitDiamond & suitClub & suitSpade;
                    if (fourMask != 0)
                    {
                        // quads
                        return quadsHandValue + (fourMask << pairOrHigherShift) + LookupTables.top1OrLessBitTable[ranks & ~fourMask];
                    };

                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    if (Utils.nBitsTable[twoMask] == numberOfDuplicates)
                    {
                        // two pair
                        // AA2233T
                        topTwoMask = LookupTables.top2OrLessBitTable[twoMask];
                        return twoPairHandValue + (topTwoMask << pairOrHigherShift) + LookupTables.top1OrLessBitTable[ranks & ~topTwoMask];
                    }

                    // fullhouse
                    threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                    // AAA222K  -> threeMask= A2; topThreeMask= A; (threeMask & ~topThreeMask)= 2; twoMask is 0
                    // AAA222   -> threeMask= A2; topThreeMask= A; (threeMask & ~topThreeMask)= 2; twoMask is 0
                    // AAA22K   -> threeMask= A;  topThreeMask= A; (threeMask & ~topThreeMask)= 0; twoMask is 2
                    // AAA2233  -> threeMask= A;  topThreeMask= A; (threeMask & ~topThreeMask)= 0; twoMask is 2,3
                    // (threeMask & ~topThreeMask) or twoMask is 0
                    uint topThreeMask = LookupTables.top1OrLessBitTable[threeMask];
                    return fullHouseHandValue + (topThreeMask << pairOrHigherShift) + (threeMask & ~topThreeMask) + LookupTables.top1OrLessBitTable[twoMask];
            }
        }
    }
}

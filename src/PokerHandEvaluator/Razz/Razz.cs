namespace PokerHandEvaluator.Razz
{
    internal static class Evaluator
    {
        #region Const

        // Bit position offset for the suit (club offset is missing because it's 0)
        const int diamondOffset = 13;
        const int heartOffset = 26;
        const int spadeOffset = 39;

        const int onePairHandValue = 0x04000000;
        const int twoPairHandValue = 0x08000000;
        const int tripsHandValue = 0x0C000000;
        const int fullHouseHandValue = 0x18000000;
        const int quadsHandValue = 0x1C000000;
        const uint fullRankSet = 0x1fff;

        /// <summary>
        /// To give pairs or higher card combinations a higher value they need to be shifted up.
        /// </summary>
        const int pairOrHigherShift = 13;

        #endregion Const

        /// <summary>
        /// Calculates the values of a Razz hand.
        /// Expects mask where aces are low (<see cref="ShiftAceToBottom(ulong)"/>).
        /// </summary>
        /// <param name="cardMask">Ace shifted card mask.</param>
        /// <param name="numberOfCards">0 - 7 cards.</param>
        /// <returns>Value of the Razz hand.</returns>
        public static uint GetHandValue(ulong cardMask, uint numberOfCards)
        {
            uint suitClub = (uint)(cardMask & fullRankSet);
            uint suitDiamond = (uint)((cardMask >> diamondOffset) & fullRankSet);
            uint suitHeart = (uint)((cardMask >> heartOffset) & fullRankSet);
            uint suitSpade = (uint)((cardMask >> spadeOffset) & fullRankSet);

            uint ranks = suitClub | suitDiamond | suitHeart | suitSpade;
            uint numberOfRanks = Utils.nBitsTable[ranks];

            // More than 4 different cards means a full low hand.
            // numberOfCards == numberOfRanks means all cards in hand are unique. This is important for hands with less than 5 cards.
            if (numberOfCards == numberOfRanks || numberOfRanks >= 5)
            {
                return LookupTables.bottom5OrLessBitTable[ranks];
            }

            uint twoMask;
            uint threeMask;
            uint fourMask;

            // use all cards in hand
            if (numberOfCards <= 5)
            {
                // number of duplicates
                switch (numberOfCards - numberOfRanks)
                {
                    case 1:
                        // one pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        return onePairHandValue + (twoMask << pairOrHigherShift) + ranks & ~twoMask;

                    case 2:
                        // two pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        if (twoMask != 0)
                            return twoPairHandValue + (twoMask << pairOrHigherShift) + ranks & ~twoMask;

                        // trips
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        return tripsHandValue + (threeMask << pairOrHigherShift) + ranks & ~threeMask;

                    default:
                        // quads
                        uint four_mask = suitHeart & suitDiamond & suitClub & suitSpade;
                        if (four_mask != 0)
                            return quadsHandValue + (four_mask << pairOrHigherShift) + ranks & ~four_mask;

                        // fullhouse
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        return fullHouseHandValue + (threeMask << pairOrHigherShift) + ranks & ~threeMask;
                }
            }

            // we have a 6 or 7 card hand here
            switch (5 - numberOfRanks)
            {

                case 1:
                    // one pair
                    // AAAA234
                    // AAA2234
                    // AA22334
                    fourMask = suitHeart & suitDiamond & suitClub & suitSpade;
                    if (fourMask != 0)
                        return onePairHandValue + (fourMask << pairOrHigherShift) + ranks & ~fourMask;

                    // AAA2234
                    threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                    if (threeMask != 0)
                        return onePairHandValue + (threeMask << pairOrHigherShift) + ranks & ~threeMask;

                    // AA22334
                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    var bottomMask = LookupTables.bottom1OrLessBitTable[twoMask];
                    return onePairHandValue + (bottomMask << pairOrHigherShift) + ranks & ~bottomMask;

                case 2:

                    // AAAA223 = 3; 2; 2 pair
                    // AAA2223 = 3; 2 pair
                    fourMask = suitHeart & suitDiamond & suitClub & suitSpade;
                    if (fourMask != 0)
                    {
                        // AAAA223
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        // twoMask detects quads
                        return twoPairHandValue + (fourMask << pairOrHigherShift) + ((twoMask & ~fourMask) << pairOrHigherShift) + (ranks & ~twoMask) & ~fourMask;
                    }

                    threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                    return twoPairHandValue + (threeMask << pairOrHigherShift) + ranks & ~threeMask;

                case 3:
                    // AAAA222 = 2; 3; Fullhouse
                    fourMask = suitHeart & suitDiamond & suitClub & suitSpade;
                    //if (fourMask != 0)
                    return fullHouseHandValue + (fourMask << pairOrHigherShift) + ranks & ~fourMask;
            }
            return 0;
        }
    }
}

namespace PokerHandEvaluator.HighVariants
{
    public static class HighVariant
    {
        #region Const

        // Bit position offset for the suit
        const int diamondOffset = 13;
        const int heartOffset = 26;
        const int spadeOffset = 39;
        const uint fullRankSet = 0x1fff;

        const int topCardShift = 16;
        const int secondCardShift = 12;
        const int thirdCardShift = 8;
        const int cardWidth = 4;
        const uint topCardMask = 0x000F0000;
        const uint secondCardMask = 0x0000F000;
        const uint fifthCardMask = 0x0000000F;

        const int onePairHandValue = 0x1000000;
        const int twoPairHandValue = 0x2000000;
        const int tripsHandValue = 0x3000000;
        const int straightHandValue = 0x4000000;
        const int flushHandValue = 0x5000000;
        const int fullHouseHandValue = 0x6000000;
        const int quadsHandValue = 0x7000000;
        const int straightFlushHandValue = 0x8000000;

        #endregion Const

        // TODO can't be used for Stud because suits matter
        public static uint GetHandValue(ulong cardMask, uint numberOfCards)
        {
            // Seperate out by suit;
            // 0x1fffUL = fullset 2 to Ace on position 0 to 12
            // shift current hands to bottom and remove set rest with & operator
            uint suitClub = (uint)(cardMask & fullRankSet);
            uint suitDiamond = (uint)((cardMask >> diamondOffset) & fullRankSet);
            uint suitHeart = (uint)((cardMask >> heartOffset) & fullRankSet);
            uint suitSpade = (uint)((cardMask >> spadeOffset) & fullRankSet);

            uint ranks = suitClub | suitDiamond | suitHeart | suitSpade;
            uint numberOfRanks = Utils.nBitsTable[ranks]; // number of different card values; 7 total cards with 1 pair = n_ranks 6
            uint numberOfDuplicates = numberOfCards - numberOfRanks;

            uint result = 0;
            // Check for straight, flush, or straight flush
            if (numberOfRanks >= 5)
            {
                if (Utils.nBitsTable[suitClub] >= 5)
                {
                    if (LookupTables.straightTable[suitClub] != 0)
                        return straightFlushHandValue + (uint)(LookupTables.straightTable[suitClub] << topCardShift);
                    result = flushHandValue + LookupTables.topFiveCardsTable[suitClub];
                }
                else if (Utils.nBitsTable[suitDiamond] >= 5)
                {
                    if (LookupTables.straightTable[suitDiamond] != 0)
                        return straightFlushHandValue + (uint)(LookupTables.straightTable[suitDiamond] << topCardShift);
                    result = flushHandValue + LookupTables.topFiveCardsTable[suitDiamond];
                }
                else if (Utils.nBitsTable[suitHeart] >= 5)
                {
                    if (LookupTables.straightTable[suitHeart] != 0)
                        return straightFlushHandValue + (uint)(LookupTables.straightTable[suitHeart] << topCardShift);
                    result = flushHandValue + LookupTables.topFiveCardsTable[suitHeart];
                }
                else if (Utils.nBitsTable[suitSpade] >= 5)
                {
                    if (LookupTables.straightTable[suitSpade] != 0)
                        return straightFlushHandValue + (uint)(LookupTables.straightTable[suitSpade] << topCardShift);
                    result = flushHandValue + LookupTables.topFiveCardsTable[suitSpade];
                }
                else
                {
                    //uint st = LookupTables.straightTable[ranks];
                    //if (st != 0)
                    //    result = straightHandValue + (st << topCardShift);
                    uint st = LookupTables.straightTable[ranks];
                    if (st != 0)
                        return straightHandValue + (st << topCardShift); // if a 7 card hand mades a straight nothing higher is possible
                };

                /* 
                   Another win -- if there can't be a FH/Quads (n_dups < 3), 
                   which is true most of the time when there is a made hand, then if we've
                   found a five card hand, just return.  This skips the whole process of
                   computing two_mask/three_mask/etc.
                */

                // 
                //if (result != 0 && numberOfDuplicates <= 2)
                //    return result;
                if (result != 0) // if we have a 5 card hand nothing more valuable is possible
                    return result;
            }


            uint threeMask;
            uint twoMask;

            /*
             * By the time we're here, either: 
               1) there's no five-card hand possible (flush or straight), or
               2) there's a flush or straight, but we know that there are enough
                  duplicates to make a full house / quads possible.  
             */
            switch (numberOfDuplicates)
            {
                case 0:
                    // highcard hand
                    return LookupTables.topFiveCardsTable[ranks];
                case 1:
                    {
                        // one pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        result = (uint)(onePairHandValue + (LookupTables.topCardTable[twoMask] << topCardShift));
                        uint t = ranks ^ twoMask;      /* Only one bit set in two_mask */
                                                       /* Get the top five cards in what is left, drop all but the top three 
                                                        * cards, and shift them by one to get the three desired kickers */
                        uint kickers = (LookupTables.topFiveCardsTable[t] >> cardWidth) & ~fifthCardMask;
                        result += kickers;
                        return result;
                    }

                case 2:
                    // Either two pair or trips
                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    if (twoMask != 0)
                    {
                        uint t = ranks ^ twoMask; // Exactly two bits set in two_mask
                        result = (uint)(twoPairHandValue + (LookupTables.topFiveCardsTable[twoMask] & (topCardMask | secondCardMask)) + (LookupTables.topCardTable[t] << thirdCardShift));
                        return result;
                    }
                    else
                    {
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        result = (uint)(tripsHandValue + (LookupTables.topCardTable[threeMask] << topCardShift));
                        uint t = ranks ^ threeMask; // Only one bit set in three_mask
                        uint second = LookupTables.topCardTable[t];
                        result += (second << secondCardShift);
                        t ^= (1U << (int)second);
                        result += (uint)(LookupTables.topCardTable[t] << thirdCardShift);
                        return result;
                    }

                default:
                    // Possible quads, fullhouse, straight or flush, or two pair
                    uint four_mask = suitHeart & suitDiamond & suitClub & suitSpade;
                    if (four_mask != 0)
                    {
                        uint tc = LookupTables.topCardTable[four_mask];
                        result = (uint)(quadsHandValue + (tc << topCardShift) + ((LookupTables.topCardTable[ranks ^ (1U << (int)tc)]) << secondCardShift));
                        return result;
                    };

                    /* Technically, three_mask as defined below is really the set of
                       bits which are set in three or four of the suits, but since
                       we've already eliminated quads, this is OK */
                    /* Similarly, two_mask is really two_or_four_mask, but since we've
                       already eliminated quads, we can use this shortcut */

                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    if (Utils.nBitsTable[twoMask] != numberOfDuplicates)
                    {
                        /* Must be some trips then, which really means there is a 
                           full house since n_dups >= 3 */
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        result = fullHouseHandValue;
                        uint tc = LookupTables.topCardTable[threeMask];
                        result += (tc << topCardShift);
                        uint t = (twoMask | threeMask) ^ (1U << (int)tc);
                        result += (uint)(LookupTables.topCardTable[t] << secondCardShift);
                        return result;
                    };

                    if (result != 0) /* flush and straight */
                        return result;
                    else
                    {
                        /* Must be two pair */
                        result = twoPairHandValue;
                        uint top = LookupTables.topCardTable[twoMask];
                        result += (top << topCardShift);
                        uint second = LookupTables.topCardTable[twoMask ^ (1 << (int)top)];
                        result += (second << secondCardShift);
                        result += (uint)((LookupTables.topCardTable[ranks ^ (1U << (int)top) ^ (1 << (int)second)]) << thirdCardShift);
                        return result;
                    }
            }
        }
    }
}

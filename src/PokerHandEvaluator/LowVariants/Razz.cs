namespace PokerHandEvaluator.LowVariants
{
    public static class Razz
    {
        #region Const

        // Bit position offset for the suit (club offset is missing because it's 0)
        const int diamondOffset = 13;
        const int heartOffset = 26;
        const int spadeOffset = 39;

        /// <summary>
        /// Set bits at position 13, 26, 39 and 52.
        /// 1000 00000000 01000000 00000010 00000000 00010000 00000000
        /// </summary>
        const ulong allAcesMask = 0x8004002001000UL;

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
        /// Shift aces from position 13 to position 1 and the rest 2 up.
        /// </summary>
        /// <param name="cards">Card mask with aces as top card.</param>
        /// <returns>Card mask with aces as bottom card.</returns>
        public static ulong ShiftAceToBottom(ulong cards)
        {
            return ((~(cards & allAcesMask) & cards) << 1) | ((cards & allAcesMask) >> 12);
        }

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
                return LookupTables.bottomFiveRanksTable[ranks];
            }

            uint twoMask;
            uint threeMask;

            // use all cards in hand
            if (numberOfCards <= 5)
            {
                // number of duplicates
                switch (numberOfCards - numberOfRanks)
                {
                    case 1:
                        // one pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        return (onePairHandValue + (twoMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks]);

                    case 2:
                        // two pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        if (twoMask != 0)
                            return (twoPairHandValue + (twoMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks]);

                        // trips
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        return (tripsHandValue + (threeMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks]);

                    default:
                        // quads
                        uint four_mask = suitHeart & suitDiamond & suitClub & suitSpade;
                        if (four_mask != 0)
                            return (quadsHandValue + (four_mask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks]);

                        // fullhouse
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        return (fullHouseHandValue + (threeMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks]);
                }
            }
            else
            {
                // we have a 6 or 7 card hand here
                switch (5 - numberOfRanks)
                {
                    case 1:
                        // one pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        return onePairHandValue + (twoMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks];

                    case 2:
                        // trips AAAA23
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        if (threeMask != 0)
                            return tripsHandValue + (threeMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks];

                        // two pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        return twoPairHandValue + (twoMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks];

                    default:
                        // full house
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        return fullHouseHandValue + (threeMask << pairOrHigherShift) + LookupTables.bottomFiveRanksTable[ranks];
                }
            }
        }


    }
}

/*
 Evaluation Process
    
    const int onePairHandValue = 0x04000000;    00000100 00000000 00000000 00000000
    const int twoPairHandValue = 0x08000000;    00001000 00000000 00000000 00000000
    const int tripsHandValue = 0x0C000000;      00001100 00000000 00000000 00000000
    const int fullHouseHandValue = 0x18000000;  00011000 00000000 00000000 00000000
    const int quadsHandValue = 0x1C000000;      00011100 00000000 00000000 00000000

    Evaluator.bottomFiveRanksTable[ranks] Max   00000000 00000000 00011111 11111111 
    mask K << 13                                00000010 00000000 00010000 00000000
  
    tripsHandValue + (threeMask << pairOrHigherShift) + Evaluator.bottomFiveRanksTable[ranks]
    Ad As Ac Kd Qh => 0xC000000 + 0x2000 + 0x1801    = 0xC003801
    2d 2s 2s 3d 4h => 0xC000000 + 0x4000 + 0x7       = 0xC004007
 */


/*
How two mask works:

 (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade) ^ ranks
  
 One pair example:
  (0001 ^ 0001 ^ 0010 ^ 1000) ^ 1011
  (0001 ^ 0001 = 0000 ^ 0010 = 0010 ^ 1000 = 1010)
  (1010) ^ 1011 = 0001
  
 Three pair example:
  (1111 ^ 1000 ^ 0100 ^ 0010) ^ 1111 = (1111 ^ 1000 = 0111 ^ 0100 = 0011 ^ 0010 = 0001) ^ 1111 = 1110
  
 Trips example (don't work in two mask):
  (0001 ^ 0001 ^ 0001 ^ 0000) ^ 0001 = (0001 ^ 0001 = 0000 ^ 0001 = 0001 ^ 0000 = 0001) ^ 0001 = 0000
  
 Quads example:
  (1001 ^ 0101 ^ 0001 ^ 0001) ^ 1101 = (1001 ^ 0101 = 1100 ^ 0001 = 1101 ^ 0001 = 1100) ^ 1101 = 0001
  
 Fullhouse example (will detect two pair):
  (1001 ^ 1001 ^ 1000 ^ 0000) ^ 1101 = (1001 ^ 1001 = 0000 ^ 1000 = 1000 ^ 0000 = 1000) ^ 1101 = 0101
 */


/* 
 * How aces shift to bottom works:
 * 
* The original cardmask puts aces on the top position. For lowball variants aces counts as low (22 vs AA= 22 is the higher hand).
* This expression extracts all aces from given hand, shifts every card up by 1 and shifts the aces down by 12. 
* 
* ulong acesOnly = cards & allAcesMask;
* ulong acesRemoved = ~acesOnly & cards;           // swap bits in the extracted aces and remove them from give cards
* acesRemoved = acesRemoved << 1;                  // shift all cards one up so that K is on position 13 and 2 is on position 2
* acesOnly = acesOnly >> 12;                       // shift all given aces 12 positions down so that they are on position 1
* ulong acesBottomCards = acesOnly | acesRemoved;  // add the shifted aces back to hand
* 
*/

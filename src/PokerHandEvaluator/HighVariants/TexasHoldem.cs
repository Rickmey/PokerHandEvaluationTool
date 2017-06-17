namespace PokerHandEvaluator.HighVariants
{
    public static class TexasHoldem
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

        public static uint GetHandValue(ulong cardMask, uint numberOfCards)
        {
            uint suitClub = (uint)(cardMask & fullRankSet);
            uint suitDiamond = (uint)((cardMask >> diamondOffset) & fullRankSet);
            uint suitHeart = (uint)((cardMask >> heartOffset) & fullRankSet);
            uint suitSpade = (uint)((cardMask >> spadeOffset) & fullRankSet);

            uint ranks = suitClub | suitDiamond | suitHeart | suitSpade;
            uint numberOfRanks = Utils.nBitsTable[ranks];
            uint numberOfDuplicates = numberOfCards - numberOfRanks;

            // Check for straight, flush, or straight flush
            if (numberOfRanks >= 5)
            {
                if (Utils.nBitsTable[suitClub] >= 5)
                {
                    if (LookupTables.straightTable[suitClub] != 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitClub];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitClub];
                }
                else if (Utils.nBitsTable[suitDiamond] >= 5)
                {
                    if (LookupTables.straightTable[suitDiamond] != 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitDiamond];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitDiamond];
                }
                else if (Utils.nBitsTable[suitHeart] >= 5)
                {
                    if (LookupTables.straightTable[suitHeart] != 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitHeart];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitHeart];
                }
                else if (Utils.nBitsTable[suitSpade] >= 5)
                {
                    if (LookupTables.straightTable[suitSpade] != 0)
                        return straightFlushHandValue + LookupTables.straightTable[suitSpade];
                    return flushHandValue + LookupTables.top5OrLessBitTable[suitSpade];
                }
                else
                {
                    uint straight = LookupTables.straightTable[ranks];
                    if (straight != 0)
                        return straightHandValue + straight;
                };
            }


            uint threeMask;
            uint twoMask;
            switch (numberOfDuplicates)
            {
                case 0:
                    // highcard
                    return LookupTables.top5OrLessBitTable[ranks];
                case 1:
                    {
                        // one pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        // ranks & ~twoMask = remove pair from ranks and get highest 3 cards
                        return onePairHandValue + (twoMask << pairOrHigherShift) + LookupTables.top3OrLessBitTable[ranks & ~twoMask];
                    }

                case 2:

                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    if (twoMask != 0)
                    {
                        // two pair
                        twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                        return twoPairHandValue + (twoMask << pairOrHigherShift) + LookupTables.top1OrLessBitTable[ranks & ~twoMask];
                    }
                    else
                    {
                        // trips
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        return tripsHandValue + (threeMask << pairOrHigherShift) + LookupTables.top2OrLessBitTable[ranks & ~threeMask];
                    }

                default:

                    uint fourMask = suitHeart & suitDiamond & suitClub & suitSpade;
                    if (fourMask != 0)
                    {
                        // quads
                        return quadsHandValue + (fourMask << pairOrHigherShift) + LookupTables.top1OrLessBitTable[ranks & ~fourMask];
                    };

                    twoMask = ranks ^ (suitClub ^ suitDiamond ^ suitHeart ^ suitSpade);
                    if (Utils.nBitsTable[twoMask] != numberOfDuplicates)
                    {
                        // fullhouse
                        threeMask = ((suitClub & suitDiamond) | (suitHeart & suitSpade)) & ((suitClub & suitHeart) | (suitDiamond & suitSpade));
                        // AAA222 -> topThreeMask is A; (threeMask & ~topThreeMask) = 2; twoMask is 0
                        // AAA22K -> topThreeMask is A; (threeMask & ~topThreeMask) = 0; twoMask is 2
                        uint topThreeMask = LookupTables.top1OrLessBitTable[threeMask];
                        return fullHouseHandValue + (topThreeMask << pairOrHigherShift) + (threeMask & ~topThreeMask) + twoMask;
                    };
                    // two pair
                    return twoPairHandValue + (twoMask << pairOrHigherShift) + LookupTables.top1OrLessBitTable[ranks & ~twoMask];
            }
        }

        internal static uint[] generateHighestNBitTable(int numberOfBits)
        {
            // highest
            // 8191 = 0x00001FFF = 00000000 00000000 00011111 11111111 
            // the used rank represent card ranks so it can only use the first 13 bit

            // mask for every bit position to check if the bit is set
            uint[] bitsmask = { 4096, 2048, 1024, 512, 256, 128, 64, 32, 16, 8, 4, 2, 1 };

            uint[] result = new uint[8192];

            for (int i = 0; i < 8192; i++)
            {
                var bitcount = 0;
                uint tempResult = 0;

                foreach (uint item in bitsmask)
                {
                    if ((i & item) > 0) // checks if bit is set
                    {
                        tempResult |= item;
                        bitcount++;
                        if (bitcount == numberOfBits)
                            break;
                    }
                }
                result[i] = tempResult;
            }
            return result;
        }

        public static void WriteHighestNBitTableToFile(int numberOfBits, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = System.AppDomain.CurrentDomain.BaseDirectory + "output.txt";

            var table = generateHighestNBitTable(numberOfBits);
            var stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append("        #region Top " + numberOfBits + " or Less Bit Table \r\n\t\tinternal static readonly uint[] top" + numberOfBits + "OrLessBitTable ={\r\n");
            for (int i = 0; i < table.Length; i++)
            {
                stringBuilder.AppendFormat("\t\t\t0x{0},\r\n", table[i].ToString("X"));
            }
            stringBuilder.Append("\t\t};\r\n#endregion");
            System.IO.File.WriteAllText(filePath, stringBuilder.ToString());
        }
    }
}

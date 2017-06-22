using System.Diagnostics;

namespace PokerHandEvaluator.Stud
{
    internal static class Simulator
    {
        public static HandOdds[] RandomizedNPlayer(ulong dead, uint trials, params ulong[] hands)
        {
            var maxCards = 0;
            ulong usedCards = dead;
            for (int i = 0; i < hands.Length; i++)
            {
                usedCards |= hands[i];
                if (Utils.BitCount(hands[i]) > maxCards)
                    maxCards = Utils.BitCount(hands[i]);
            }

            int nNeededCards = 7 - maxCards;
            uint count = 0;

            uint[] winTable = new uint[hands.Length];
            uint[] tieTable = new uint[hands.Length];
            uint[] values = new uint[hands.Length];
            for (int i = 0; i < values.Length; i++)
            {
                winTable[i] = 0;
                tieTable[i] = 0;
                values[i] = 0;
            }
            uint maxValue = 0;
            bool tie = false;
            ulong randomCards;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (count < trials)
            {
                maxValue = 0;
                tie = false;
                ulong currentUsedCards = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    randomCards = Utils.GetRandomCards(usedCards | currentUsedCards, nNeededCards);
                    currentUsedCards |= randomCards;
                    var value = TexasHoldem.Evaluator.GetHandValue(randomCards | hands[i], 7);
                    values[i] = value;
                    if (value > maxValue)
                        maxValue = value;
                    else if (maxValue != 0 && value == maxValue)
                        tie = true;
                }
                for (int i = 0; i < values.Length; i++)
                {
                    if (tie && values[i] == maxValue)
                        tieTable[i]++;
                    else if (values[i] == maxValue)
                    {
                        winTable[i]++;
                        break;
                    }
                }
                count++;
            }

            sw.Stop();

            HandOdds[] result = new HandOdds[hands.Length];
            for (int i = 0; i < hands.Length; i++)
            {
                result[i] = new HandOdds(hands[i], trials, sw.ElapsedMilliseconds, false, winTable[i], tieTable[i]);
            }
            return result;
        }
    }
}

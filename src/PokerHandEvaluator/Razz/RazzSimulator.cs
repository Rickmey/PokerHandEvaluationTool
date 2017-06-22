using System.Diagnostics;

namespace PokerHandEvaluator.Razz
{
    internal static class Simulator
    {
        public static HandOdds[] RandomizedNPlayer(ulong dead, uint trials, params ulong[] hands)
        {
            ulong usedCards = Utils.ShiftAceToBottom(dead);
            for (int i = 0; i < hands.Length; i++)
            {
                hands[i] = Utils.ShiftAceToBottom(hands[i]);

                usedCards |= hands[i];
            }

            int nNeededCards = 7 - Utils.BitCount(hands[0]); // assumes that every hand has the same amount of cards
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
            uint minValue = uint.MaxValue;
            bool tie = false;
            ulong randomCards;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (count < trials)
            {
                minValue = uint.MaxValue;
                tie = false;
                ulong currentUsedCards = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    randomCards = Utils.GetRandomCards(usedCards | currentUsedCards, nNeededCards);
                    currentUsedCards |= randomCards;
                    var value = Evaluator.GetHandValue(randomCards | hands[i], 7);
                    values[i] = value;
                    if (value < minValue)
                        minValue = value;
                    else if (minValue != 0 && value == minValue)
                        tie = true;
                }
                for (int i = 0; i < values.Length; i++)
                {
                    if (tie && values[i] == minValue)
                        tieTable[i]++;
                    else if (values[i] == minValue)
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
                result[i] = new HandOdds(Utils.ShiftAceToTop(hands[i]), trials, sw.ElapsedMilliseconds, false, winTable[i], tieTable[i]);
            }
            return result;
        }
    }
}

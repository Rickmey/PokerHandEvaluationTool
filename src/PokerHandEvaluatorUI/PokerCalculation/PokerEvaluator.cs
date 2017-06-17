using System.Collections.Generic;
using System.Diagnostics;

namespace PokerCalculations
{
    public enum GameTypes
    {
        TexasHoldem,
        SevenCardStud,
        Razz
    }

    class PokerEvaluator
    {
        public static IReadOnlyList<HandOdds> Evaluate(GameTypes gameType, ulong board, ulong dead, int runs = 10000000, params ulong[] hands)
        {
            var count = hands.Length;
            if (gameType == GameTypes.TexasHoldem && count > 10)
                count = 10;
            else if (gameType == GameTypes.SevenCardStud && count > 6)
                count = 6;

            var result = new HandOdds[count];
            ulong usedCards = dead;
            for (int i = 0; i < count; i++)
            {
                usedCards |= hands[i];
                result[i] = new HandOdds(hands[i]);
            }
            var values = new uint[count];

            uint maxValue = uint.MinValue;
            bool tie = false;

            int nNeededCards = 0;
            if (gameType == GameTypes.TexasHoldem)
                nNeededCards = 5 - PokerHandEvaluator.Utils.BitCount(board);
            else if (gameType == GameTypes.SevenCardStud || gameType == GameTypes.Razz)
            {
                // find the first hand that has cards; assuming all hands have the same number of cards
                foreach (var hand in hands)
                {
                    if (hand == 0)
                        continue;
                    nNeededCards = 7 - PokerHandEvaluator.Utils.BitCount(hand);
                    break;
                }
            }

            ulong randomCards = 0;
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < runs; ++i)
            {
                maxValue = uint.MinValue;
                tie = false;

                switch (gameType)
                {
                    case GameTypes.TexasHoldem:
                        randomCards = PokerHandEvaluator.Utils.GetRandomCards(usedCards, nNeededCards);
                        for (int j = 0; j < count; j++)
                        {
                            if (hands[j] == 0)
                                continue;

                            var value = PokerHandEvaluator.HighVariants.TexasHoldem.GetHandValue(randomCards | board | hands[j], 7);
                            values[j] = value;
                            if (value > maxValue)
                                maxValue = value;
                        }
                        break;

                    case GameTypes.SevenCardStud:
                        var currentUsedCards = 0UL;
                        for (int j = 0; j < count; j++)
                        {
                            if (hands[j] == 0)
                                continue;
                            randomCards = PokerHandEvaluator.Utils.GetRandomCards(usedCards | currentUsedCards, nNeededCards);
                            currentUsedCards |= randomCards;
                            var value = PokerHandEvaluator.HighVariants.TexasHoldem.GetHandValue(randomCards | hands[j], 7);
                            values[j] = value;
                            if (value > maxValue)
                                maxValue = value;
                        }
                        break;
                }

                var duplicate = false;
                if (gameType != GameTypes.Razz)
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (values[j] < maxValue)
                            continue;
                        if (duplicate && values[j] == maxValue)
                        {
                            tie = true;
                            break;
                        }

                        if (values[j] == maxValue)
                        {
                            duplicate = true;
                            continue;
                        }
                    }

                    for (int j = 0; j < count; j++)
                    {
                        if (hands[j] == 0)
                            continue;

                        if (values[j] < maxValue)
                        {
                            result[j].Losses++;
                        }
                        else if (!tie)
                        {
                            result[j].Wins++;
                        }
                        else
                        {
                            result[j].Ties++;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < count; j++)
                    {
                        if (values[j] < maxValue)
                            continue;
                        if (duplicate && values[j] == maxValue)
                        {
                            tie = true;
                            break;
                        }

                        if (values[j] == maxValue)
                        {
                            duplicate = true;
                            continue;
                        }
                    }

                    for (int j = 0; j < count; j++)
                    {
                        if (hands[j] == 0)
                            continue;

                        if (values[j] > maxValue)
                        {
                            result[j].Losses++;
                        }
                        else if (!tie)
                        {
                            result[j].Wins++;
                        }
                        else
                        {
                            result[j].Ties++;
                        }
                    }
                }
            }

            sw.Stop();
            for (int i = 0; i < count; i++)
            {
                result[i].Emulations = runs;
                result[i].TimeInMs = sw.ElapsedMilliseconds;
            }
            return result;
        }
    }
}

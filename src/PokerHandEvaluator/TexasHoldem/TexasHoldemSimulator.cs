using System.Diagnostics;

namespace PokerHandEvaluator.TexasHoldem
{
    internal static class Simulator
    {
        public static HandOdds[] ExhaustiveNPlayer(ulong board, params ulong[] handsOriginal)
        {
            int nNeededCards = 5 - Utils.BitCount(board); // 5 = 7 - 2 because we need 7 cards in total and each hand has 2 cards
            ulong usedCards = board;
            var hands = new System.Collections.Generic.List<ulong>(handsOriginal).ToArray();
            for (int i = 0; i < handsOriginal.Length; i++)
            {
                usedCards |= hands[i];
                hands[i] |= board;
            }

            uint count = 0;
            ulong seventh, sixth, fifth, fourth, third;

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

            Stopwatch sw = new Stopwatch();
            sw.Start();
            switch (nNeededCards)
            {
                case 0:
                    for (int i = 0; i < values.Length; i++)
                    {
                        var value = Evaluator.GetHandValue(hands[i], 7);
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
                    break;

                case 1:
                    for (int seventhStreet = 0; seventhStreet < 52; seventhStreet++)
                    {
                        if ((usedCards & Utils.CardMasksTable[seventhStreet]) > 0)
                            continue;
                        seventh = Utils.CardMasksTable[seventhStreet];
                        maxValue = 0;
                        tie = false;
                        for (int i = 0; i < values.Length; i++)
                        {
                            var value = Evaluator.GetHandValue(hands[i] | seventh, 7);
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
                    break;

                case 2:
                    for (int seventhStreet = 0; seventhStreet < 52; seventhStreet++)
                    {
                        if ((usedCards & Utils.CardMasksTable[seventhStreet]) > 0)
                            continue;
                        seventh = Utils.CardMasksTable[seventhStreet];
                        for (int sixthStreet = seventhStreet + 1; sixthStreet < 52; sixthStreet++)
                        {
                            sixth = Utils.CardMasksTable[sixthStreet];
                            if (((usedCards | seventh) & Utils.CardMasksTable[sixthStreet]) > 0)
                            {
                                continue;
                            }
                            maxValue = 0;
                            tie = false;
                            for (int i = 0; i < values.Length; i++)
                            {
                                var value = Evaluator.GetHandValue(hands[i] | seventh | sixth, 7);
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
                    }
                    break;

                case 3:

                    for (int seventhStreet = 0; seventhStreet < 52; seventhStreet++)
                    {
                        seventh = Utils.CardMasksTable[seventhStreet];
                        if ((usedCards & seventh) > 0)
                            continue;

                        for (int sixthStreet = seventhStreet + 1; sixthStreet < 52; sixthStreet++)
                        {
                            sixth = Utils.CardMasksTable[sixthStreet];
                            if (((usedCards | seventh) & sixth) > 0)
                                continue;

                            for (int fifthStreet = sixthStreet + 1; fifthStreet < 52; fifthStreet++)
                            {
                                fifth = Utils.CardMasksTable[fifthStreet];
                                if (((usedCards | seventh | sixth) & fifth) > 0)
                                    continue;

                                maxValue = 0;
                                tie = false;
                                for (int i = 0; i < values.Length; i++)
                                {
                                    var value = Evaluator.GetHandValue(hands[i] | seventh | sixth | fifth, 7);
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
                        }
                    }
                    break;

                case 4:

                    for (int seventhStreet = 0; seventhStreet < 52; seventhStreet++)
                    {
                        seventh = Utils.CardMasksTable[seventhStreet];
                        if ((usedCards & seventh) > 0)
                            continue;

                        for (int sixthStreet = seventhStreet + 1; sixthStreet < 52; sixthStreet++)
                        {
                            sixth = Utils.CardMasksTable[sixthStreet];
                            if (((usedCards | seventh) & sixth) > 0)
                                continue;

                            for (int fifthStreet = sixthStreet + 1; fifthStreet < 52; fifthStreet++)
                            {
                                fifth = Utils.CardMasksTable[fifthStreet];
                                if (((usedCards | seventh | sixth) & fifth) > 0)
                                    continue;
                                for (int fourthStreet = fifthStreet + 1; fourthStreet < 52; fourthStreet++)
                                {
                                    fourth = Utils.CardMasksTable[fourthStreet];
                                    if (((usedCards | seventh | sixth | fifth) & fourth) > 0)
                                        continue;
                                    maxValue = 0;
                                    tie = false;
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        var value = Evaluator.GetHandValue(hands[i] | seventh | sixth | fourth, 7);
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
                            }
                        }
                    }
                    break;

                case 5:
                    for (int seventhStreet = 0; seventhStreet < 52; ++seventhStreet)
                    {
                        seventh = Utils.CardMasksTable[seventhStreet];
                        if ((usedCards & seventh) > 0)
                            continue;
                        for (int sixthStreet = seventhStreet + 1; sixthStreet < 52; ++sixthStreet)
                        {
                            sixth = Utils.CardMasksTable[sixthStreet];
                            if ((usedCards & sixth) > 0)
                                continue;
                            for (int fifthStreet = sixthStreet + 1; fifthStreet < 52; ++fifthStreet)
                            {
                                fifth = Utils.CardMasksTable[fifthStreet];
                                if ((usedCards & fifth) > 0)
                                    continue;
                                for (int fourthStreet = fifthStreet + 1; fourthStreet < 52; ++fourthStreet)
                                {
                                    fourth = Utils.CardMasksTable[fourthStreet];
                                    if ((usedCards & fourth) > 0)
                                        continue;
                                    for (int thirdStreet = fourthStreet + 1; thirdStreet < 52; ++thirdStreet)
                                    {
                                        third = Utils.CardMasksTable[thirdStreet];
                                        if ((usedCards & third) > 0)
                                            continue;

                                        maxValue = 0;
                                        tie = false;
                                        for (int i = 0; i < values.Length; i++)
                                        {
                                            var value = Evaluator.GetHandValue(hands[i] | seventh | sixth | fourth | third, 7);
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
                                }
                            }
                        }
                    }
                    break;
            }

            sw.Stop();

            HandOdds[] result = new HandOdds[hands.Length];
            for (int i = 0; i < hands.Length; i++)
            {
                result[i] = new HandOdds(handsOriginal[i], count, sw.ElapsedMilliseconds, true, winTable[i], tieTable[i]);
            }
            return result;
        }
    }
}

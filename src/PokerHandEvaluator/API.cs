using System;
using System.Collections.Generic;

namespace PokerHandEvaluator
{
    public enum GameTypes
    {
        TexasHoldem,
        SevenCardStud,
        Razz
    }

    public static class PokerHandEvaluatorAPI
    {
        /// <summary>
        /// 600000 gives decent results for randomized
        /// </summary>
        const int defaultTrials = 600000;

        public static IReadOnlyList<HandOdds> Evaluate(GameTypes gameType, ulong board, ulong dead, params ulong[] hands)
        {
            // removed empty hands and hands that have less cards then other hands
            List<ulong> cleanHands = new List<ulong>();
            var maxCards = 0;
            for (int i = 0; i < hands.Length; i++)
            {
                if (Utils.BitCount(hands[i]) > maxCards)
                    maxCards = Utils.BitCount(hands[i]);
            }
            for (int i = 0; i < hands.Length; i++)
            {
                var hand = hands[i];
                if (hand > 0 && Utils.BitCount(hand) == maxCards)
                    cleanHands.Add(hand);
            }

            IReadOnlyList<HandOdds> tempResult;
            switch (gameType)
            {
                case GameTypes.TexasHoldem:
                    tempResult = TexasHoldem.Simulator.ExhaustiveNPlayer(board, cleanHands.ToArray());
                    break;
                case GameTypes.SevenCardStud:
                    tempResult = Stud.Simulator.RandomizedNPlayer(dead, defaultTrials, cleanHands.ToArray());
                    break;
                case GameTypes.Razz:
                    tempResult = Razz.Simulator.RandomizedNPlayer(dead, defaultTrials, cleanHands.ToArray());
                    break;
                default:
                    throw new ArgumentException("Unknown game type");
            }

            // result as handodds array must be the same length as the given hand array
            var result = new HandOdds[hands.Length];
            foreach (var odds in tempResult)
            {
                // find the hand for the result
                var index = Array.IndexOf(hands, odds.CardMask);
                result[index] = odds;
            }
            return result;
        }

        public static string ToFriendlyString(this GameTypes gameTypes)
        {
            switch (gameTypes)
            {
                case GameTypes.TexasHoldem: return "Texas Holdem";
                case GameTypes.SevenCardStud: return "Seven Card Stud";
                case GameTypes.Razz: return "Razz";
                default: throw new ArgumentException("Gametype not found");
            }
        }
    }
}

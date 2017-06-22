using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    public static class Extensions
    {
        public static IEnumerable<T> FindLogicalChildren<T>(this Control control, DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject)
                    {
                        DependencyObject child = (DependencyObject)rawChild;
                        if (child is T)
                        {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in control.FindLogicalChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        public static string ToFriendlyString(this CardRanks rank)
        {
            switch (rank)
            {
                case CardRanks.None: return "";
                case CardRanks.Two: return "2";
                case CardRanks.Three: return "3";
                case CardRanks.Four: return "4";
                case CardRanks.Five: return "5";
                case CardRanks.Six: return "6";
                case CardRanks.Seven: return "7";
                case CardRanks.Eight: return "8";
                case CardRanks.Nine: return "9";
                case CardRanks.Ten: return "T";
                case CardRanks.Jack: return "J";
                case CardRanks.Queen: return "Q";
                case CardRanks.King: return "K";
                case CardRanks.Ace: return "A";
                default: throw new System.ArgumentException("Rank not found");
            }
        }

        public static string ToFriendlyShortString(this CardSuits suit)
        {
            switch (suit)
            {
                case CardSuits.None: return "";
                case CardSuits.Clubs: return "c";
                case CardSuits.Diamonds: return "d";
                case CardSuits.Hearts: return "h";
                case CardSuits.Spades: return "s";
                default: throw new System.ArgumentException("Suit not found");
            }
        }

        public static string ToFriendlyLongString(this CardSuits suit)
        {
            switch (suit)
            {
                case CardSuits.None: return "";
                case CardSuits.Clubs: return "club";
                case CardSuits.Diamonds: return "diamond";
                case CardSuits.Hearts: return "heart";
                case CardSuits.Spades: return "spade";
                default: throw new System.ArgumentException("Suit not found");
            }
        }

        public static string ToFriendlyString(this CardLayouts layout)
        {
            switch (layout)
            {
                case CardLayouts.CenterRank: return "CenterRank";
                case CardLayouts.CenterSuit: return "CenterSuit";
                default: throw new System.ArgumentException("Layout not found");
            }
        }
    }
}

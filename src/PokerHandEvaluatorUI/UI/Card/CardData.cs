namespace PokerEvaluationToolUI
{
    public interface ICardData
    {
        ulong Mask { get; }
        CardRanks Rank { get; }
        CardSuits Suit { get; }

        string ToString();
    }

    public class CardData : ICardData
    {
        public CardRanks Rank { get; private set; }
        public CardSuits Suit { get; private set; }

        public ulong Mask
        {
            get { return PokerHandEvaluator.Utils.StringToHandMask(ToString()); }
        }

        public CardData(CardRanks rank, CardSuits suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString()
        {
            return Rank.ToFriendlyString() + Suit.ToFriendlyShortString();
        }
    }
}

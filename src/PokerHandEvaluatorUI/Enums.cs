namespace PokerEvaluationToolUI
{
    public enum CardRanks
    {
        None = 0,
        Two = 1,
        Three = 2,
        Four = 3,
        Five = 4,
        Six = 5,
        Seven = 6,
        Eight = 7,
        Nine = 8,
        Ten = 9,
        Jack = 10,
        Queen = 11,
        King = 12,
        Ace = 13
    }

    public enum CardSuits
    {
        None = 0,
        Clubs = 1,
        Diamonds = 2,
        Hearts = 3,
        Spades = 4
    }

    public enum CardLayouts
    {
        CenterSuit = 0,
        CenterRank = 1,
    }

    public enum AppConfigProperties
    {
        GameType,
        CardLayout
    }
}

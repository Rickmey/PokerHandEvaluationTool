namespace PokerEvaluationToolUI
{
    public interface ICardViewModel : IViewModelBase
    {
        bool CanBeUsed { get; set; }
        ICardData CardData { get; set; }
        CardLayouts CardLayout { get; set; }
        string CardLayoutKey { get; }
        bool Highlighted { get; set; }
        CardRanks Rank { get; }
        CardSuits Suit { get; }
        ulong CardAsNumber { get; }
        bool IsEmpty { get; }
        string ToString();
    }

    public class CardViewModel : ViewModelBase, ICardViewModel
    {
        ICardData _data;
        public ICardData CardData
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged(""); }
        }

        CardLayouts _cardLayout;
        public CardLayouts CardLayout
        {
            get { return _cardLayout; }
            set { if (Frozen) return; _cardLayout = value; OnPropertyChanged("CardLayoutKey"); }
        }

        public string CardLayoutKey
        {
            get { return CardLayout.ToFriendlyString(); }
        }

        public CardRanks Rank
        {
            get { if (CardData == null) return CardRanks.None; return CardData.Rank; }
        }

        public CardSuits Suit
        {
            get { if (CardData == null) return CardSuits.None; return CardData.Suit; }
        }

        public ulong CardAsNumber
        {
            get => CardData == null ? 0 : CardData.Mask;
        }

        public bool IsEmpty
        {
            get => Rank == CardRanks.None && Suit == CardSuits.None;
        }


        bool _canBeUsed;
        public bool CanBeUsed
        {
            get => _canBeUsed;
            set => SetValue(ref _canBeUsed, value);
        }

        bool _highlighted;
        public bool Highlighted
        {
            get => _highlighted;
            set { SetValue<bool>(ref _highlighted, value); }
        }


        public CardViewModel(ICardData iCardData)
        {
            _canBeUsed = true;
            _data = iCardData;
        }


        public override string ToString()
        {
            var rank = Rank.ToFriendlyString();
            var suit = Suit.ToFriendlyShortString();
            return rank + suit;
        }
    }
}

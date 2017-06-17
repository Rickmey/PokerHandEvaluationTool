using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PokerEvaluationToolUI
{
    public class HandModel : ViewModelBase
    {
        OddsModel _odds = new OddsModel();
        public OddsModel Odds
        {
            get => _odds;
            set => SetValue(ref _odds, value);
        }

        public ObservableCollection<CardView> CardsViews { get; private set; }
        public ICardViewModel[] CardModels { get; private set; }

        public int UsedCardsCount
        {
            get
            {
                int result = 0;
                foreach (var item in CardModels)
                {
                    if (item.CardAsNumber > 0)
                        result++;
                }
                return result;
            }
        }

        public HandModel(IList<CardView> cardViews)
        {
            CardModels = new CardViewModel[cardViews.Count];
            for (int i = 0; i < cardViews.Count; i++)
                CardModels[i] = cardViews[i].Model;
            CardsViews = new ObservableCollection<CardView>(cardViews);
        }
    }
}

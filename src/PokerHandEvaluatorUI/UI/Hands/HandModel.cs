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

        public IEnumerable<ICardViewModel> CardModels { get; private set; }

        public int UsedCardsCount
        {
            get
            {
                int result = 0;
                foreach (var item in CardModels)
                {
                    if (item.CardMask > 0)
                        result++;
                }
                return result;
            }
        }

        public HandModel(IEnumerable<ICardViewModel> cardModels)
        {
            CardModels = cardModels;

        }
    }
}

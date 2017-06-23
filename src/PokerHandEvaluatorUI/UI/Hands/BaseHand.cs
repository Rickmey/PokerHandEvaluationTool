using System.Collections.Generic;
using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    public class BaseHand : UserControl
    {
        HandModel model;
        public HandModel Model
        {
            get
            {
                // lazy load cardmodels in hand
                if (model == null)
                {
                    List<ICardViewModel> cardModels = new List<ICardViewModel>();
                    foreach (var item in this.FindLogicalChildren<CardView>(this))
                    {
                        cardModels.Add(item.Model);
                    }
                    model = new HandModel(cardModels);
                }
                return model;
            }
            private set { model = value; }
        }
    }
}

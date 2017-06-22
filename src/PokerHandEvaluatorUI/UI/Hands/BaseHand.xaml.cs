using PokerHandEvaluator;
using System.ComponentModel;
using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    /// <summary>
    /// Interaction logic for BaseHand.xaml
    /// </summary>
    public partial class BaseHand : UserControl
    {
        public HandModel Model { get; set; }

        public BaseHand()
        {
            var nCards = 2;
            GameTypes gametype;
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                gametype = GameTypes.TexasHoldem;
            }
            else
            {
                gametype = (GameTypes)System.Windows.Application.Current.Properties[AppConfigProperties.GameType];
            }

            switch (gametype)
            {
                case GameTypes.TexasHoldem: nCards = 2; break;
                case GameTypes.SevenCardStud: nCards = 6; break;
                case GameTypes.Razz: nCards = 6; break;
                default: throw new System.ArgumentException("Gametype not found");
            }

            CardView[] cardViews = new CardView[nCards];
            for (int i = 0; i < nCards; i++)
                cardViews[i] = new CardView();

            Model = new HandModel(cardViews);

            InitializeComponent();
            DataContext = Model;
        }
    }
}

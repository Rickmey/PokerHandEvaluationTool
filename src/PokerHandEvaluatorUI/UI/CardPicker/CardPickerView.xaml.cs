using System.Windows;
using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    /// <summary>
    /// Interaction logic for CardPickerView.xaml
    /// </summary>
    public partial class CardPickerView : UserControl
    {
        ICardViewModel[,] models = new CardViewModel[13, 4];

        public CardPickerView()
        {
            InitializeComponent();

            foreach (CardView cardView in _MainContainer.Children)
            {
                var row = Grid.GetRow(cardView);
                var column = Grid.GetColumn(cardView);
                cardView.Model.CardData = new CardData((CardRanks)row + 1, (CardSuits)column + 1);
                models[row, column] = cardView.Model;
            }
            _MainContainer.MouseLeftButtonDown += Main_MouseLeftButtonDown;

            App.EventAggregator?.GetEvent<GameState_CardCleared>()?.Subscribe((gameStateCardModel) =>
            {
                models[(int)gameStateCardModel.CardData.Rank - 1, (int)gameStateCardModel.CardData.Suit - 1].CanBeUsed = true; // free card
                gameStateCardModel.CardData = null; // empty model from gamestate
            });
        }

        public void Reset()
        {
            for (int row = 0; row < 13; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    models[row, column].CanBeUsed = true;
                }
            }
        }

        private void Main_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            var model = (e.OriginalSource as FrameworkElement)?.DataContext as CardViewModel;
            if (model == null || !models[(int)model.CardData.Rank - 1, (int)model.CardData.Suit - 1].CanBeUsed)
                return;
            App.EventAggregator.GetEvent<CardPicker_CardSelected>().Publish(model.CardData);
            model.CanBeUsed = false;
        }
    }
}

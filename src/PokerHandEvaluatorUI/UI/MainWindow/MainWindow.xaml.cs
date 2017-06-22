using PokerHandEvaluator;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel model;

        public MainWindow()
        {
            model = new MainWindowViewModel();
            InitializeComponent();
            DataContext = model;

            // setup preview for card layout change
            var cardPreviewLayouts = this.FindLogicalChildren<CardView>(CardLayoutPreview).ToArray();
            for (int i = 0; i < cardPreviewLayouts.Length; i++)
            {
                // setup destinct design and freeze model
                var cardView = cardPreviewLayouts[i];
                cardView.Model.CardLayout = ((CardLayouts)i);
                cardView.Model.CardData = new CardData(CardRanks.Five, CardSuits.Hearts);
                cardView.Model.Frozen = true;
            }
        }

        /// <summary>
        /// This code is called when a card in the card layout preview is clicked
        /// </summary>
        private void CardLayoutPreview_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var cardView = e.Source as CardView;
            e.Handled = true;
            if (cardView.Model.CardLayout == (CardLayouts)Application.Current.Properties[AppConfigProperties.CardLayout])
                return;
            Application.Current.Properties[AppConfigProperties.CardLayout] = cardView.Model.CardLayout;
            App.EventAggregator.GetEvent<CardLayoutChange>().Publish();
        }

        private async void CalculationBtn_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var result = await Task.Run(() => PokerHandEvaluatorAPI.Evaluate((GameTypes)Application.Current.Properties[AppConfigProperties.GameType], model.GameState.GetBoard(), model.GameState.GetDeadCards(), model.GameState.GetPlayerCards()));
            model.GameState.ApplyHandOdds(result);
        }

        private void GameType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            string text = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            if (string.IsNullOrEmpty(text))
                return;
            GameTypes gameType;
            switch (text)
            {
                case "Texas Holdem": gameType = GameTypes.TexasHoldem; break;
                case "Seven Card Stud": gameType = GameTypes.SevenCardStud; break;
                case "Razz": gameType = GameTypes.Razz; break;
                default: throw new ArgumentException("Gametype not found");
            }
            if (gameType == (GameTypes)Application.Current.Properties[AppConfigProperties.GameType])
                return;
            Application.Current.Properties[AppConfigProperties.GameType] = gameType;

            model.GameState.Dispose();

            switch (gameType)
            {
                case GameTypes.TexasHoldem: model.GameState = new TexasHoldemGameStateView(); break;
                case GameTypes.SevenCardStud: model.GameState = new SevenCardStudGameStateView(); break;
                case GameTypes.Razz: model.GameState = new RazzGameStateView(); break;
            }
            _CardPicker.Reset();
        }
    }
}

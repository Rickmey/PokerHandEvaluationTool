using Prism.Events;
using System.Windows;

namespace PokerEvaluationToolUI
{
    // Prism Events
    public class CardLayoutChange : PubSubEvent { }
    public class CardPicker_CardSelected : PubSubEvent<ICardData> { }
    public class GameState_CardCleared : PubSubEvent<ICardViewModel> { }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IEventAggregator EventAggregator { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            EventAggregator = new EventAggregator();
            Current.Properties.Add(AppConfigProperties.GameType, PokerCalculations.GameTypes.TexasHoldem);
            Current.Properties.Add(AppConfigProperties.CardLayout, CardLayouts.CenterSuit);
            base.OnStartup(e);
        }
    }
}

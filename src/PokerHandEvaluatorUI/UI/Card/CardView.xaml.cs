using System;
using System.Windows;
using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    public interface ICardView
    {
        ICardViewModel Model { get; set; }
        void InitializeComponent();
    }

    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class CardView : UserControl, ICardView, IDisposable
    {
        public ICardViewModel Model { get; set; }

        public CardView()
        {
            Model = new CardViewModel(null);
            DataContext = Model;

            InitializeComponent();

            App.EventAggregator?.GetEvent<CardLayoutChange>()?.Subscribe(card_ChangeCardLayout);
        }

        void card_ChangeCardLayout()
        {
            var layout = Application.Current.Properties[AppConfigProperties.CardLayout];
            Model.CardLayout = (CardLayouts)layout;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                App.EventAggregator.GetEvent<CardLayoutChange>().Unsubscribe(card_ChangeCardLayout);
            }
        }
    }
}

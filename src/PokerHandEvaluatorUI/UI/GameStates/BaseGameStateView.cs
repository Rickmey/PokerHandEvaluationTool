using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    public class BaseGameStateView : UserControl, IDisposable
    {
        int currentSelectedIndex = 0;

        ICardViewModel _currentSelected;
        public ICardViewModel CurrentSelected
        {
            get => _currentSelected;
            set
            {
                if (_currentSelected != null)
                    _currentSelected.Highlighted = false;
                _currentSelected = value;
                if (_currentSelected != null)
                    _currentSelected.Highlighted = true;
            }
        }

        List<HandModel> baseHands = new List<HandModel>();
        List<ICardViewModel> boardModels = new List<ICardViewModel>();
        List<ICardViewModel> deadCardModels = new List<ICardViewModel>();
        List<ICardViewModel> allCards = new List<ICardViewModel>();

        int playerCount;

        public BaseGameStateView()
        {
            App.EventAggregator?.GetEvent<CardPicker_CardSelected>()?.Subscribe(CardPickerClick);

            Loaded += (sender, e) =>
            {
                var board = FindName("_Board");
                foreach (var item in this.FindLogicalChildren<CardView>(board as DependencyObject))
                {
                    boardModels.Add(item.Model);
                }

                var dead = FindName("_Dead");
                foreach (var item in this.FindLogicalChildren<CardView>(dead as DependencyObject))
                {
                    deadCardModels.Add(item.Model);
                }

                var main = FindName("_MainGrid");
                foreach (var hand in this.FindLogicalChildren<BaseHand>(main as DependencyObject))
                {
                    baseHands.Add(hand.Model);
                    foreach (var item in hand.Model.CardModels)
                        allCards.Add(item);
                    playerCount++;
                }

                allCards.AddRange(boardModels);
                allCards.AddRange(deadCardModels);

                if (allCards != null && allCards.Count > 0)
                {
                    CurrentSelected = allCards[0];
                    CurrentSelected.Highlighted = true;
                }
                var grid = main as Grid;
                grid.MouseLeftButtonDown += Main_MouseLeftButtonDown;
            };
        }

        private void Main_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            var clickedModel = (e.OriginalSource as FrameworkElement)?.DataContext as CardViewModel;
            if (clickedModel == null)
                return;
            CurrentSelected = clickedModel;
            currentSelectedIndex = allCards.IndexOf(CurrentSelected);
            if (!clickedModel.IsEmpty)
                App.EventAggregator.GetEvent<GameState_CardCleared>().Publish(clickedModel);
        }

        private void CardPickerClick(ICardData data)
        {
            if (CurrentSelected == null)
                return;

            if (!CurrentSelected.IsEmpty) // autoselected full card;
                App.EventAggregator.GetEvent<GameState_CardCleared>().Publish(CurrentSelected); // clear the old data
            CurrentSelected.CardData = data; // set the new data

            // autoselect next
            var index = allCards.IndexOf(CurrentSelected);
            currentSelectedIndex++;
            if (currentSelectedIndex > allCards.Count - 1)
                currentSelectedIndex = 0;
            CurrentSelected = allCards[currentSelectedIndex];
        }

        public void ApplyHandOdds(IReadOnlyList<PokerCalculations.HandOdds> odds)
        {
            for (int i = 0; i < baseHands.Count; i++)
                baseHands[i].Odds.HandOdds = odds[i];
        }

        public ulong GetBoard()
        {
            var result = 0UL;
            foreach (var item in boardModels)
                result |= item.CardAsNumber;
            return result;
        }

        public ulong GetDeadCards()
        {
            var result = 0UL;
            foreach (var item in deadCardModels)
                result |= item.CardAsNumber;
            return result;
        }

        public ulong[] GetPlayerCards()
        {
            //get the hand with the most non empty cards
            var maxHandCards = int.MinValue;
            foreach (var hand in baseHands)
            {
                if (maxHandCards < hand.UsedCardsCount)
                    maxHandCards = hand.UsedCardsCount;
            }

            var result = new ulong[playerCount];
            for (int i = 0; i < baseHands.Count; i++)
            {
                // only use hands that match the number of non empty cards than the hand with the most non empty cards
                if (baseHands[i].UsedCardsCount < maxHandCards)
                    continue;
                var t = 0UL;
                foreach (var card in baseHands[i].CardModels)
                    t |= card.CardAsNumber;
                result[i] = t;
            }
            return result;
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
                App.EventAggregator.GetEvent<CardPicker_CardSelected>().Unsubscribe(CardPickerClick);
            }
        }
    }
}

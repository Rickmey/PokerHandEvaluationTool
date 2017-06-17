namespace PokerEvaluationToolUI
{
    class MainWindowViewModel : ViewModelBase
    {
        BaseGameStateView _gameState;
        public BaseGameStateView GameState
        {
            get => _gameState;
            set => SetValue(ref _gameState, value);
        }

        public MainWindowViewModel()
        {
            _gameState = new TexasHoldemGameStateView();
        }
    }
}

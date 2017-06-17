using PokerCalculations;

namespace PokerEvaluationToolUI
{
    public class OddsModel : ViewModelBase
    {
        HandOdds _handOdds;
        public HandOdds HandOdds
        {
            get { return _handOdds; }
            set { _handOdds = value; OnPropertyChanged(""); }
        }

        public float Odds
        {
            get { return (Wins + Ties * 0.5f) / Emulations; }
        }

        public int Emulations
        {
            get { return HandOdds == null ? 0 : HandOdds.Emulations; }
        }

        public long TimeInMs
        {
            get { return HandOdds == null ? 0 : HandOdds.TimeInMs; }
        }

        public uint Wins
        {
            get { return HandOdds == null ? 0 : HandOdds.Wins; }
        }

        public uint Ties
        {
            get { return HandOdds == null ? 0 : HandOdds.Ties; }
        }

        public uint Losses
        {
            get { return HandOdds == null ? 0 : HandOdds.Losses; }
        }

        public float WinPercent
        {
            get { return HandOdds == null ? 0 : HandOdds.WinPercent; }
        }

        public float TiePercent
        {
            get { return HandOdds == null ? 0 : HandOdds.TiePercent; }
        }

        public float LossPercent
        {
            get { return HandOdds == null ? 0 : HandOdds.LossPercent; }
        }
    }
}
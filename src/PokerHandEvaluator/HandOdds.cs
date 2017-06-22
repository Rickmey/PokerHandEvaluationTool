namespace PokerHandEvaluator
{
    public class HandOdds
    {
        public uint Trials;
        public long TimeInMs;
        public bool Exhaustive;


        public float Equity { get => (Wins + Ties * 0.5f) / Trials; }
        public uint Wins;
        public uint Ties;
        public uint Losses { get => Trials - Wins - Ties; }

        public ulong CardMask { get; private set; }

        public float WinPercent { get { return Wins * 100f / Trials; } }
        public float TiePercent { get { return Ties * 100f / Trials; } }
        public float LossPercent { get { return Losses * 100f / Trials; } }

        public HandOdds(ulong cardMask, uint trials, long time, bool exhaustive, uint wins, uint ties)
        {
            CardMask = cardMask;
            Trials = trials;
            TimeInMs = time;
            Exhaustive = exhaustive;
            Wins = wins;
            Ties = ties;
        }

        public override string ToString()
        {
            return string.Format("{0}: Equity={1}; Wins={2}; Ties={3}", Utils.CardMaskToString(CardMask), Equity, Wins, Ties);
        }
    }
}

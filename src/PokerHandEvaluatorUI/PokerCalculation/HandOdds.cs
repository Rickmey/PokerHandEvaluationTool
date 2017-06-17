namespace PokerCalculations
{
    public class HandOdds
    {
        public int Emulations;
        public long TimeInMs;

        public uint Wins;
        public uint Ties;
        public uint Losses;

        public string HandAsString { get; private set; }
        public ulong HandAsNumber { get; private set; }

        public float WinPercent { get { return Wins * 100f / Emulations; } }
        public float TiePercent { get { return Ties * 100f / Emulations; } }
        public float LossPercent { get { return Losses * 100f / Emulations; } }

        public HandOdds(ulong handAsNumber)
        {
            HandAsNumber = handAsNumber;
            HandAsString = PokerHandEvaluator.Utils.CardMaskToString(HandAsNumber);
        }
    }
}

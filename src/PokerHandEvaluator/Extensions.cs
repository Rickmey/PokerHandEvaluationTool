namespace PokerHandEvaluator
{
    static class Extensions
    {
        public static string ToFriendlyString(this ulong cardMask, bool acesShifted)
        {
            if (acesShifted)
                return Utils.CardMaskToStringSorted(Utils.ShiftAceToTop(cardMask));
            return Utils.CardMaskToStringSorted(cardMask);
        }

        public static ulong ToCardMask(this string cards)
        {
            return Utils.StringToHandMask(cards);
        }
    }
}

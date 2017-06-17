using System;
using System.Globalization;
using System.Windows.Data;

namespace PokerEvaluationToolUI
{
    class FloatToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var f = (float)value;
            if (float.IsNaN(f))
                f = 0;
            return (f * 100).ToString("00.00") + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class ValueTimesXConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var d = (double)value;
            double param;
            if (!double.TryParse(parameter as string, out param))
                return d;
            return d * param;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class HighlightedToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool highlighted = (bool)value;
            if (highlighted)
                return "Yellow";
            return "White";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class UsedToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool canBeUsed = (bool)value;
            if (canBeUsed)
                return 0d;
            return 0.2d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class RankToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CardRanks rank = (CardRanks)value;
            var result = rank.ToFriendlyString();
            if (string.IsNullOrEmpty(result))
                result = "empty";
            var path = string.Format("pack://application:,,,/PokerEvalUI;component/Resources/{0}.png", result);
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    class SuitToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CardSuits suit = (CardSuits)value;
            var result = suit.ToFriendlyLongString();
            if (string.IsNullOrEmpty(result))
                result = "empty";
            var path = string.Format("pack://application:,,,/PokerEvalUI;component/Resources/{0}.png", result);
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

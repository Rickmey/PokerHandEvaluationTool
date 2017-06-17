using System.Windows.Controls;

namespace PokerEvaluationToolUI
{
    /// <summary>
    /// Interaction logic for Odds.xaml
    /// </summary>
    public partial class Odds : UserControl
    {
        public OddsModel Model { get; private set; }
        public Odds()
        {
            InitializeComponent();
            Model = new OddsModel();
            DataContext = Model;
        }
    }
}

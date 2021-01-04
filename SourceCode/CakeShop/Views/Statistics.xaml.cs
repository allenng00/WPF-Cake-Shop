using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows.Controls;

namespace CakeShop.Views
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        public Statistics()
        {
            InitializeComponent();

        }



        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var series01 = new SeriesCollection();
            var now = DateTime.Now.Month;

            for (int mon = 1; mon < 12; mon++)
            {
                series01.Add(new ColumnSeries
                {
                    Title = "Tháng " + mon.ToString(),
                    ToolTip = "Tháng " + mon.ToString(),
                    Values = new ChartValues<double> { Double.Parse(mon.ToString()) }
                }); ;
            }

            Chart01.Series = series01;
        }
    }
}

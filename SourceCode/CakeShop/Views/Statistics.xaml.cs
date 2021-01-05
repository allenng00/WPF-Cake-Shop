using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CakeShop.Views
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        public List<string> Labels02 { get; set; }
        public List<string> Tooltips02 { get; set; }
        public int Month { get; set; }
        public Statistics()
        {
            InitializeComponent();

        }



        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Month = 12;
            Labels02 = new List<string>();
            Tooltips02 = new List<string>();

            for (var m = 1; m <= Month; m++)
            {
                Tooltips02.Add("Tháng" + m.ToString());
                Labels02.Add(m.ToString());
            }
            DataContext = this;
            PrePareRevenuaByMonthChart();
        }

        private void PrePareRevenuaByMonthChart()
        {
            var series = new SeriesCollection();
            //var now = DateTime.Now.Month;
            
            var values = new ChartValues<double>();
            var rand = new Random();

            // Chuẩn bị dữ liệu
            for (int mon = 1; mon <= Month; mon++)
            {
                var num = rand.Next(50) - 20;
                values.Add(Double.Parse(num.ToString()));
            }

            series.Add(new ColumnSeries
            {
                Title = "Doanh thu theo tuần(12/2020)",
                DataLabels = true,
                LabelPoint = point => point.Y + "USD",
                //ToolTip = "Tháng " + mon.ToString(),
                Values = values
            }) ;


            ChartRevenuabyMonth.Series = series;
        }
        private void PrePareRevenuaByCatChart()
        {
            var series = new SeriesCollection();
            var now = DateTime.Now.Month;
            var values = new ChartValues<double>();
            var rand = new Random();

            // Chuẩn bị dữ liệu
            for (int mon = 1; mon < 12; mon++)
            {
                var num = rand.Next(50) - 20;
                values.Add(Double.Parse(num.ToString()));
            }

            series.Add(new ColumnSeries
            {
                Title = "Doanh thu loại sản phẩm",
                Values = values
            }); ;

            Chart01a.Series = series;
        }
    }
}

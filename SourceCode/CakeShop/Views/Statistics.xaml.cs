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
        public int Year { get; set; }
        public Statistics()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Hàm xử lí sau khi trang khởi tạo xong
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Labels02 = new List<string>();
            PrePareRevenuaByMonthChart();

            DataContext = this;
        }

        private void PrePareRevenuaByMonthChart()
        {
            var series = new SeriesCollection();
            //var now = DateTime.Now.Month;

            var values = new ChartValues<double>();
            var rand = new Random();

            Month = 12;
            Year = 2020;
            List<long> profitList = new List<long>();
            for (int i = 0; i < 4; i++)
            {
                var total1 = App.appDAO.TotalOrders(i, Month, Year);
                var total2 = App.appDAO.TotalReceives(i, Month, Year);

                profitList.Add(total1 - total2);
                values.Add(Double.Parse(profitList[i].ToString()));
                Labels02.Add(i.ToString());
            }

            series.Add(new ColumnSeries
            {
                Title= $"Doanh thu theo tuần ({Month}/{Year})",
                DataLabels = true,
                LabelPoint = point => point.Y + " USD",
                Values = values
            });


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

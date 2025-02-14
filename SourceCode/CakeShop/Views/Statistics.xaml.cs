﻿using LiveCharts;
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
        public List<string> Labels01 { get; set; }
        public List<string> Labels02 { get; set; }
        public List<string> Tooltips01 { get; set; }
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
            Labels01 = new List<string>();
            Tooltips01 = new List<string>();
            Labels02 = new List<string>();
            PrePareRevenuaByMonth();
            PrePareRevenuaByCat();
            DataContext = this;
        }

        /// <summary>
        /// Hàm chuẩn bị dữ liệu tổng doanh thu theo tuần vào tháng 12/2020
        /// </summary>
        private void PrePareRevenuaByMonth()
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
                Labels02.Add("Tuần " + i.ToString());
            }

            series.Add(new ColumnSeries
            {
                Title = $"Doanh thu ({Month}/{Year})",
                DataLabels = true,
                LabelPoint = point => point.Y + " USD",
                Values = values
            });


            ChartRevenuabyMonth.Series = series;
        }

        /// <summary>
        /// Hàm chuẩn bị dữ liệu cho đồ thị theo loại bánh vào tháng 12/2020
        /// </summary>
        private void PrePareRevenuaByCat()
        {
            int catNum = (int)App.appDAO.CategoryCount();
            var series = new SeriesCollection();

            var values = new List<ChartValues<double>>(catNum);

            for (int p = 0; p < catNum; p++)
            {
                values.Add(new ChartValues<double>());
            };

            long profit = 0;
            List<long> profitList = new List<long>(catNum);

            for (int id = 0; id < catNum; id++)
            {
                profitList.Add((long)0);
            }

            for (int w = 0; w < 4; w++)
            {
                var list1 = App.appDAO.TotalOrders_CakeCat(w, Month, Year);
                var list2 = App.appDAO.TotalReceives_CakeCat(w, Month, Year);

                if (Tooltips01.Count == 0)
                {
                    for (int j = 0; j < catNum; j++)
                    {
                        Tooltips01.Add(list1[j].Item2);
                    }
                }
                else { }

                for (int id = 0; id < catNum; id++)
                {
                    profit = list1[id].Item3 - list2[id].Item3;
                    profitList[id] += profit;
                    values[id].Add(Double.Parse(profit.ToString()));
                };

            }

            // Khởi tạo dữ liệu cho pieChart
            Func<ChartPoint, string> labelPoint =
                chartPoint =>
            string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            var tmp = new SeriesCollection();

            for (int id = 0; id < catNum; id++)
            {
                tmp.Add(new PieSeries
                {
                    Title = Tooltips01[id],
                    Values = new ChartValues<double> { Double.Parse(profitList[id].ToString()) },
                    DataLabels = false,
                    ToolTip = Tooltips01[id],
                    LabelPoint = labelPoint
                });
            }
            cat_cakeProfitPieChart.Series = tmp;

            for (int id = 0; id < catNum; id++)
            {
                series.Add(
                    new LineSeries
                    {
                        ToolTip = Tooltips01[id],
                        Title = $"{Tooltips01[id]}",
                        Values = values[id]
                    });
            }

            Chart01a.Series = series;
        }

        /// <summary>
        /// Hàm sử lý khi nhấn chuột vào bánh trên PieChart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="chartPoint"></param>
        private void Chart_OnDataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            var chart = chartPoint.ChartView as PieChart;

            foreach (PieSeries series in chart.Series)
            {
                series.PushOut = 0;
            }

            var selectedSeries = chartPoint.SeriesView as PieSeries;
            selectedSeries.PushOut = 8;
        }
    }
}

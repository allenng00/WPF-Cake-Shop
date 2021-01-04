using CakeShop.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CakeShop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimiseButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maximiseButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }
        /// <summary>
        /// Hàm xử lí khi Close Button được nhấn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Hàm xử lí sau khi các phần tử của cửa khổ khởi động xong
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Button05.IsChecked = true;
            mainContentFrame.Content =
                            (App.aboutPage == null)
                            ? new Views.AboutUs()
                            : App.aboutPage;
        }

        private void TabMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var index = int.Parse(((RadioButton)e.Source).Uid);

            switch (index)
            {
                case 1:
                    {
                        mainContentFrame.Content =
                            (App.ordersPage == null)
                            ? new Views.Orders()
                            : App.ordersPage;
                        break;
                    }
                case 2:
                    {
                        mainContentFrame.Content =
                            (App.receivesPage == null)
                            ? new Views.Receives()
                            : App.receivesPage;
                        break;
                    }
                case 3:
                    {
                        mainContentFrame.Content =
                            (App.statisticsPage == null)
                            ? new Views.Statistics()
                            : App.statisticsPage;
                        break;
                    }
                case 4:
                    {
                        mainContentFrame.Content =
                            (App.settingsPage == null)
                            ? new Views.Settings()
                            : App.settingsPage;
                        break;
                    }
                case 5:
                    {
                        mainContentFrame.Content =
                            (App.aboutPage == null)
                            ? new Views.AboutUs()
                            : App.aboutPage;
                        break;
                    }
                default:
                    {
                        mainContentFrame.Content =
                            (App.homePage == null)
                            ? new Views.Home()
                            : App.homePage;
                        break;
                    }
            }
        }

        //private void addImgButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (cakesComboBox.SelectedIndex >= 0)
        //    {
        //        //Hiển thị cửa sổ chọn ảnh

        //        var screen = new OpenFileDialog();
        //        // Thiết đặt bộ lọc (filter) cho file hình ảnh
        //        var codecs = ImageCodecInfo.GetImageEncoders();
        //        var sep = string.Empty;

        //        foreach (var c in codecs)
        //        {
        //            string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
        //            screen.Filter = String.Format("{0}{1}{2} ({3})|{3}", screen.Filter, sep, codecName, c.FilenameExtension);
        //            sep = "|";
        //        }
        //        screen.Filter = String.Format("{0}{1}{2} ({3})|{3}", screen.Filter, sep, "All Files", "*.*");
        //        screen.Title = "Chọn ảnh lộ trình";
        //        screen.Multiselect = false;
        //        screen.FilterIndex = 6;
        //        screen.RestoreDirectory = true;
        //        // Lưu hình ảnh
        //        if (screen.ShowDialog() == true)
        //        {
        //            var path = screen.FileName;
        //            Debug.WriteLine(path);
        //            //Đổi ảnh sang mảng byte
        //            var array = UnknownImageToByteArrayConverter.ImageToByteArray(path);
        //            //Luu anh lai
        //            var cake = cakesComboBox.SelectedItem as CAKE;
        //            cake.AvatarImage = array;
        //            var index = cakesComboBox.SelectedIndex;
        //            Debug.WriteLine(mainList[index].AvatarImage.ToString());
        //            dao.UpdateDatabase();
        //        }
        //    }
        //}


    }
}

using CakeShop.View;
using CakeShop.Views;
using System;
using System.Windows;
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
            //Dispatcher.Invoke(() =>
            //{
            //    var left = Button00.Padding.Left + 10;
            //    Double width = Button00.Padding.Left + Label00.Width;
            //    this.cursorStackPanel.Margin = new Thickness(left, 0, 0, 0);
            //    this.cursorStackPanel.Width = width;
            //    this.cursorStackPanel.UpdateLayout();
            //});
        }

        private void Button03_Click(object sender, RoutedEventArgs e)
        {
            mainContentFrame.Content = new Statistics();
        }

      

        private void Cakes_Click(object sender, RoutedEventArgs e)
        {
            mainContentFrame.Content = new NewCake();
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            mainContentFrame.Content = new Order();
        }

        private void NewCake_Click(object sender, RoutedEventArgs e)
        {
            mainContentFrame.Content = new NewCake();
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

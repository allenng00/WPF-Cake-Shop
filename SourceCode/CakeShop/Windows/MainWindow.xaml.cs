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

        /// <summary>
        /// Hàm khởi tạo các phần tử trong cửa sổ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Hàm xử lí kéo thả cửa sổ
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
        /// Hàm xử lí khi nhấn mini Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimiseButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Hàm xử lí khi nhấn maximise Button
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
            Button02.IsChecked = true;
            App.receivesPage = new Views.Receives();
            mainContentFrame.Content = App.receivesPage;
        }

        /// <summary>
        /// Hàm xử lí khi bấn vào các tab menu để chuyển đổi trang
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var index = int.Parse(((RadioButton)e.Source).Uid);

            switch (index)
            {
                case 1: // Orders Page
                    {
                        if (App.ordersPage == null)
                        {
                            App.ordersPage = new Views.Orders();
                        }
                        else { }
                        mainContentFrame.Content = new Orders();
                        break;
                    }
                case 2: // Receives Page
                    {
                        if (App.receivesPage == null)
                        {
                            App.receivesPage = new Views.Receives();
                        }
                        else { }
                        mainContentFrame.Content = App.receivesPage;
                        break;
                    }
                case 3: // Statistics Page
                    {
                        if (App.statisticsPage == null)
                        {
                            App.statisticsPage = new Views.Statistics();
                        }
                        else { }
                        mainContentFrame.Content = App.statisticsPage;
                        break;
                    }
                case 4: // Settings Page
                    {
                        if (App.settingsPage == null)
                        {
                            App.settingsPage = new Views.Settings();
                        }
                        else { }
                        mainContentFrame.Content = App.settingsPage;
                        break;
                    }
                case 5: // About Page
                    {
                        if (App.aboutPage == null)
                        {
                            App.aboutPage = new Views.AboutUs();
                        }
                        else { }
                        mainContentFrame.Content = App.aboutPage;
                        break;
                    }
                case 6: // About Page
                    {
                        if (App.newCake == null)
                        {
                            App.newCake = new Views.NewCake();
                        }
                        else { }
                        mainContentFrame.Content = App.newCake;
                        break;
                    }
                case 7: // About Page
                    {
                        if (App.newOrder == null)
                        {
                            App.newOrder = new Views.NewOrder();
                        }
                        else { }
                        mainContentFrame.Content = App.newOrder;
                        break;
                    }
                default:
                    {
                        if (App.homePage == null)
                        {
                            App.homePage = new Views.Home();
                        }
                        else { }
                        mainContentFrame.Content = new Home();
                        break;
                    }
            }
        }



        private void Cakes_Click(object sender, RoutedEventArgs e)
        {
            mainContentFrame.Content = new NewCake();
        }

      
    }
}

using System.Configuration;
using System.Windows;
using System.Windows.Input;
using CakeShop.Data;
using System.Windows.Threading;


namespace CakeShop.Windows
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        #region Constant
        private const int MaxTime = 15; // Thời gian hiển thị tối đa hiển thị  
        #endregion

        private int MyTime = 0; // Biến đếm thời gian hiển thị của màn hình
        private System.Timers.Timer _timer; // Biến timer để đếm thời gian chạy của chương trình
        private ViewModels.SplashScreenViewModel _mainVM; // main view model cho window

        /// <summary>
        /// Hàm khởi tạo màn hình Splash Screen
        /// </summary>
        public SplashScreen(CAKE cake)
        {
            InitializeComponent();
            _mainVM = new ViewModels.SplashScreenViewModel(cake, MaxTime.ToString());
            DataContext = _mainVM;
        }

        /// <summary>
        /// Hàm khi màn hình khởi tạo xong.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyTime = 0;
            _timer = new System.Timers.Timer();
            _timer.Elapsed += Timer_Elapsed;
            _timer.Interval = 1000;
            _timer.Start();
        }

        /// <summary>
        /// Hàm xử lí bộ đếm thời gian sau mỗi chu kỳ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Tăng biến đếm thời gian
            MyTime++;

            // Hết thời gian
            if (MyTime == MaxTime)
            {
                // Dừng bộ đếm
                _timer.Stop();

                Dispatcher.Invoke(() =>
 {
     if (neverShowAgainCheckBox.IsChecked == true)
     {
         UpdateAppConfiguration("ShowSplashScreen", false);
     }
     else { }

     App.mainWindow = new MainWindow();
     App.mainWindow.Show();
     this.Close();
 });
            }
            else { }

            // Cập nhật tiến trình processbar 
            Dispatcher.Invoke(() =>
            {
                timeProgressBar.Value = MyTime;
            });
        }

        /// <summary>
        /// Hàm cập nhật thẻ appsettings trong app.config
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void UpdateAppConfiguration(string name, object value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[name].Value = value.ToString();
            config.Save(ConfigurationSaveMode.Minimal);
            //
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Hàm xử lí sự kiện khi nhấn vào button skip Splash Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skipButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();

            if (neverShowAgainCheckBox.IsChecked == true)
            {
                UpdateAppConfiguration("ShowSplashScreen", false);
            }
            else { }

            App.mainWindow = new MainWindow();
            App.mainWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Hàm xử lí di chuyển màn hình
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Border;
            var win = Window.GetWindow(move);
            win.DragMove();
        }


    }
}

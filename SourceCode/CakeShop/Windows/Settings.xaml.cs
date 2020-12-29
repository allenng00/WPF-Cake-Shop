using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CakeShop.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hàm xử lí hiện footer cho menu settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var index = int.Parse(((Button)e.Source).Uid);

            GridCursor.Margin = new Thickness(20 + index * 150, 0, 0, 0);
            settingGrid.Children[index].Visibility = Visibility.Visible;

            for (var i = 0; i < settingGrid.Children.Count; i++)
            {
                if (i != index)
                {
                    settingGrid.Children[i].Visibility = Visibility.Hidden;
                }
                else { }
            }
        }

        /// <summary>
        /// Hàm xử lí khi của số Setting đang đóng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (splashScreenToggleBtn.IsChecked == true)
            {
                UpdateAppConfiguration("ShowSplashScreen", true);
            }
            else
            {
                UpdateAppConfiguration("ShowSplashScreen", false);
            }
        }

        /// <summary>
        /// Hàm cập nhật các key appSettings trong App.config
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
        /// Hàm xử lí khi nhấn close Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeSettingButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

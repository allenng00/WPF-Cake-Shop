using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using CakeShop.Data;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CakeShopDAO AppDAO;

        public static Windows.MainWindow mainWindow;
        //public static 

        /// <summary>
        /// Hàm xử lí các sự kiện khi áp được khởi động
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDAO = new CakeShopDAO();
            var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
            var showSplash = bool.Parse(value);

            // Nếu không hiển thị SlashScreen
            if (showSplash == false)
            {
                mainWindow = new Windows.MainWindow();
                mainWindow.Show();
            }
            // Hiển thị SplashScreen
            else
            {
                var cakes = AppDAO.CakeList();
                var _rng = new Random();
                var cakeIndex = _rng.Next(cakes.Count);
                var cake = cakes[cakeIndex];
               
                var screen = new Windows.SplashScreen();
                screen.Show();
            }
        }

        public static void AddConnectionStringSettings(
           System.Configuration.Configuration config,
             System.Configuration.ConnectionStringSettings conStringSettings)

        {
            ConnectionStringsSection connectionStringsSection = config.ConnectionStrings;
            connectionStringsSection.ConnectionStrings.Add(conStringSettings);
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

    }
}

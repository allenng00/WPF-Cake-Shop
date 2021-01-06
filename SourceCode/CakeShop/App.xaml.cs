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
        public static int currentTabIndex = 0; 
        public static CakeShopDAO appDAO;

        public static Windows.MainWindow mainWindow;
        public static Views.Home homePage;
        public static Views.Orders ordersPage;
        public static Views.Receives receivesPage;
        public static Views.Statistics statisticsPage;
        public static Views.Settings settingsPage;
        public static Views.AboutUs aboutPage;
        public static Views.NewCake newCake;
        public static Views.NewOrder newOrder;
        /// <summary>
        /// Hàm xử lí các sự kiện khi áp được khởi động
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            appDAO = new CakeShopDAO();
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
                var cakes = appDAO.CakeList();
                //var cake2 = new CAKE

                var _rng = new Random();
                var cakeIndex = _rng.Next(cakes.Count);
                var cake = cakes[cakeIndex];
               
                var screen = new Windows.SplashScreen(cake);
                screen.Show();
            }
        }

        public static void AddConnectionStringSettings(
           Configuration config,
             ConnectionStringSettings conStringSettings)

        {
            ConnectionStringsSection connectionStringsSection = config.ConnectionStrings;
            connectionStringsSection.ConnectionStrings.Add(conStringSettings);
            config.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

    }
}

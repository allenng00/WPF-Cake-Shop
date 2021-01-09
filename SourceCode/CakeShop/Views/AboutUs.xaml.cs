using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows.Controls;

namespace CakeShop.Views
{
    /// <summary>
    /// Interaction logic for AboutUs.xaml
    /// </summary>
    public partial class AboutUs : Page
    {
        private ViewModels.AboutUsViewModel _mainVM;

        /// <summary>
        /// Hàm khởi tạo trang
        /// </summary>
        public AboutUs()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hàm xử lí sau khi trang about đã khởi tạo xong
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainVM = new ViewModels.AboutUsViewModel();
            DataContext = _mainVM;
        }

        /// <summary>
        /// Hàm liên hệ qua facebook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactFb_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = sender as Chip;
            var content = item.Content as TextBlock;
            var text = content.Text;

            Process.Start(@"https://m.me/" + text);
        }

        /// <summary>
        /// Hàm liên hệ qua email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactEmail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = sender as Chip;
            var content = item.Content as TextBlock;
            var text = content.Text + "@gmail.com";

            Process.Start(@"mailto:" + text + "?subject=WPF.App.Cakeshop Question");
        }

        /// <summary>
        /// Hàm liên hệ qua facebook của team
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TeamContactFb_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(@"https://facebook.com/" + _mainVM.Members[0].Facebook);
        }

        private void TeamContactGg_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void TeamContactGit_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}

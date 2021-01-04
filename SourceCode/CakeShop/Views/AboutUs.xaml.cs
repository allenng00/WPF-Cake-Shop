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
    }
}

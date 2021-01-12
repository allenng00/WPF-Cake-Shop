using System.Windows;
using System.Windows.Controls;

namespace CakeShop.Views
{
    /// <summary>
    /// Interaction logic for Receive.xaml
    /// </summary>
    public partial class Receives : Page
    {
        private ViewModels.ReceiveViewModel _mainVM;

        public Receives()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _mainVM = new ViewModels.ReceiveViewModel();
            DataContext = _mainVM;
        }

        /// <summary>
        /// Hàm cập nhật Item Source cho Detail Receive ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void receiveDetailBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = long.Parse(((Button)e.Source).Uid);
            var receive = _mainVM.ReceiveList.Find(r => r.ID == index);

            _mainVM.CurrentReceiveID = index;
            _mainVM.CurrentCakeList = receive.CakeList;
        }

        private void addReceiveBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

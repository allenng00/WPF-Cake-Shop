using System.Windows;
using System.Windows.Controls;

namespace CakeShop.Views
{
    /// <summary>
    /// Interaction logic for NewReceive.xaml
    /// </summary>
    public partial class NewReceive : Page
    {
        private ViewModels.NewReceiveViewModel _mainVM;

        public NewReceive(long nextID)
        {
            InitializeComponent();
            _mainVM = new ViewModels.NewReceiveViewModel();
            _mainVM.MainReceive.ID = nextID;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainVM;
        }

        private void AddCakes_Click(object sender, RoutedEventArgs e)
        {
            _mainVM.MainReceive.CakeList.Add(_mainVM.CurrentCake);
            receiveDetailListView.ItemsSource = _mainVM.MainReceive.CakeList;
            _mainVM.CurrentCake = new ViewModels.CakeModel_ReceiveModel();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mainVM.CurrentCatID = (sender as ComboBox).SelectedIndex + 1;

            _mainVM.CakeByCat();
        }

        private void DownQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (_mainVM.CurrentCake.Num > 1)
            {
                _mainVM.CurrentCake.Num--;

            }
            else
            {

            }
        }

        private void UpQuantity_Click(object sender, RoutedEventArgs e)
        {
            _mainVM.CurrentCake.Num++;
        }

        private void addReceiveButton_Click(object sender, RoutedEventArgs e)
        {
           

        }

        private void cancelReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.SelectedIndex = -1;
            cakeCombobox.SelectedIndex = -1;
            _mainVM.CurrentCake.Num = 1;
            _mainVM.CurrentCake.Price = 0;
            _mainVM.MainReceive.CakeList.Clear();
        }

        private void cakeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cake = (sender as ComboBox).SelectedItem as Data.CAKE;

            _mainVM.CurrentCake.ID = cake.ID;
            _mainVM.CurrentCake.Name = cake.Name;
        }
    }
}

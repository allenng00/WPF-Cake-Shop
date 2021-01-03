using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Data;
using CakeShop.Data;
using Microsoft.Win32;

namespace CakeShop.View
{
    /// <summary>
    /// Interaction logic for AddNewOrder.xaml
    /// </summary>
    public partial class AddNewOrder : Page
    {
        CakeShopDAO dao;
        List<CATEGORY> CATEGORies;
        List<CAKE> CAKEs;
        public AddNewOrder()
        {
            InitializeComponent();
            Prepare();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void  Prepare()
        {
            dao = new CakeShopDAO();
            CATEGORies = dao.CategoryList();
            CAKEs = dao.CakeList();
            //CakeListView.ItemsSource = CAKEs;
            CategoryComboBox.ItemsSource = CATEGORies;
        }

        private void RefreshDataInput()
        {

        }

        private void CheckDataInputError()
        {

        }

        private void StoreDataInput()
        {

        }

        private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

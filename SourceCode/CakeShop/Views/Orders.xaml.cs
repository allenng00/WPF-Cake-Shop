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
    /// 
    #region 
    ///ViewModel for Order.xmal
    ///
    public class OrderViewModel
    {
        CakeShopDAO dao;
        public OrderViewModel()
        {
            dao = new CakeShopDAO();
        }
    }

    #endregion

    public partial class Order: Page
    {
        public Order()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
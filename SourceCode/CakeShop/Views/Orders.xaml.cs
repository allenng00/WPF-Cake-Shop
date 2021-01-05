using CakeShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CakeShop.Views
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

    public partial class Orders : Page
    {
        CakeShopDAO dao;
        List<CATEGORY> CATEGORies;
        List<CAKE> CAKEs;
        List<STATUS> STATUs;
        List<CakeInCart> cakeInCarts;
        AddNewOrderViewModel _mainvm;
        long No_;//Số thứ tự bánh trong giỏ hàng hiện tại;
        long TotalBill;// Tổng giá trị giỏ hàng

        public Orders()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
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
    /// Model for AddNewOrder.xmal
    /// 
    public class CakeInCart
    {
        public string Name { get; set; }
        public long Quantity { get; set; }
        public long TotalCost { get; set; }
        public long No { get; set; }
        public long CakeID { get; set; }
    }
    #endregion


    #region
    /// ViewModel for AddNewOrder
    /// 
    public class AddNewOrderViewModel
    {
        CakeShopDAO dao;
        public AddNewOrderViewModel()
        {
            dao = new CakeShopDAO();
        }
        public List<CATEGORY> GetCATEGORies()
        {
            return dao.CategoryList();
        }
        public List<CAKE> GetCAKEs()
        {
            return dao.CakeList();
        }

        public List<CAKE> GetCAKEs(long CatID)
        {
            return dao.CakeList(CatID);
        }

        public List<STATUS> GetSTATUs()
        {
            return dao.StatusList();
        }
    }
    #endregion

    public partial class Order : Page
    {
        CakeShopDAO dao;
        List<CATEGORY> CATEGORies;
        List<CAKE> CAKEs;
        List<STATUS> STATUs;
        List<CakeInCart> cakeInCarts;
        AddNewOrderViewModel _mainvm;
        public Order()
        {
            InitializeComponent();
            Prepare();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Prepare()
        {
            dao = new CakeShopDAO();
            _mainvm = new AddNewOrderViewModel();

            STATUs = _mainvm.GetSTATUs();
            CAKEs = _mainvm.GetCAKEs();
            cakeInCarts = new List<CakeInCart>();

            CATEGORies = _mainvm.GetCATEGORies();
            CATEGORies.Reverse();
            CATEGORies.Add(new CATEGORY
            {
                ID = 0,
                Name = "Tất cả",
            });
            CATEGORies.Reverse();

            StatusComboBox.ItemsSource = STATUs;
            CakeListView.ItemsSource = CAKEs;
            CategoryComboBox.ItemsSource = CATEGORies;
            CartDataGrid.ItemsSource = cakeInCarts;
            CartCost.Text = "0" + " VND";

            CategoryComboBox.SelectedIndex = 0;
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

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = CategoryComboBox.SelectedIndex;

            if (index != 0)// không phải lựa chọn tất cả
            {
                long id = CATEGORies[index].ID;
                CAKEs = _mainvm.GetCAKEs(id);
                CakeListView.ItemsSource = CAKEs;
            }
            else if (index == 0)
            {
                CAKEs = _mainvm.GetCAKEs();
                CakeListView.ItemsSource = CAKEs;
            }
        }

        private void UpQuantity_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;
            long quantity;
            try
            {
                quantity = long.Parse(cakeQuantity.Text.ToString());
                Console.WriteLine(quantity);
                if (quantity < 0)
                    throw new Exception("error");
            }
            catch (Exception ex)
            {
                check = false;
                quantity = 1;
            }
            if (check == true)
            {
                quantity++;
            }
            cakeQuantity.Text = quantity.ToString();

        }

        private void DownQuantity_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;
            long quantity;
            try
            {
                quantity = long.Parse(cakeQuantity.Text.ToString());
                if (quantity < 2)
                    throw new Exception("error");
            }
            catch (Exception ex)
            {
                check = false;
                quantity = 1;
            }
            if (check == true)
            {
                quantity--;
            }
            cakeQuantity.Text = quantity.ToString();
        }

        private void CakeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = CakeListView.SelectedIndex;
            if (index != -1)
            {
                cakeName.Text = CAKEs[index].Name;
                int num = 1;
                cakeQuantity.Text = num.ToString();
            }
        }

        private void AddCurrentCake_Click(object sender, RoutedEventArgs e)
        {
            int index = CakeListView.SelectedIndex;
            if (index != -1)
            {
                CAKE curCake = CAKEs[index];
                try
                {
                    long quantity = long.Parse(cakeQuantity.Text);
                    if (quantity < 1)
                        throw new Exception("1");
                    CakeInCart curCakeInCart = new CakeInCart
                    {
                        CakeID = curCake.ID,
                        Name = curCake.Name,
                        Quantity = quantity,
                        TotalCost = (long)(curCake.SellPrice * quantity),
                        No = 1,
                    };
                    cakeInCarts.Add(curCakeInCart);
                    CartDataGrid.ItemsSource = null;
                    CartDataGrid.ItemsSource = cakeInCarts;
                    UpdateTotalCost();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bạn nhập số lượng bánh sai", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void UpdateTotalCost()
        {
            if (cakeInCarts.Count != 0)
            {
                long sum = 0;
                foreach (var i in cakeInCarts)
                {
                    if (i.TotalCost > 0)
                        sum += i.TotalCost;
                    else
                        return;
                }
                CartCost.Text = sum.ToString() + " VNĐ";
            }
        }
    }
}
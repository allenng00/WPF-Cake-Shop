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

        public long GetORDERsCount()
        {
            return dao.OrderCount();
        }
    }
    #endregion

    public partial class NewOrder : Page
    {
        CakeShopDAO dao;
        List<CATEGORY> CATEGORies;
        List<CAKE> CAKEs;
        List<STATUS> STATUs;
        List<CakeInCart> cakeInCarts;
        AddNewOrderViewModel _mainvm;
        long No_;//Số thứ tự bánh trong giỏ hàng hiện tại;
        long TotalBill;// Tổng giá trị giỏ hàng

        public NewOrder()
        {
            InitializeComponent();
            Prepare();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Prepare()
        {
            //Set local variable - DAO, ViewModel
            dao = new CakeShopDAO();
            _mainvm = new AddNewOrderViewModel();

            //Set local variable
            STATUs = _mainvm.GetSTATUs();
            CAKEs = _mainvm.GetCAKEs();
            cakeInCarts = new List<CakeInCart>();
            No_ = 0;
            TotalBill = 0;

            //Set CATEGORies
            CATEGORies = _mainvm.GetCATEGORies();
            CATEGORies.Reverse();
            CATEGORies.Add(new CATEGORY
            {
                ID = 0,
                Name = "Tất cả",
            });
            CATEGORies.Reverse();

            //Update UI
            StatusComboBox.ItemsSource = STATUs;
            CakeListView.ItemsSource = CAKEs;
            CategoryComboBox.ItemsSource = CATEGORies;
            CartDataGrid.ItemsSource = cakeInCarts;
            CartCost.Text = "0" + " VNĐ";
            CreatedDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            //Set selected index
            CategoryComboBox.SelectedIndex = 0;
        }

        private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            bool check = CheckDataInputError();
            if (check == true)
            {
                try
                {
                    StoreDataInput();
                    RefreshDataInput();
                    Prepare();
                    MessageBox.Show("Tạo đơn hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshDataInput();
            Prepare();
        }

        private void StoreDataInput()
        {
            bool check = CheckDataInputError();
            if (check == true)
            {

                try
                {
                    OurCakeShopEntities database = new OurCakeShopEntities();
                    long orderId = _mainvm.GetORDERsCount();

                    //insert order
                    ORDER curOrder = new ORDER
                    {
                        ID = orderId + 1,
                        Status = STATUs[StatusComboBox.SelectedIndex].ID,
                        DateCompleted = DateTime.UtcNow,
                        TotalBill = this.TotalBill,
                        BuyingMethod = STATUs[StatusComboBox.SelectedIndex].ID,
                        CustomerName = customerName.Text,
                        CustomerAddress = customerAddress.Text,
                        CustomerPhone = customerPhoneNumber.Text,

                    };
                    database.ORDERs.Add(curOrder);
                    database.SaveChanges();
                    Console.WriteLine(1);
                    foreach (var detail in cakeInCarts)
                    {
                        ORDER_DETAIL cur = new ORDER_DETAIL
                        {
                            OrderID = orderId + 1,
                            No_ = detail.No,
                            ProductNum = detail.Quantity,
                            ProductID = detail.CakeID,
                            Price = detail.TotalCost,
                        };
                        database.ORDER_DETAIL.Add(cur);
                        database.SaveChanges();
                        Console.WriteLine(3);
                    }
                    database.SaveChanges();
                    Console.WriteLine(2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("Tạo đơn hàng không thành công\n Vui lòng thử lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void RefreshDataInput()
        {
            //Refresh customer frame
            customerName.Text = "";
            customerAddress.Text = "";
            customerPhoneNumber.Text = "";
            StatusComboBox.SelectedIndex = -1;

            //Refresh cart frame
            cakeName.Text = "";
            cakeQuantity.Text = "";
            CartCost.Text = "0 VNĐ";
            CartDataGrid.ItemsSource = null;


            //Refresh cakeList frame
            CategoryComboBox.ItemsSource = null;
            CakeListView.ItemsSource = null;
        }

        private bool CheckDataInputError()
        {
            bool result = true;

            // - Check Cart Info
            // + Check số lượng bánh trong đơn hàng ko rỗng
            if (cakeInCarts.Count() == 0)
            {
                result = false;
                MessageBox.Show("Bạn chưa chọn bánh cho đơn hàng hiện tại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            // - Check Customer Info
            // + Check nhập đủ thông tin hay không
            else if (customerName.Text == "")
            {
                result = false;
                MessageBox.Show("Bạn chưa nhập tên khách hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (customerPhoneNumber.Text == "")
            {
                result = false;
                MessageBox.Show("Bạn chưa nhập sô điện thoại khách hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (StatusComboBox.SelectedIndex == -1)
            {
                result = false;
                MessageBox.Show("Bạn chưa chọn hình thức giao hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return result;
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
                        No = ++No_,
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

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = CategoryComboBox.SelectedIndex;


            if (index != 0 && index != -1)// không phải lựa chọn tất cả
            {
                long id = CATEGORies[index].ID;
                CAKEs = _mainvm.GetCAKEs(id);
                CakeListView.ItemsSource = CAKEs;
            }
            else if (index == 0 && index != -1)
            {
                CAKEs = _mainvm.GetCAKEs();
                CakeListView.ItemsSource = CAKEs;
            }
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

        private void UpdateTotalCost()
        {
            if (cakeInCarts.Count != 0)
            {
                TotalBill = 0;
                foreach (var i in cakeInCarts)
                {
                    if (i.TotalCost > 0)
                        TotalBill += i.TotalCost;
                    else
                        return;
                }
                CartCost.Text = TotalBill.ToString() + " VNĐ";
            }
        }
    }
}
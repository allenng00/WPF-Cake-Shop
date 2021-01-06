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

namespace CakeShop.Views
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
        public long InventoryNumber;
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
            List<STATUS> result = new List<STATUS>();
            result.Add(dao.GetStatusByID("OBM01"));//mua hàng trực tiếp
            result.Add(dao.GetStatusByID("OBM02"));//mua hàng online
            return result;
        }

        public long GetORDERsCount()
        {
            return dao.OrderCount();
        }

        public bool UpdateInvetoryCake(long CakeID, long InventoryNumber)
        {
            bool check = dao.UpdateInvetoryCake(CakeID, InventoryNumber);
            return check;
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
            moneyCalculationFrame.Visibility = Visibility.Hidden;

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

        private void AddCurrentCake_Click(object sender, RoutedEventArgs e)
        {
            int index = CakeListView.SelectedIndex;

            if (index != -1)
            {
                CAKE curCake = CAKEs[index];

                try
                {
                    long quantity = long.Parse(cakeQuantity.Text);
                    //Check dữ liệu
                    if (quantity < 1)
                        throw new Exception("1");
                    if (CAKEs[index].InventoryNum - quantity < 0)
                        throw new Exception("2");

                    //Khởi tạo curCakeInCart
                    CakeInCart curCakeInCart = new CakeInCart
                    {
                        CakeID = curCake.ID,
                        Name = curCake.Name,
                        Quantity = quantity,
                        TotalCost = (long)(curCake.SellPrice * quantity),
                        No = ++No_,
                        InventoryNumber =(long)( CAKEs[index].InventoryNum - quantity),
                    };

                    //Update CartDataGrid
                    cakeInCarts.Add(curCakeInCart);
                    CartDataGrid.ItemsSource = null;
                    CartDataGrid.ItemsSource = cakeInCarts;

                    //Update InvetoryNumber
                    inventoryNumber.Text = (curCakeInCart.InventoryNumber == 0) ? "Hết hàng" : curCakeInCart.InventoryNumber.ToString();
                    CAKEs[index].InventoryNum -= quantity;
                    
                    //Update TotalCost
                    UpdateTotalCost();
                }
                catch (Exception ex)
                {
                    if (ex.Message == "1"){
                        MessageBox.Show("Số lượng bánh phải lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (ex.Message=="2"){
                        MessageBox.Show("Số lượng phải nhỏ hơn hoặc bằng lượng tồn kho", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else{
                        MessageBox.Show("Số lượng bánh phải là chuỗi số", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void StoreDataInput()
        {
            bool check = CheckDataInputError();
            if (check == true)
            {
                OurCakeShopEntities database = new OurCakeShopEntities();

                try
                {
                    long orderId = _mainvm.GetORDERsCount();
                    string str= DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss");
                    DateTime time = DateTime.Parse(str);
                    //insert order
                    Console.WriteLine(orderId);
                    ORDER curOrder = new ORDER
                    {
                        ID = orderId + 1,
                        Status = "OS11",
                        DateCompleted = time,
                        TotalBill = this.TotalBill,
                        BuyingMethod = STATUs[StatusComboBox.SelectedIndex].ID,
                        CustomerName = customerName.Text,
                        CustomerAddress = customerAddress.Text,
                        CustomerPhone = customerPhoneNumber.Text,

                    };
                    database.ORDERs.Add(curOrder);
                    Console.WriteLine(curOrder.ID);
                    database.SaveChanges();
                    Console.WriteLine(orderId + 1);
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
                        _mainvm.UpdateInvetoryCake(detail.CakeID, detail.InventoryNumber);
                    }
                    database.SaveChanges();
                    MessageBox.Show("Tạo đơn hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
            inventoryNumber.Text = null;

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
            else if (StatusComboBox.SelectedIndex == -1)
            {
                result = false;
                MessageBox.Show("Bạn chưa chọn hình thức giao hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return result;
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
                inventoryNumber.Text = CAKEs[index].InventoryNum.ToString();

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

        private void ReceivedMoney_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CalculateMoney();
        }

        private void ShippingFee_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CalculateMoney();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusComboBox.SelectedIndex != -1)
            {
                STATUS cur = StatusComboBox.SelectedItem as STATUS;
                if (cur.ID == "OBM01")//mua hàng trực tiếp
                {
                    moneyCalculationFrame.Visibility = Visibility.Visible;
                    ShippingFee.Visibility = Visibility.Collapsed;
                }
                if (cur.ID == "OBM02")//mua hàng online
                {
                    moneyCalculationFrame.Visibility = Visibility.Visible;
                    ShippingFee.Visibility = Visibility.Visible;
                }
            }
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
                CalculateMoney();
                CartCost.Text = TotalBill.ToString() + " VNĐ";
            }
        }

        public void CalculateMoney()
        {
            long receivedMoney = 0;
            long shippingFee = 0;
            try
            {
                if (ReceivedMoney.Text != "")
                    receivedMoney = long.Parse(ReceivedMoney.Text);
                if (ShippingFee.Text != "")
                    shippingFee = long.Parse(ShippingFee.Text);
                if (receivedMoney < 0 || shippingFee < 0)
                    throw new Exception("1");
                long refundMoney = receivedMoney + shippingFee - TotalBill;
                RefundMoney.Text = refundMoney.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Số tiền bạn nhập vào không hợp lệ \n Là chuỗi số và lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                RefundMoney.Text = "";
                ShippingFee.Text = "";
                RefundMoney.Text = "";
            }
        }

        
    }
}
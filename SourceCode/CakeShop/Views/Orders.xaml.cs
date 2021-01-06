using CakeShop.Data;
using CakeShop.Views;
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
    ///Model for Order.xmal
    ///
    public class OrderModel
    {
        public long ID { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public long TotalBill { get; set; }
        public DateTime DateCompleted { get; set; }
        public string BuyingMethod { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
#endregion

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

        public List<OrderModel> GetOrders()
        {
            List<ORDER> orderList= dao.OrderList();
            List<OrderModel> result = new List<OrderModel>();
            foreach(var cur in orderList)
            {
                result.Add(new OrderModel
                {
                    ID = cur.ID,
                    Status = cur.Status,
                    StatusName = dao.GetSTATUSsName(cur.Status),
                    TotalBill=cur.TotalBill,
                    DateCompleted=cur.DateCompleted,
                    BuyingMethod=cur.BuyingMethod,
                    CustomerName=cur.CustomerName,
                    CustomerAddress=cur.CustomerAddress,
                    CustomerPhone=cur.CustomerPhone,
                }) ;
            }
            return result;
        }

        public List<CakeInCart> GetORDER_DETAILs(long OrderID)
        {
            List<ORDER_DETAIL> order_detail_list = dao.OrderDetailList(OrderID);
            List<CakeInCart> result = new List<CakeInCart>();
            foreach (var i in order_detail_list)
            {
                string name = dao.CakeName(i.ProductID);
                result.Add(new CakeInCart
                {
                    Name=name,
                    No=i.No_,
                    Quantity=i.ProductNum,
                    TotalCost=i.Price
                });

            }
            return result;
        }

        public string GetStatusName(string ID)
        {
            string result = dao.GetSTATUSsName(ID);
            return result;
        }

        public bool ChangeOrderStatus(long OrderID, string Status)
        {
            bool check=dao.UpdateOrderStatus(OrderID, Status);
            return check;
        }
    }

    #endregion

    public partial class Orders : Page
    {
        List<OrderModel> ORDERs;
        List<CakeInCart> ORDER_DETAILs;
        List<CakeInCart> CakeInCarts;
        OrderViewModel _mainvm;
        int OrderIndex;        

        public Orders()
        {
            InitializeComponent();
            Prepare();
        }

        public void Prepare()
        {

            //Set data variable
            _mainvm = new OrderViewModel();
            ORDERs = _mainvm.GetOrders();
            //Update UI
            OrdersDataGrid.ItemsSource = ORDERs;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = OrdersDataGrid.SelectedIndex;
            if(index!=-1)
            {
                long OrderID = ORDERs[index].ID;
                OrderIndex = index;
                RefreshOrderDetailFrame();
                ShowOrderDetailFrame(OrderID);
            }
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            if(App.newOrder==null)
            {
                App.newOrder =new  NewOrder();
            }
            else { }
            App.mainWindow.mainContentFrame.Content = new NewOrder();
        }

        private void IsDone_Click(object sender, RoutedEventArgs e)
        {
            if(OrderIndex != -1)
            {
                try
                {
                    //kiểm tra lại trạng thái lần nữa
                    if(ORDERs[OrderIndex].Status=="OS11") // trạng thái đang thực hiện
                    {
                        long OrderID = ORDERs[OrderIndex].ID;
                        //đổi lại dưới cơ sở dữ liệu
                        if (_mainvm.ChangeOrderStatus(OrderID, "OS12") == false)
                            throw new Exception("1");
                        //Update cartDataGrid
                        ORDERs = _mainvm.GetOrders();
                        OrdersDataGrid.ItemsSource = null;
                        OrdersDataGrid.ItemsSource = ORDERs;
                        MessageBox.Show("Thay đổi trạng thái thành công", "Thông báo", MessageBoxButton.OK);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Thay đổi trạng thái thất bại", "Thông báo", MessageBoxButton.OK);
                }

            }
        }

        private void RefreshOrderDetailFrame()
        {
            customerName.Text = "";
            customerPhoneNumber.Text = "";
            customerAddress.Text = "";
            buyingMethod.Text = "";
            createdDate.Text = "";
            TotalBill.Text = "0"+ " VNĐ";
            CartDataGrid.ItemsSource = null;
        }

        private void ShowOrderDetailFrame(long OrderID)
        {
            try
            {
                //Retrieve Data
                CakeInCarts = _mainvm.GetORDER_DETAILs(OrderID);

                //Update UI
                customerName.Text = ORDERs[OrderIndex].CustomerName;
                customerPhoneNumber.Text = ORDERs[OrderIndex].CustomerPhone;
                customerAddress.Text = ORDERs[OrderIndex].CustomerAddress;
                buyingMethod.Text = _mainvm.GetStatusName(ORDERs[OrderIndex].BuyingMethod);
                createdDate.Text = ORDERs[OrderIndex].DateCompleted.ToString();
                TotalBill.Text = ORDERs[OrderIndex].TotalBill.ToString() + " VNĐ";
                CartDataGrid.ItemsSource = null;
                CartDataGrid.ItemsSource = CakeInCarts;
                //Hide and Show button  depending on Order Status
                string status = ORDERs[(int)OrderIndex].Status;
                if(status=="OS11")//trạng thái đang thực hiện
                {
                    IsDone.Visibility = Visibility.Visible;
                }
                else if(status=="OS12")//trạng thái đã hoàn thành
                {
                    IsDone.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiện thị đơn hàng", "Thông báo", MessageBoxButton.OK);
            }
        }

    }
}
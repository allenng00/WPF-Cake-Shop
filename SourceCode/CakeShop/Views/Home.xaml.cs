using CakeShop.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CakeShop.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    /// 


    public partial class Home : Page
    {
        public ViewModels.HomeViewModel _mainVM { get; set; }
        public int CakeIndex { get; set; }
        public long CakeID { get; set; }

        /// <summary>
        /// Hàm khởi tạo Homepage
        /// </summary>
        public Home()
        {
            InitializeComponent();
            Prepare();
        }

        /// <summary>
        /// Hàm khởi tạo các tài nguyên có HomePage
        /// </summary>
        public void Prepare()
        {
            //Set data for variables
            _mainVM = new ViewModels.HomeViewModel();

            //Update UI
            DataContext = _mainVM;
            CakeListView.ItemsSource = _mainVM.CakeList;

            //Set selectedIndex
            CakeListView.SelectedIndex = -1;
            SortComboBox.SelectedIndex = -1;
            CategoryListView.SelectedIndex = 0;
        }

        /// <summary>
        /// Bắt sự kiện thay đổi loại bánh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int catIndex = CategoryListView.SelectedIndex;
            var sortIndex = SortComboBox.SelectedIndex;

            if (catIndex != -1)
            {
                if (catIndex == 0)
                {
                    _mainVM.GetCakeList(null, sortIndex);
                }
                else
                {
                    var CatID = _mainVM.CategoryList[catIndex].CatID;
                    var CatIDs = new long[] { CatID };
                    _mainVM.GetCakeList(CatIDs, sortIndex);
                }

                if (_mainVM.CakeList.Count != 0)
                {
                    //Update CakeListView
                    CakeListView.ItemsSource = _mainVM.CakeList;
                }
                else { }
            }
        }

        /// <summary>
        /// Hàm xử lí sự kiện sắp xếp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sortIndex = SortComboBox.SelectedIndex;

            if (sortIndex != -1)
            {
                try
                {
                    var CategoryID = _mainVM.CategoryList[CategoryListView.SelectedIndex].CatID;

                    if (CategoryID != 0)
                    {
                        var CatIDs = new long[] { CategoryID };
                        _mainVM.GetCakeList(CatIDs, sortIndex);
                    }
                    else
                    {
                        _mainVM.GetCakeList(null, sortIndex);
                    }

                    if (_mainVM.CakeList.Count != 0)
                    {
                        //Update CakeListView
                        CakeListView.ItemsSource = _mainVM.CakeList;
                    }
                    else { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi bộ lọc không hoạt động đúng cách", "Thông báo", MessageBoxButton.OK);
                }
            }
        }

        /// <summary>
        /// Search Cake theo tên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_Click(object sender, RoutedEventArgs e)
        {
            string text = SearchBox.Text;

            try
            {
                _mainVM.SearchCAKEsByName(text);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Lỗi!Không tìm được cake ", "Thông báo", MessageBoxButton.OK);
            }

            CakeListView.ItemsSource = null;
            CakeListView.ItemsSource = _mainVM.CakeList;
        }

        /// <summary>
        /// Hàm xử lý khi nhấn double click vào bánh để xem chi tiết
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CakeListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int index = CakeListView.SelectedIndex;
                if (index != -1)
                {
                    CakeIndex = index;
                    CakeID = _mainVM.CakeList[CakeIndex].ID;
                    if (CakeID != -1)
                    {
                        App.mainWindow.mainContentFrame.Content = new CakeDetail(this.CakeID);
                    }
                    else { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị", "Thông báo", MessageBoxButton.OK);
            }
        }
    }
}

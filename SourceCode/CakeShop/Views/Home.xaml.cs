using CakeShop.Data;
using System;
using System.Collections.Generic;
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

    #region 
    ///Model for Home.xmal
    ///
    public class CategoryModel
    {
        public long CatID { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
    }
    #endregion

    #region
    ///ViewModel for Home.xmal
    ///
    public class HomeViewModel
    {
        public CakeShopDAO dao;
        public HomeViewModel()
        {
            dao = new CakeShopDAO();
        }
        public List<CategoryModel> GetCATEGORies()
        {
            List<CategoryModel> result = new List<CategoryModel>();
            List<CATEGORY> list = dao.CategoryList();
            result.Add( new CategoryModel
            {
                CatID = 0,
                Name = "Tất cả",
                Quantity=dao.CountAllCakes()
            });
            foreach (var i in list)
            {
                result.Add(new CategoryModel
                {
                    CatID = i.ID,
                    Name = i.Name,
                    Quantity = dao.CountCakesByCategory(i.ID)
                }) ;
            }
            return result.ToList();
        }
        public List<CAKE> GetCAKEs()
        {
            return dao.CakeList();
        }
        public List<CAKE> GetCAKEsByCategory(long CatID)
        {
            List<CAKE> result = new List<CAKE>();
            if(CatID==0)
            {
                result = dao.CakeList();
            }
            else
            {
                result = dao.CakeList(CatID);
            }
            return result;
        }
        
    }
    #endregion
    public partial class Home : Page
    {
        public HomeViewModel _mainvm;
        public List<CAKE> CAKEs;
        public List<CategoryModel> CATEGORies;
        
        public Home()
        {
            InitializeComponent();
            Prepare();
        }



        public void  Prepare()
        {
            //Set data for variables
            _mainvm = new HomeViewModel();
            CATEGORies = _mainvm.GetCATEGORies();
            CAKEs = _mainvm.GetCAKEs();

            //Update UI
            CakeListView.ItemsSource = CAKEs;
            CategoryListView.ItemsSource = CATEGORies;

            //Set selectedIndex
            CakeListView.SelectedIndex = -1;
            SortComboBox.SelectedIndex = -1;
            CategoryListView.SelectedIndex = 0;
        }

        private void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = CategoryListView.SelectedIndex;
            if(index!=-1)
            {
                CakeListView.ItemsSource = null;
                long CatID = CATEGORies[index].CatID;
                CAKEs = _mainvm.GetCAKEsByCategory(CatID);

                CakeListView.ItemsSource = CAKEs;
                SortComboBox.SelectedIndex = -1;
            }
        }

        private void CakeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SortCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = SortComboBox.SelectedIndex;
            if(index!=-1)
            {
                try
                {
                    long CategoryID = CATEGORies[CategoryListView.SelectedIndex].CatID;
                    string name = CATEGORies[CategoryListView.SelectedIndex].Name;
                    string[] CatName = new string[] { name };

                    switch (index)
                    {
                        //Sắp xếp theo ký tự Alphabet tăng dần
                        case 1:
                            {
                                CAKEs = _mainvm.dao.CakeList(CatName, 0);
                                break;
                            }
                        //Sắp xếp theo ký tự Alphabet giảm dần
                        case 2:
                            {
                                CAKEs = _mainvm.dao.CakeList(CatName, 1);
                                break;
                            }
                        //Sắp xếp theo giá tăng dần
                        case 3:
                            {
                                CAKEs = _mainvm.dao.CakeList(CatName, 2);
                                break;
                            }
                        //Sắp xếp theo giá giảm dần
                        case 4:
                            {
                                CAKEs = _mainvm.dao.CakeList(CatName, 3);
                                break;
                            }
                        // Sắp xếp theo lượng tồn tăng dần
                        case 5:
                            {
                                CAKEs = _mainvm.dao.CakeList(CatName, 4);
                                break;
                            }
                        // Sắp xếp theo lường tồn giảm dần
                        case 6:
                            {
                                CAKEs = _mainvm.dao.CakeList(CatName, 5);
                                break;
                            }
                        //Lấy tất cả
                        default:
                            {
                                CAKEs = _mainvm.dao.CakeList(CatName);
                                break;
                            }
                    }
                    if (CAKEs.Count != 0)
                    {
                        //Update CakeListView
                        CakeListView.ItemsSource = null;
                        CakeListView.ItemsSource = CAKEs;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Lỗi bộ lọc không hoạt động đúng cách", "Thông báo", MessageBoxButton.OK);
                    Console.WriteLine(ex);
                }
            }            
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

using CakeShop.Data;
using Microsoft.Win32;
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

namespace CakeShop.Views
{
    /// <summary>
    /// Interaction logic for CakeDetail.xaml
    /// </summary>
    /// 
    #region
    ///ViewModel for CakeDetail
    ///
    public class CakeDetailViewModel
    {
        public CakeShopDAO dao;
        public CakeDetailViewModel()
        {
            dao = new CakeShopDAO();
        }
        public CAKE GetCAKEs(long CakeID)
        {
            CAKE result = dao.GetCAKEs(CakeID);
            return result;
        }

        public bool UpdateCake(CAKE Cake,long CakeID)
        {
            bool check = true;
            try
            {
                check = dao.UpdateCake(Cake, CakeID);
            }
            catch(Exception ex)
            {
                check = false;
            }
            return check;
        }
        public string GetCategoryNameByID(long CatID)
        {
            string result = dao.CategoryNameByID(CatID);
            return result;
        }
        public List<CATEGORY> GetCATEGORies()
        {
            return dao.CategoryList();
        }
    }
    #endregion
    public partial class CakeDetail : Page
    {
        public long CakeID { get; set; }
        public CAKE CurCake { get; set; }
        public CAKE UpdatedCake { get; set; }
        public List<CATEGORY> CATEGORies { get; set; }
        public CakeDetailViewModel _mainvm { get; set; }
        public int isUpdate { get; set; }

        public CakeDetail()
        {
            InitializeComponent();
        }
        public CakeDetail(long  CakeID)
        {
            InitializeComponent();
            this.CakeID = CakeID;
            Prepare();
        }
        private void Prepare()
        {
            //Set dat for variable
            _mainvm = new CakeDetailViewModel();
            CurCake = _mainvm.GetCAKEs(CakeID);
            CATEGORies = _mainvm.GetCATEGORies();
            isUpdate = 0;

            //Show Cake Detail Frame, Hide Update Frame
            ShowCakeDetailFrame();

            //UpdateUI
            AvatarImage.Source = AlternativeByteArrayToImageConveter.Convert( CurCake.AvatarImage);
            CakeDetailFrame.DataContext = CurCake;
            CategoryName.Text = _mainvm.GetCategoryNameByID(CurCake.CatID);
        }

        private void SaveInfo_Click(object sender, RoutedEventArgs e)
        {
            //Check Data Input Error
            bool check = CheckInputError();

            //Store data in database 
            if (check==true)
            {
                string time = DateTime.UtcNow.ToString("dd-MM-yyyy HH:mm:ss");
                UpdatedCake.ID = CurCake.ID;
                UpdatedCake.Name = CakeName.Text;
                UpdatedCake.Introduction = Introduction.Text;
                UpdatedCake.Description = Description.Text;
                UpdatedCake.BasePrice = long.Parse(BasePrice.Text);
                UpdatedCake.SellPrice = long.Parse(SellPrice.Text);
                UpdatedCake.CatID = CurCake.CatID;
                UpdatedCake.DateAdded = DateTime.Parse(time);
                UpdatedCake.InventoryNum = CurCake.InventoryNum;
                UpdatedCake.CatID = CATEGORies[CategoryComboBox.SelectedIndex].ID;

                //Store Update Cake into databse
                _mainvm.UpdateCake(UpdatedCake, UpdatedCake.ID);

                //Show Cake Data Frame
                ShowCakeDetailFrame();

                //Retreive cake from database
                CurCake = _mainvm.GetCAKEs(CakeID);
                //Update UI
                CakeDetailFrame.DataContext = CurCake;
                AvatarImage.Source = AlternativeByteArrayToImageConveter.Convert(CurCake.AvatarImage);
                CategoryName.Text = _mainvm.GetCategoryNameByID(CurCake.CatID);
                //Inform success
                MessageBox.Show("Update dữ liệu thành công", "Thông báo", MessageBoxButton.OK);
                //Mark Updated
                isUpdate++;
            }
        }


        private void UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            UpdatedCake = new CAKE();

            ShowUpdateCakeFrame();
            CurCake = _mainvm.GetCAKEs(CakeID);
            UpdatedCake.AvatarImage = CurCake.AvatarImage.ToArray();

            //Update UI with Data
            UpdateCakeFrame.DataContext = CurCake;
            AvatarImage.Source = AlternativeByteArrayToImageConveter.Convert(CurCake.AvatarImage);
            //Set Category text
            CategoryComboBox.ItemsSource = CATEGORies;
            foreach(var i in CATEGORies)
            {
                if (i.ID == CurCake.CatID)
                    CategoryComboBox.SelectedItem = i;
            }
        }

        private void AddAvatarImage_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            // Thiết đặt bộ lọc (filter) cho file hình ảnh
            var codecs = ImageCodecInfo.GetImageEncoders();
            var sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                screen.Filter = String.Format("{0}{1}{2} ({3})|{3}", screen.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }
            screen.Filter = String.Format("{0}{1}{2} ({3})|{3}", screen.Filter, sep, "All Files", "*.*");
            screen.Title = "Chọn ảnh lộ trình";
            screen.Multiselect = false;
            screen.RestoreDirectory = true;

            if (screen.ShowDialog() == true)
            {
                var path = screen.FileName;
                byte[] array = UnknownImageToByteArrayConverter.ImageToByteArray(path);
                AvatarImage.Source = AlternativeByteArrayToImageConveter.Convert(array);
                UpdatedCake.AvatarImage = array.ToArray();
            }
        }


        public void ShowCakeDetailFrame()
        {
            //Show CakeDetail Frame , Hide UpdateCake Frame
            SaveInfo.Visibility = Visibility.Collapsed;
            CakeDetailFrame.Visibility = Visibility.Visible;
            UpdateCakeFrame.Visibility = Visibility.Collapsed;
            AddImage.Visibility = Visibility.Collapsed;

        }

        public void ShowUpdateCakeFrame()
        {
            // Show Update Frame, hide CakeDetail Frame
            AddImage.Visibility = Visibility.Visible;
            CakeDetailFrame.Visibility = Visibility.Collapsed;
            UpdateCakeFrame.Visibility = Visibility.Visible;
            SaveInfo.Visibility = Visibility.Visible;
        }

        private bool CheckInputError()
        {
            bool check = true;
            if (Introduction.Text == "" || Description.Text == "" || SellPrice.Text == "" || BasePrice.Text == "")
            {
                check = false;
                MessageBox.Show("Tồn tại trường dữ liệu rỗng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (check == true)
                {
                    try
                    {
                        long sellprice = long.Parse(SellPrice.Text);
                        if (sellprice < 0)
                            throw new Exception("1");
                    }
                    catch (Exception ex)
                    {
                        check = false;
                        MessageBox.Show("Giá bán không hơp lệ\n Dữ liệu phải là dạng số và lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                if(check == true)
                {
                    try
                    {
                        long baseprice = long.Parse(BasePrice.Text);
                        if (baseprice < 0)
                            throw new Exception("1");
                    }
                    catch (Exception ex)
                    {
                        check = false;
                        MessageBox.Show("Giá bán không hơp lệ\n Dữ liệu phải là dạng số và lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            return check;
        }

        /// <summary>
        /// Quay lại giao diện Home Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComeBack_Click(object sender, RoutedEventArgs e)
        {
            if(isUpdate==0)//Update didn't happen
            {
                App.mainWindow.mainContentFrame.Content = App.homePage;
            }
            else
            {
                App.homePage = new Home();
                App.mainWindow.mainContentFrame.Content = App.homePage;
            }
        }
    }
}

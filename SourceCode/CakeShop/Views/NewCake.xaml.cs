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
    /// Interaction logic for AddNewCake.xaml
    /// </summary>
    /// 
    #region
    /// ViewModel for AddNewCake
    public class AddNewCakeViewModel
    {
        CakeShopDAO dao;

        public AddNewCakeViewModel()
        {
            dao = new CakeShopDAO();
        }
        public List<CATEGORY> GetCATEGORies()
        {
            return dao.CategoryList();
        }


    }
    #endregion
    public partial class NewCake : Page
    {
        CAKE curCake;
        AddNewCakeViewModel _mainvm;
        List<CATEGORY> categories;
        public NewCake()
        {
            InitializeComponent();
            Prepare();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Prepare()
        {
            curCake = new CAKE();
            _mainvm = new AddNewCakeViewModel();
            categories = _mainvm.GetCATEGORies();
            chosenCategory.ItemsSource = categories;
        }


        private void SaveCake_Click(object sender, RoutedEventArgs e)
        {
            bool check = CheckInputError();
            if (check == true)
            {
                StoreCakeData();
                RefreshDataInput();
                Prepare();
                MessageBox.Show("Thêm Cake thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StoreCakeData()
        {
            OurCakeShopEntities database = new OurCakeShopEntities();
            int index = (int)chosenCategory.SelectedIndex;
            CAKE cake = new CAKE
            {
                ID = 1000,
                Name = cakeName.Text,
                SellPrice = long.Parse(sellPrice.Text),
                BasePrice = long.Parse(basePrice.Text),
                InventoryNum = long.Parse(inventoryNumber.Text),
                Introduction = introduction.Text,
                Description = description.Text,
                AvatarImage = curCake.AvatarImage,
                CatID = categories[index].ID,
                DateAdded = DateTime.UtcNow
            };
            database.CAKEs.Add(cake);
            database.SaveChanges();
        }

        private void AddAvatarImage_Click(object sender, RoutedEventArgs e)
        {
            //Hiển thị cửa sổ chọn ảnh

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
                AvatarImage.ImageSource = AlternativeByteArrayToImageConveter.Convert(array);
                curCake.AvatarImage = array.ToArray();
            }
        }

        private bool CheckInputError()
        {
            bool check = true;
            if (AvatarImage.ImageSource == null)
            {
                check = false;
                MessageBox.Show("Bạn chưa đổi ảnh Avatar", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else if (introduction.Text == "" || description.Text == "" || cakeName.Text == "" || sellPrice.Text == "" ||
                    basePrice.Text == "" || inventoryNumber.Text == "")
            {
                check = false;
                MessageBox.Show("Bạn chưa nhập đủ thông tin", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (check == true)
                {
                    try
                    {
                        long sellprice = long.Parse(sellPrice.Text);
                        if (sellprice < 0)
                            throw new Exception("1");
                    }
                    catch (Exception ex)
                    {
                        check = false;
                        MessageBox.Show("Giá bán không hơp lệ\n Dữ liệu phải là dạng số và lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                if (check == true)
                {
                    try
                    {
                        long baseprice = long.Parse(basePrice.Text);
                        if (baseprice < 0)
                            throw new Exception("1");
                    }
                    catch (Exception ex)
                    {
                        check = false;
                        MessageBox.Show("Giá bán không hơp lệ\n Dữ liệu phải là dạng số và lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                if (check == true)
                {
                    try
                    {
                        long inventorynumber = long.Parse(inventoryNumber.Text);
                        if (inventorynumber < 0)
                            throw new Exception("1");
                    }
                    catch (Exception ex)
                    {
                        check = false;
                        MessageBox.Show("Giá gốc không hơp lệ\n Dữ liệu phải là dạng số và lớn hơn 0", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            return check;
        }

        private void RefreshDataInput()
        {
            cakeName.Text = "";
            chosenCategory.SelectedIndex = -1;
            inventoryNumber.Text = "";
            sellPrice.Text = "";
            basePrice.Text = "";
            introduction.Text = "";
            description.Text = "";
            AvatarImage.ImageSource = null;
            curCake = new CAKE();
        }

    }
}
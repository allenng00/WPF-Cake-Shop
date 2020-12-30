using System.Windows;

namespace CakeShop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            
        }

        
        //private void addImgButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (cakesComboBox.SelectedIndex >= 0)
        //    {
        //        //Hiển thị cửa sổ chọn ảnh

        //        var screen = new OpenFileDialog();
        //        // Thiết đặt bộ lọc (filter) cho file hình ảnh
        //        var codecs = ImageCodecInfo.GetImageEncoders();
        //        var sep = string.Empty;

        //        foreach (var c in codecs)
        //        {
        //            string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
        //            screen.Filter = String.Format("{0}{1}{2} ({3})|{3}", screen.Filter, sep, codecName, c.FilenameExtension);
        //            sep = "|";
        //        }
        //        screen.Filter = String.Format("{0}{1}{2} ({3})|{3}", screen.Filter, sep, "All Files", "*.*");
        //        screen.Title = "Chọn ảnh lộ trình";
        //        screen.Multiselect = false;
        //        screen.FilterIndex = 6;
        //        screen.RestoreDirectory = true;
        //        // Lưu hình ảnh
        //        if (screen.ShowDialog() == true)
        //        {
        //            var path = screen.FileName;
        //            Debug.WriteLine(path);
        //            //Đổi ảnh sang mảng byte
        //            var array = UnknownImageToByteArrayConverter.ImageToByteArray(path);
        //            //Luu anh lai
        //            var cake = cakesComboBox.SelectedItem as CAKE;
        //            cake.AvatarImage = array;
        //            var index = cakesComboBox.SelectedIndex;
        //            Debug.WriteLine(mainList[index].AvatarImage.ToString());
        //            dao.UpdateDatabase();
        //        }
        //    }
        //}


    }
}

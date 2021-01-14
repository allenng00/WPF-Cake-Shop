using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CakeShop
{
    class DateTimeToSpecifiedStyleString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = (DateTime)value;
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Chuyển đổi đường dẫn tuyệt đối sang tương đối
    /// </summary>
    class AbsolutePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string relative = value.ToString();
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = $"{folder}{relative}";
            return absolutePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class PictureHelper
    {
        public static BitmapImage GetImage(object obj)
        {
            try
            {
                if (obj == null || string.IsNullOrEmpty(obj.ToString())) return new BitmapImage();

                #region Picture

                byte[] data = (byte[])obj;

                MemoryStream strm = new MemoryStream();

                strm.Write(data, 0, data.Length);

                strm.Position = 0;

                Image img = Image.FromStream(strm);

                BitmapImage bi = new BitmapImage();

                bi.BeginInit();

                MemoryStream ms = new MemoryStream();

                img.Save(ms, ImageFormat.Bmp);

                ms.Seek(0, SeekOrigin.Begin);

                bi.StreamSource = ms;

                bi.EndInit();

                return bi;

                #endregion
            }
            catch
            {
                return new BitmapImage();
            }
        }

        //public static string PathReturner(ref string name)
        //{
        //    string filepath = "";
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    openFileDialog.Multiselect = false;
        //    openFileDialog.Filter = @"Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.gif;*.png;*.jpg";
        //    openFileDialog.RestoreDirectory = true;
        //    openFileDialog.Title = @"Please select an image file to upload.";

        //    MiniWindow miniWindow = new MiniWindow();
        //    miniWindow.Show();

        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        filepath = openFileDialog.FileName;
        //        name = openFileDialog.SafeFileName;
        //    }

        //    miniWindow.Close();
        //    miniWindow.Dispose();
        //    return filepath;
        //}

        public static string Encryptor(this string safeName)
        {
            string extension = Path.GetExtension(safeName);

            string newFileName = String.Format(@"{0}{1}{2}", Guid.NewGuid(), DateTime.Now.ToString("MMddyyyy(HHmmssfff)"), extension);
            newFileName = newFileName.Replace("(", "").Replace(")", "");
            return newFileName;
        }


        public static Bitmap ByteToBitmap(this byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }

        public static byte[] BitmapToByte(this Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;


        }
    }

    /// <summary>
    /// Chuyển đổi mảng byte sang bitmap Codebehind
    /// </summary>
    public class AlternativeByteArrayToImageConveter
    {
        public static BitmapImage Convert(byte[] imageArray)
        {
            using (var stream = new MemoryStream(imageArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
    }

    /// <summary>
    /// Chuyển đổi mảng byte sang bitmap UI
    /// </summary>
    public class ByteArrayToImageConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Byte[] array = value as Byte[];

            using (var stream = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
            // Nếu ảnh null
            //else
            //{
            //    var path = AppDomain.CurrentDomain.BaseDirectory;
            //    var image = new BitmapImage();

            //    image.BeginInit();
            //    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            //    image.CacheOption = BitmapCacheOption.OnLoad;
            //    image.UriSource = new Uri(path + @"Data\Images\cart.png");
            //    image.EndInit();
            //    image.Freeze();

            //    return image;

            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UnknownImageToByteArrayConverter
    {
        public static byte[] ImageToByteArray(string sourcePath)
        {
            var result = new byte[] { };
            var image = new BitmapImage(
               new Uri(sourcePath, UriKind.Absolute)
                   );
            var encoder = CheckEncoder(sourcePath);
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                result = stream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static BitmapEncoder CheckEncoder(string filepath)
        {
            BitmapEncoder result = null;
            var img = System.Drawing.Image.FromFile(filepath);

            // Nếu là file chuẩn JEPG
            if (IsJEPGImage(img) == true)
            {
                result = new JpegBitmapEncoder();
            }
            // Nếu là file chuẩn PNG
            else if (IsPNGImage(img) == true)
            {
                result = new PngBitmapEncoder();
            }
            // Nếu là file chuẩn BMP
            else if (IsBMPImage(img) == true)
            {
                result = new BmpBitmapEncoder();
            }
            // Nếu là file chuẩn TIFFI
            else if (IsTIFFImage(img) == true)
            {
                result = new TiffBitmapEncoder();
            }
            // Nếu là file chuẩn GIF
            else if (IsGIFImage(img) == true)
            {
                result = new GifBitmapEncoder();
            }

            return result;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại JEPG hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsJEPGImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Jpeg);
            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại BMP hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsBMPImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Bmp);

            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại PNG hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsPNGImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Png);

            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại TIFFI hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsTIFFImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Tiff);

            return check;
        }

        /// <summary>
        /// Hàm kiểm tra hình là có loại GIF hay không
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static bool IsGIFImage(System.Drawing.Image img)
        {
            var check = false;
            check = img.RawFormat.Equals(ImageFormat.Gif);

            return check;
        }
    }

}
